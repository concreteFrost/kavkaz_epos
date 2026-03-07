using UnityEngine;

public class PlayerLocomotionActionHandler : MonoBehaviour
{
    PlayerMotor motor;
    PlayerActionGuards actionGuards;
    CharacterStatsController stats;
    CharacterConsumeController consumeController;
    ICollector interact;
    IClimber climbing;


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
        ICollector interaction,
        IClimber climbing,
        PlayerActionGuards actionGuards,
        CharacterStatsController stats,
        CharacterConsumeController consumeController
        )
    {

        this.motor = motor;
        this.interact = interaction;
        this.climbing = climbing;
        this.stats = stats;
        this.actionGuards = actionGuards;
        this.consumeController = consumeController; 

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
        stats.Stamina.ReduceCurrent(stats.statsSO.staminaDodgeReducePenalty);

    }

    private void Jump()
    {
        if (!actionGuards.CanJump()) return;

        motor.Jump(stats.jumpTimer);
        stats.Stamina.ReduceCurrent(stats.statsSO.staminaJumpReducePenalty);
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

            stats.Stamina.ReduceCurrent(stats.statsSO.staminaRunReducePenalty);
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


    #region Interaction
    public void Interact()
    {
        //не взаимодействуем если игрок атакует
        if (!actionGuards.CanInteract()) return;

        interact.StartInteracion();
    }

    public void Consume()
    {
        if(!actionGuards.CanConsume()) return;

        consumeController.StartConsume();
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