using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMotor motor;
    PlayerActionGuards actionGuards;
    CharacterStatsController stats;

    IEmitter emitController;

    IHumanoidMeleeCombat combatController;
    ICollector interact;
    
    IClimber climbing;
    IPushSource pushSource;

    private void Update()
    {
        UpdateMotor();
        TryClimb();
    }

    private void FixedUpdate()
    {
        UpdateAnimator();
    }

    public void Init(
        PlayerMotor motor,   
        IHumanoidMeleeCombat combatController,
        ICollector interaction,
        IClimber climbing,
        PlayerActionGuards actionGuards,
        CharacterStatsController stats,
        IPushSource pushSource,
        IEmitter emitController
      
        )
    {
  
        this.motor = motor;
        this.combatController = combatController;
        this.interact = interaction;
        this.climbing = climbing;
        this.stats = stats;

        this.pushSource = pushSource;
        this.actionGuards = actionGuards;

        this.emitController = emitController;


    }

    public void MoveAndRotate(Vector3 dir)
    {
        MoveCharacter(dir);
        RotateCharacter(dir);
    }

    private void UpdateMotor()
    {
        if (!actionGuards.CanUseMotor()) return;

        motor.UpdateMotor(stats.jumpHeight);
    }

    private void UpdateAnimator()
    {
        motor.UpdateAnimatorLocomotion();
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
            motor.StopMovement();
            return;
        }

        stats.Speed.SetSprint(motor.IsSprinting);
        stats.Speed.Tick(Time.deltaTime);

        motor.moveSpeed = stats.Speed.Current;
        motor.MoveCharacter(dir);
    }



    /// <summary>
    /// Поворот персонажа в сторону ввода или в сторону цели
    /// </summary>
    /// <param name="input">Направление ввода</param>
    private void RotateCharacter(Vector3 input)
    {

        if (!actionGuards.CanRotate(input)) return;

        // lock-on активен → вращаем к цели
        if (motor.rotateTarget != null)
        {

            motor.RotateToTarget(motor.rotateTarget.position);
            return;
        }
        // иначе — вращаемся по движению игрока
        motor.RotateToDirection(input);
    }

    private void Dodge(Vector3 dir)
    {
        if (!actionGuards.CanDodge()) return;

        motor.Dodge(dir);
        stats.Stamina.Reduce(stats.statsSO.staminaDodgeReducePenalty);

    }

    private void Jump()
    {
        if (!actionGuards.CanJump()) return;

        motor.Jump(stats.jumpTimer);
        stats.Stamina.Reduce(stats.statsSO.staminaJumpReducePenalty);
    }

    public void HandleJumpOrDodge(Vector3 dir)
    {
        if (motor.IsStrafing)
        {
            Dodge(dir);
            return;
        }

        Jump();
    }

    public void Sprint(bool sprintHeld)
    {

        motor.IsSprinting = actionGuards.CanSprint(sprintHeld);

        if (motor.IsSprinting)
        {
            stats.Stamina.Reduce(stats.statsSO.staminaRunReducePenalty);
        }

    }

    public void SetStrafe(bool isStrafing)
    {
        motor.SetStrafe(isStrafing);
    }

    #endregion

    #region Target Lock

    public void SetLockTarget(Transform target)
    {
        motor.rotateTarget = target;
        //locomotion.IsStrafing = true;
    }

    public void ResetLockTarget()
    {
        motor.rotateTarget = null;
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

    public void PerformEmit()
    {
        if(!actionGuards.CanEmit()) return;

        emitController.Emit();
    }

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
