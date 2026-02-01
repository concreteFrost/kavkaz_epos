using System.Collections;
using UnityEngine;

public class EnemyAttackState : AIState<EnemyBrainContext>
{

    // runtime
    private Coroutine combatCoroutine;
    private float distance;

    EnemyFOVController fov;
    EnemyCombatHandler combatHandler;

    HumanoidCombatInventory inventory;
    HumanoidCombatController combatController;

    HumanoidAIMotor motor;


    public override void Enter()
    {

        fov = context.fov;

        motor = context.motor;
        combatHandler = context.stateTracker.combatHandler;
        combatController = context.combat;
        inventory = context.inventory;

        combatHandler.ResetCombatState();
        combatCoroutine = null;

        if (context.fov.currentTarget == null)
            return;

        
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());
    }

    public override AIStateResult Run()
    {
        var self = context.self;
        var targetEntity = fov.currentTarget;

        // 1. Цель потеряна
        if (targetEntity == null)
        {
           
            return AIStateResult.Idle;
        }

        var target = targetEntity.GetOrigin();

        // 2. Если враг в додже — не делаем ничего
        if (motor.IsDodging)
            return AIStateResult.None;

        // Обновляем кулдаун для следующего шанса на Dodge
        combatHandler.UpdateDodgeCooldown();

        // 3. Дистанция до цели
        distance = Vector3.Distance(self.position, target.position);

        //// 4. Спринт, если нужно отдаляться
        motor.IsSprinting = combatHandler.IsRunningDistance(distance);

        // 5. Проверка доступности пути к цели
        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);
        if (!canReach && !combatHandler.IsInAttackRange(distance))
            return AIStateResult.Wait;

        // 6. Если цель вышла из боевой дистанции — преследуем
        if (!combatHandler.IsCombatDistance(distance))
            return AIStateResult.Chase;

        // 7. Если идёт комбо — не вмешиваемся
        if (combatCoroutine != null)
            return AIStateResult.None;

        // 8. Подходим к цели, если ещё не в атаке
        if (!combatHandler.IsInAttackRange(distance))
        {
            motor.MoveCharacter(target.position);

            //поднимаем щит на подходе к цели
            combatHandler.ToggleShield(true, inventory, combatController);
            return AIStateResult.None;
        }


        // 9. Проверяем возможность атаки (учитываем кулдаун и другие ограничения)
        bool canAttack = combatHandler.CanAttack();

        if (!canAttack)
        {
            motor.StopMovement();
            combatHandler.UpdateCombatCooldown();

            return AIStateResult.None;
        }

        combatHandler.ToggleShield(false, inventory, combatController);

        // 10. Боевой выбор: атака или стрейф
        switch (combatHandler.GetNextDecision())
        {

            case CombatTransition.Attack:
                HandleAttack(target);
                break;
            case CombatTransition.Dodge:
                StartCoroutine(DodgeCoroutine(target));
                break;

            case CombatTransition.Strafe:
                return AIStateResult.Strafe;
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {
        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;
        }

        combatHandler.ToggleShield(false,inventory,combatController);
        motor.ResetLockTarget();

    }


    private void FinishCombatAction()
    {
       
        combatHandler.ResetCombatCooldown(0.2f, 0.7f);
        combatCoroutine = null;
    }

    // ===== Combat logic =====
    private void HandleAttack(Transform target)
    {
        if (combatHandler.WillPowerAttack())
            combatCoroutine = StartCoroutine(PowerAttackCoroutine());

        else
            combatCoroutine = StartCoroutine(ComboCoroutine());
    }

    private IEnumerator DodgeCoroutine(Transform target)
    {
        var motor = context.motor;
        var tracker = context.stateTracker;

        motor.IsDodging = true;

        combatHandler.ResetDodgeChance();

        Vector3 fromTarget =
            (context.self.position - target.position).normalized;

        motor.Dodge(fromTarget);

        while (motor.IsDodging)
            yield return null;

        FinishCombatAction();


    }

    private IEnumerator PowerAttackCoroutine()
    {
        Debug.Log("power attack!");

        var combat = context.combat;

        combat.PerformPowerAttack();

        while (combat.isAttacking)
        {
            yield return null;  
        }

        combatHandler.ResetPowerAttackChance();
        FinishCombatAction();
    }

    private IEnumerator ComboCoroutine()
    {

        int punchesCount = Random.Range(1, 5);
        var combat = context.combat;

        //combatHandler.SetComboRunning(true);

        int executedAttacks = 0;

        void OnAttackEnd() => executedAttacks++;

        combat.OnAttackEnd += OnAttackEnd;

        combat.PerformAttack();

        while (executedAttacks < punchesCount - 1 &&
               distance <= combatHandler.GetAttackDistanceWithOffset())
        {
            yield return new WaitForSeconds(
                combat.attackBufferTime * 0.9f
            );
            combat.PerformAttack();
        }

        combat.OnAttackEnd -= OnAttackEnd;

        //combatHandler.SetComboRunning(false);
        FinishCombatAction();
    }


}
