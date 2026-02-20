using System.Collections;
using UnityEngine;

public abstract class BaseEnemyAttackState : AIState<EnemyBrainContext>
{
    protected Coroutine combatCoroutine;
    protected Coroutine cooldownCoroutine;

    protected EnemyFOVController fov;
    protected HumanoidAIMotor motor;

    protected EnemyCombatHandler combatHandler;
    protected HumanoidAgentController agentController;
    protected IHumanoidMeleeCombat combatController;

    protected Transform self;
    protected Transform target;
    protected float distance;

    protected abstract void Init();

    public override void Enter()
    {
        Init();
        self = context.self;
        fov = context.fov;
        motor = context.motor;
        agentController = context.agentController;

        combatCoroutine = null;
        combatHandler = context.stateTracker.combatHandler;
        combatHandler.ResetCombatState();
        combatController = context.combat;
    }

    public override AIStateResult Run()
    {
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        target = fov.currentTarget.GetOrigin();

        combatHandler.UpdateDodgeChance();
        if (combatHandler.IsStrafeBlocked())
            combatHandler.UpdateBlockStrafeTimer();

        distance = Vector3.Distance(self.position, target.position);

        if (!combatHandler.IsCombatDistance(distance) || !fov.IsTargetVisible())
            return AIStateResult.Chase;

        if (combatCoroutine != null || cooldownCoroutine != null)
            return AIStateResult.None;

       
        bool canSprint = combatHandler.IsRunningDistance(distance);
        motor.IsSprinting = canSprint;

        bool canLockOn = combatHandler.IsInAttackRange(distance);

        if (!canLockOn)
        {
            motor.ResetLockTarget();
            motor.SetStrafe(false);
        }
        else
        {
            motor.SetLockTarget(fov.currentTarget.GetAimTransform());
            motor.SetStrafe(true);
        }

        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);
        if (!canReach)
            return AIStateResult.Wait;


        if (!combatHandler.IsInAttackRange(distance) || !fov.IsTargetVisible())
        {

            motor.MoveCharacter(target.position);
            HandleDefense(true);
            return AIStateResult.None;
        }


        HandleDefense(false);

        return GetNextDecision();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        combatCoroutine = null;
        cooldownCoroutine = null;

        HandleDefense(false);
        motor.ResetLockTarget();
        motor.SetStrafe(false);
    }



    protected void FinishCombatAction()
    {
        //motor.ResetLockTarget();
        combatCoroutine = null;
        combatHandler.SetCanAttack(false);
        cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }



    #region Coroutines
    protected IEnumerator MeleeCoroutine(int punchesCount)
    {

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

    protected IEnumerator DodgeCoroutine(Transform target)
    {
        motor.IsDodging = true;
        combatHandler.ResetDodgeChance();

        Vector3 fromTarget = (self.position - target.position).normalized;
        motor.Dodge(fromTarget);

        while (motor.IsDodging)
            yield return null;

        FinishCombatAction();
    }

    protected IEnumerator CooldownCoroutine()
    {
        float elapsed = 0f;
        float max = Random.Range(combatHandler.GetMinAttackCooldown(), combatHandler.GetMaxAttackCooldown());

        motor.StopMovement();
        HandleDefense(true);

        while (elapsed < max && !ShouldExitCooldown())
        {
            HandleCooldown();
            elapsed += Time.deltaTime;
            yield return null;
        }


        combatHandler.SetCanAttack(true);
        HandleDefense(false);
        cooldownCoroutine = null;
    }
    #endregion

    #region Abstract Methods
    protected abstract void HandleDefense(bool willDefend);
    protected abstract void HandleAttack(Transform target);
    protected abstract bool ShouldExitCooldown();
    protected abstract void HandleCooldown();
    #endregion


    #region Virtual Methods
    protected virtual AIStateResult GetNextDecision()
    {
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
    #endregion
}
