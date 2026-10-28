using UnityEngine;

public class PlayerLocomotionActionHandler : MonoBehaviour
{
    PlayerMotor motor;
    PlayerActionGuards actionGuards;
    CharacterStatsController stats;
    CharacterConsumeController consumeController;
    CharacterAudioManager audioManager;
    IInteractor interact;
    IClimber climbing;


    private void Update()
    {
        TryClimb();
    }



    private void FixedUpdate()
    {
        UpdateAnimator();
    }

    public void Init(
        PlayerMotor motor,
        IInteractor interaction,
        IClimber climbing,
        PlayerActionGuards actionGuards,
        CharacterStatsController stats,
        CharacterConsumeController consumeController,
        CharacterAudioManager audioManager
        )
    {

        this.motor = motor;
        this.interact = interaction;
        this.climbing = climbing;
        this.stats = stats;
        this.actionGuards = actionGuards;
        this.consumeController = consumeController;
        this.audioManager = audioManager;

    }

    public void MoveAndRotate(Vector3 dir)
    {
        UpdateMotor();
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

        Rigidbody body = motor._rigidbody;
        float speed = motor.BlockRotation ? motor.blockedRotationSpeed : motor.rotationSpeed;
        Quaternion rotation;

        // Поворот применяется в том же шаге физики, что и движение.
        if (motor.rotateTarget != null)
        {
            Vector3 direction = motor.rotateTarget.position - body.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= Mathf.Epsilon) return;

            rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(direction),
                speed * Time.deltaTime);
        }
        else
        {
            Vector3 forward = Vector3.RotateTowards(body.rotation * Vector3.forward,
                input.normalized, speed * Time.deltaTime, 0f);
            rotation = Quaternion.LookRotation(forward);
        }

        body.MoveRotation(rotation);
    }

    private void Dodge(Vector3 dir)
    {
        if (!actionGuards.CanDodge()) return;

        motor.Dodge(dir);
        stats.Stamina.ChangeCurrent(stats.statsSO.staminaDodgeReducePenalty, OperationType.Negative);

    }

    private void Jump()
    {
        if (!actionGuards.CanJump()) return;

        audioManager.PlayJump();
        motor.Jump(stats.jumpTimer);
        stats.Stamina.ChangeCurrent(stats.statsSO.staminaJumpReducePenalty, OperationType.Negative);
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

            stats.Stamina.ChangeCurrent(stats.statsSO.staminaRunReducePenalty, OperationType.Negative);
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
