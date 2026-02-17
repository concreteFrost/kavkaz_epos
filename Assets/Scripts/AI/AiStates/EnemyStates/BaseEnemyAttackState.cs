using System.Collections;
using UnityEngine;

/// <summary>
/// Базовое состояние атаки, универсальное для всех типов врагов.
/// </summary>
public abstract class BaseEnemyAttackState : AIState<EnemyBrainContext>
{
    // runtime
    protected Coroutine combatCoroutine;
    protected float distance;

    protected EnemyFOVController fov;
    protected HumanoidAIMotor motor;
    protected EnemyStateTracker stateTracker;

    // Эти поля могут быть null для магов
    protected EnemyCombatHandler combatHandler;
    protected HumanoidAgentController agentController;
    protected IHumanoidMeleeCombat combatController;

    protected Transform self;
    protected Transform target;

    protected abstract void Init();

    public override void Enter()
    {
        Init();

        self = context.self;
        fov = context.fov;
        motor = context.motor;
        stateTracker = context.stateTracker;
        agentController = context.agentController;

        combatCoroutine = null;

        // Если есть мили-комбат, получаем ссылки
        combatHandler = stateTracker.combatHandler;
        combatHandler.ResetCombatState();

        combatController = context.combat;


    }

    public override AIStateResult Run()
    {
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        Transform self = context.self;
        target = fov.currentTarget.GetOrigin();


        if (motor.IsDodging)
            return AIStateResult.None;

        combatHandler.UpdateDodgeCooldown();


        if (combatHandler.IsStrafeBlocked())
        {
            combatHandler.UpdateBlockStrafeTimer();
        }
            

        distance = Vector3.Distance(self.position, target.position);

        motor.IsSprinting = combatHandler.IsRunningDistance(distance);

        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);

        if (!canReach && !combatHandler.IsInAttackRange(distance))
            return AIStateResult.Wait;

        if (!combatHandler.IsCombatDistance(distance))
            return AIStateResult.Chase;

        if (combatCoroutine != null)
            return AIStateResult.None;

        // 8. Подходим к цели, если ещё не в атаке
        if (!combatHandler.IsInAttackRange(distance))
        {
            motor.MoveCharacter(target.position);
            motor.ResetLockTarget();

            //поднимаем щит на подходе к цели
            HandleDefense(true);
            return AIStateResult.None;
        }

        motor.SetLockTarget(fov.currentTarget.GetAimTransform());

        // 9. Проверяем возможность атаки (учитываем кулдаун и другие ограничения)
        bool canAttack = combatHandler.CanAttack();

        if (!canAttack)
        {
            return CantAttackResult();
        }

       
        HandleDefense(false);
        HandleAttack(target);

        return AIStateResult.None;  


    }

    public override void Exit()
    {
        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;
        }

        HandleDefense(false);
        motor.ResetLockTarget();

    }


    protected void FinishCombatAction()
    {
        combatHandler.ResetCombatCooldown();
        combatCoroutine = null;
    }

    protected IEnumerator DodgeCoroutine(Transform target)
    {
        motor.IsDodging = true;
        combatHandler.ResetDodgeChance();

        Vector3 fromTarget = (context.self.position - target.position).normalized;
        motor.Dodge(fromTarget);

        while (motor.IsDodging)
            yield return null;

        FinishCombatAction();
    }


    #region Handlers 


    protected abstract void HandleDefense(bool willDefend);

    protected abstract AIStateResult CantAttackResult();

    protected abstract void HandleAttack(Transform target);

    protected AIStateResult HandleCombatDecision(Transform target)
    {

        // Выбор атаки или стрейфа
        switch (combatHandler.GetNextDecision())
        {
            case CombatTransition.Attack:
                HandleAttack(target);
                break;

            case CombatTransition.Dodge:
                combatCoroutine = StartCoroutine(DodgeCoroutine(target));
                break;

            case CombatTransition.Strafe:
                return AIStateResult.Strafe;
        }

        return AIStateResult.None;
    }

    protected IEnumerator ComboCoroutine()
    {
        int punchesCount = Random.Range(1, 5);
        int executedAttacks = 0;

        void OnAttackEnd() => executedAttacks++;

        combatController.OnAttackEnd += OnAttackEnd;
        combatController.PerformAttack();

        while (executedAttacks < punchesCount - 1 &&
               distance <= combatHandler.GetAttackDistanceWithOffset())
        {
            yield return new WaitForSeconds(combatController.AttackBufferTime * 0.9f);
            combatController.PerformAttack();
        }

        combatController.OnAttackEnd -= OnAttackEnd;
        FinishCombatAction();
    }


    #endregion



}
