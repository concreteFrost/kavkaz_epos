using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMotor locomotion;
    IHumanoidCombat combatController;
    IDamagable damageController;
    CharacterStatsController statsController;
    ITargetLocker targetLocker;
    ICollector interact;
    PlayerClimbing climbing;
    PlayerActionGuards actionGuards;
    PlayerAnimatorController animatorController;
    AgressivePushController pushSource;

    HumanoidStats stats;

    private void Update()
    {
        UpdateMotor();
        TryClimb();
    }

    private void FixedUpdate()
    {
        UpdateAnimator();
    }


    public void Init(PlayerControllerService provider)
    {

        animatorController = provider.animatorController;
        locomotion = provider.controller;
        damageController = provider.damageController;
        combatController = provider.combatController;
       
        interact = provider.interact;
        climbing = provider.climbing;
        statsController = provider.statsManager.Controller;

        stats = statsController.stats;
        targetLocker = provider.locker;
        pushSource = provider.pushSource;   

        actionGuards = new PlayerActionGuards(locomotion, combatController, statsController.stats, damageController, climbing,targetLocker);
        climbing.Init(locomotion, actionGuards,animatorController );

    }

    public void MoveAndRotate(Vector3 dir)
    {
        MoveCharacter(dir);
        RotateCharacter(dir);
    }

    private void UpdateMotor()
    {
        if (!actionGuards.CanUseMotor()) return;

        locomotion.UpdateMotor(stats.jumpHeight);
    }

    private void UpdateAnimator()
    {
        locomotion.UpdateAnimatorLocomotion();
    }

    #region Locomotion
    /// <summary>
    /// Наземное движение
    /// </summary>
    /// <param name="dir">Направление ввода</param>
    private void MoveCharacter(Vector3 dir)
    {
        if (!actionGuards.CanMove())
        {
            locomotion.StopMovement();
            return;
        }

        float baseSpeed = locomotion.IsSprinting
            ? stats.runningSpeed
            : stats.walkSpeed;


        locomotion.moveSpeed = Mathf.Lerp(
            locomotion.moveSpeed,
            baseSpeed,
            locomotion.movementSmooth * Time.deltaTime
        );

        locomotion.MoveCharacter(dir);
    }


    /// <summary>
    /// Поворот персонажа в сторону ввода или в сторону цели
    /// </summary>
    /// <param name="input">Направление ввода</param>
    private void RotateCharacter(Vector3 input)
    {

        if (!actionGuards.CanRotate(input)) return;

        // lock-on активен → вращаем к цели
        if (locomotion.rotateTarget != null)
        {

            locomotion.RotateToTarget(locomotion.rotateTarget.position);
            return;
        }
        // иначе — вращаемся по движению игрока
        locomotion.RotateToDirection(input);
    }

    private void Dodge(Vector3 dir)
    {
        if (!actionGuards.CanDodge()) return;

        locomotion.Dodge(dir);
        statsController.ReduceStamina(stats.statsSO.staminaDodgeReducePenalty);

    }

    private void Jump()
    {
        if (!actionGuards.CanJump()) return;

        locomotion.Jump(stats.jumpTimer);
        statsController.ReduceStamina(stats.statsSO.staminaJumpReducePenalty);
    }

    public void HandleJumpOrDodge(Vector3 dir)
    {
        if (locomotion.IsStrafing)
        {
            Dodge(dir);
            return;
        }

        Jump();
    }

    public void Sprint(bool sprintHeld)
    {

        locomotion.IsSprinting = actionGuards.CanSprint(sprintHeld);

        if (locomotion.IsSprinting)
        {
            statsController.ReduceStamina(stats.statsSO.staminaRunReducePenalty);
        }

    }

    public void SetStrafe(bool isStrafing)
    {
        locomotion.SetStrafe(isStrafing);
    }

    #endregion

    #region Target Lock

    public void SetLockTarget(Transform target)
    {
        locomotion.rotateTarget = target;
        //locomotion.IsStrafing = true;
    }

    public void ResetLockTarget()
    {
        locomotion.rotateTarget = null;
        //locomotion.IsStrafing= false;   
    }

    #endregion

    #region Combat
    public void PerformAttack()
    {
        if (!actionGuards.CanAttack()) return;

        combatController.PerformAttack();

    }

    public void PerformPowerAttack()
    {
        if(!actionGuards.CanAttack()) return;

        combatController.PerformPowerAttack();
    }

    public void ThrowWeapon()
    {
        if (!actionGuards.CanThrowWeapon()) return;

        combatController.ThrowWeapon();
    }
    public void ThrowShield()
    {
        if (!actionGuards.CanThrowWeapon()) return;

        combatController.ThrowShield();
    }

    public void PerformBlock()
    {
        if (!actionGuards.CanBlock()) return;
        combatController.PerformBlock();
    }

    public void CancelBlock()
    {
        combatController.CancelBlock();
    }

    public void PerformPush()
    {
        if (!actionGuards.CanAttack()) return;

        pushSource.TriggerPushAnimation();  
    }

    #endregion

    #region Interaction
    public void Interact()
    {
        //не взаимодействуем если игрок атакует
        if (!actionGuards.CanInteract()) return;

        interact.StartInteracion();
    }
    #endregion

    #region Climbing

    void TryClimb()
    {
        if (!actionGuards.CanEnterClimb()) return;

        climbing.TryToClimb();
    }

    public void ExitClimb()
    {
        climbing.ExitClimb();
    }

    #endregion
}
