using System.Collections;
using UnityEngine;

public class EnemyAttackState : AIState<EnemyBrainContext>
{

    // runtime
    private Coroutine comboCoroutine;
    private Transform target;
    private float distance;

    EnemyFOVController fov;
    EnemyCombatHandler combatHandler;
   

    HumanoidAIMotor motor;


    public override void Enter()
    {

        fov = context.fov;

        motor = context.motor;
        combatHandler = context.stateTracker.combatHandler;

        combatHandler.ResetAttackState();
        comboCoroutine = null;

        if (context.fov.currentTarget == null)
            return;

        target = context.fov.currentTarget.GetOrigin();
    }

    public override AIStateResult Run()
    {

        var self = context.self;

        //цель потеряна
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        //во время доджа ничего не делаем
        if (motor.IsDodging)
            return AIStateResult.None;

        //обновление восстановления доджа
        combatHandler.UpdateDodgeCooldown();

        //дистанция
        distance = Vector3.Distance(self.position, target.position);

        //изменения поведения вращения к цели
        if (distance < 2f)
        {
            motor.SetLockTarget(fov.currentTarget.GetAimTransform());
        }
        else
        {
            motor.ResetLockTarget();
        }

        motor.IsSprinting = combatHandler.IsRunningDistance(distance);

        bool canReach = NavAgentUtils.HasCompletePath(
            self.position,
            target.position
        );

        //цель недостижима
        if (!canReach && !combatHandler.IsInAttackRange(distance))
            return AIStateResult.Wait;

        //цель вышла из боевой дистанции
        if (!combatHandler.IsComboDistance(distance))
            return AIStateResult.Chase;

        //если идёт комбо — не вмешиваемся
        if (combatHandler.IsComboRuning())
            return AIStateResult.None;

        //подходим к цели
        if (!combatHandler.IsInAttackRange(distance))
        {
            motor.MoveCharacter(target.position);
            return AIStateResult.None;
        }

        // 9. боевое решение
        combatHandler.UpdateCombatCooldown();

        if (comboCoroutine == null)
        {

            bool willAttack = Random.value > combatHandler.GetDodgeChance();
            comboCoroutine = StartCoroutine(
                CombatDecision(target, willAttack)
            );
        }
        else
        {

            motor.StopMovement();
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }

        motor.ResetLockTarget();

    }

    // ===== Combat logic =====
    private IEnumerator CombatDecision(Transform target, bool willAttack)
    {
        if (willAttack)
        {
            int punches = Random.Range(1, 5);
            yield return ComboCoroutine(punches);
        }
        else
        {
            yield return DodgeCoroutine(target);
        }

        comboCoroutine = null;

        combatHandler.ResetCombatCooldown(0.5f, 1f);
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
    }

    private IEnumerator ComboCoroutine(int punchesCount)
    {

        var combat = context.combat;

        combatHandler.SetComboRunning(true);

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

        combatHandler.SetComboRunning(false);
    }


}
