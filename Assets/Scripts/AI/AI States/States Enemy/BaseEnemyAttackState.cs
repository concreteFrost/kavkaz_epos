using System.Collections;
using UnityEngine;

public abstract class BaseEnemyAttackState : AIState<EnemyBrainContext>
{
    protected Coroutine combatCoroutine;
    protected Coroutine cooldownCoroutine;

    protected EnemyFOVController fov;
    protected HumanoidAIMotor motor;

    [HideInInspector] public EnemyCombatHandler combatHandler;
    protected HumanoidAgentController agentController;
    protected IHumanoidMeleeCombat combatController;
    protected HumanoidAICombatActions combatActions;  

    protected Transform self;
    protected Transform target;
    protected float distanceToTarget;

    protected CharacterAudioManager audioManager;

    protected CombatMode combatMode; 

    public abstract void Init();

    public override void Enter()
    {
        
        self = context.self;
        fov = context.fov;
        motor = context.motor;
        agentController = context.agentController;
        audioManager = context.audioManager;

        combatCoroutine = null;
        combatHandler = context.stateTracker.combatHandler;
        combatHandler.ResetCombatState();
        combatController = context.combat;

        if(combatActions == null)
        {
            combatActions = new HumanoidAICombatActions(this);
        }

        Init();
    }

    public override AIStateResult Run()
    {
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        target = fov.currentTarget.GetOrigin();

        // --- базовая логика до inRange ---
        combatHandler.UpdateDodgeChance();
        if (combatHandler.IsStrafeBlocked())
            combatHandler.UpdateBlockStrafeTimer();

        distanceToTarget = Vector3.Distance(self.position, target.position);

        if (combatHandler.IsChaseDistance(distanceToTarget) || !fov.IsTargetVisible())
            return AIStateResult.Chase;

        if (cooldownCoroutine != null || combatCoroutine !=null)
        {
            motor.SetLockTarget(fov.currentTarget.GetAimTransform());
            motor.SetStrafe(true);
            motor.RotateToTarget(target.position);
            return AIStateResult.None;
        }
          

        combatHandler.UpdateDecideToRunTimer();

        motor.SetStrafe(false);
        motor.ResetLockTarget();
        motor.IsSprinting = combatHandler.willDecideToRun;
        //motor.ResetLockTarget();
        //motor.SetStrafe(false);
      
        
        var agentTypeId = context.agentController.agent.agentTypeID;

        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position,agentTypeId);
        if (!canReach)
            return AIStateResult.Wait;

       
        return HandleCombatBehavior();
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

    protected void TryPlayVoice(System.Action playVoice)
    {
        if (!combatHandler.CanDoVoice)
            return;

        playVoice();
        combatHandler.StartVoiceCooldown();
    }


    public virtual AIStateResult HandleCombatBehavior()
    {
        if(motor.IsJumping) return AIStateResult.None;

        bool inRange = distanceToTarget < combatHandler.GetAttackDistance(combatMode);

        if (!inRange || !fov.IsTargetVisible())
        {
            motor.MoveCharacter(target.position);
            HandleDefense(true);
            return AIStateResult.None;
        }

        HandleDefense(false);

        return GetNextDecision();
    }



    protected virtual void FinishCombatAction()
    {
        //motor.ResetLockTarget();
       
        combatCoroutine = null;
        combatHandler.SetCanAttack(false);
        combatHandler.ResetDecideRunTimer();    
        cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }




    #region Coroutines

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
    public abstract void HandleDefense(bool willDefend);
    public abstract void HandleAttack(Transform target);
    public abstract bool ShouldExitCooldown();
    public abstract void HandleCooldown();
    #endregion


    #region Virtual Methods
    public virtual AIStateResult GetNextDecision()
    {
        switch (combatHandler.GetNextDecision())
        {
            case CombatTransition.Attack:
                HandleAttack(target);
                break;
            case CombatTransition.Dodge:
                combatCoroutine = combatActions.StartDodge(motor, self, target, combatHandler, FinishCombatAction);
                break;

            case CombatTransition.Strafe:
                return AIStateResult.Strafe;
        }

        return AIStateResult.None;
    }
    #endregion
}
