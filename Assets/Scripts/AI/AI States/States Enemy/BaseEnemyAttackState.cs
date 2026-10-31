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

    protected IWeapon CurrentWeapon => context.weaponSetter.CurrentWeapon;

    protected bool usePowerAttack;

    private bool hasAttackChoice;
    private IWeapon choiceWeapon;
    private Transform choiceTarget;

    public abstract void Init();

    public override void Enter()
    {
        
        self = context.self;
        fov = context.fov;
        motor = context.motor;
        agentController = context.agentController;
        audioManager = context.audioManager;

        combatCoroutine = null;
        ResetMeleeAttackChoice();
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

        // Начатое действие заканчивается до перехода в погоню; потеря видимости не обрывает комбо.
        if (combatCoroutine != null)
        {
            motor.SetLockTarget(fov.currentTarget.GetAimTransform());
            motor.SetStrafe(true);
            motor.RotateToTarget(target.position);
            return AIStateResult.None;
        }

        if (combatHandler.IsChaseDistance(distanceToTarget) || !fov.IsTargetVisible())
            return AIStateResult.Chase;

        // При возвращении из погони продолжаем оставшуюся перезарядку, а не начинаем бой заново.
        if (cooldownCoroutine == null && combatHandler.IsAttackCooldownActive)
            cooldownCoroutine = StartCoroutine(CooldownCoroutine());

        if (cooldownCoroutine != null)
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
        if (combatCoroutine != null)
        {
            if (!combatHandler.IsAttackCooldownActive)
                combatHandler.StartAttackCooldown();

            combatController.ForceAttackCancel();
            context.emitter?.EndEmit();
        }

        StopAllCoroutines();
        combatCoroutine = null;
        cooldownCoroutine = null;
        ResetMeleeAttackChoice();
       
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

        WeaponAttack selectedAttack = null;
        bool inRange;
        if (combatMode == CombatMode.Melee)
        {
            selectedAttack = PrepareMeleeAttackChoice();
            inRange = selectedAttack != null && selectedAttack.IsInRange(distanceToTarget);
        }
        else
        {
            inRange = distanceToTarget < combatHandler.GetEmitAttackDistance();
        }

        if (!inRange || !fov.IsTargetVisible())
        {
            // Сближаемся для выбранной атаки. Доступность другой атаки не заставляет останавливаться.
            if (combatMode == CombatMode.Melee && !NeedsApproach(selectedAttack))
                motor.StopMovement();
            else
                motor.MoveCharacter(target.position);

            HandleDefense(true);
            return AIStateResult.None;
        }

        motor.StopMovement();
        HandleDefense(false);

        AIStateResult result = GetNextDecision();
        ResetMeleeAttackChoice();
        return result;
    }



    /// <summary>Возвращает первый удар серии, а не оставшуюся от прошлого боя текущую атаку.</summary>
    protected WeaponAttack GetFirstMeleeAttack()
    {
        var attackSet = CurrentWeapon?.WeaponData()?.attackSet;
        if (attackSet == null || attackSet.attackList == null || attackSet.attackList.Count == 0)
            return null;

        return attackSet.attackList[0];
    }

    protected bool IsFirstMeleeAttackInRange()
    {
        var attack = GetFirstMeleeAttack();
        return attack != null && attack.IsInRange(distanceToTarget);
    }

    /// <summary>Выбирает тип атаки один раз на подход, сохраняя решение до начала действия.</summary>
    private WeaponAttack PrepareMeleeAttackChoice()
    {
        IWeapon weapon = CurrentWeapon;
        if (choiceWeapon != weapon || choiceTarget != target)
            ResetMeleeAttackChoice();

        var firstAttack = GetFirstMeleeAttack();
        var powerAttack = weapon?.WeaponData()?.attackSet?.powerAttack;
        if (!hasAttackChoice)
        {
            choiceWeapon = weapon;
            choiceTarget = target;
            hasAttackChoice = true;

            // Дальняя атака не становится обязательной только потому, что её диапазон уже доступен.
            usePowerAttack = powerAttack != null && combatHandler.WillPowerAttack();
        }

        // Если цель подошла слишком близко для сильного удара, продолжаем обычной серией без нового броска.
        if (usePowerAttack &&
            (powerAttack == null || distanceToTarget < powerAttack.minAttackDistance))
            usePowerAttack = false;

        return usePowerAttack ? powerAttack : firstAttack;
    }

    private void ResetMeleeAttackChoice()
    {
        hasAttackChoice = false;
        choiceWeapon = null;
        choiceTarget = null;
        usePowerAttack = false;
    }

    private bool NeedsApproach(WeaponAttack attack)
    {
        return attack != null &&
               attack.maxAttackDistance > 0f &&
               attack.maxAttackDistance >= attack.minAttackDistance &&
               distanceToTarget > attack.maxAttackDistance;
    }

    protected virtual void FinishCombatAction()
    {
        //motor.ResetLockTarget();
       
        combatCoroutine = null;
        combatHandler.StartAttackCooldown();
        combatHandler.ResetDecideRunTimer();    
        cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }




    #region Coroutines

    protected IEnumerator CooldownCoroutine()
    {
        motor.StopMovement();
        HandleDefense(true);

        // Даже при досрочном завершении вызывающий код должен успеть сохранить ссылку на корутину.
        yield return null;
        while (combatHandler.IsAttackCooldownActive && !ShouldExitCooldown())
        {
            HandleCooldown();
            yield return null;
        }


        FinishCooldown();
    }

    /// <summary>Завершает перезарядку и освобождает связанные с ней действия состояния.</summary>
    protected virtual void FinishCooldown()
    {
        combatHandler.FinishAttackCooldown();
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
                if (combatMode == CombatMode.Melee)
                {
                    var attack = PrepareMeleeAttackChoice();
                    if (attack == null || !attack.IsInRange(distanceToTarget))
                        break;
                }

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
