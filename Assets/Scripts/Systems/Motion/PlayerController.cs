using UnityEngine;

public class PlayerController : PlayerMotor
{

    public virtual void ControlLocomotionType()
    {
        if (lockMovement) return;

        SetControllerMoveSpeed(playerStats);

        if (!useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlRotationType()
    {
        if (lockRotation) return;

        bool validInput = input != Vector3.zero || (rotateWithCamera);

        if (validInput)
        {
            // calculate input smooth
            inputSmooth = Vector3.Lerp(inputSmooth, input, (movementSmooth) * Time.deltaTime);

            Vector3 dir = (rotateWithCamera && input == Vector3.zero) && rotateTarget ? rotateTarget.forward : moveDirection;
            RotateToDirection(dir);
        }
    }

    public virtual void UpdateMoveDirection()
    {
        moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
    }

    public virtual void Sprint(bool value)
    {
        bool isMoving = input.sqrMagnitude > 0.1f && !(horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f);
        bool hasStamina = playerStats.currentStamina > 0;

        var sprintConditions = isMoving && isGrounded && !isAttacking && !isDamaged && hasStamina;

        if (value && sprintConditions)
        {
            if (input.sqrMagnitude > 0.1f)
            {
                if (isGrounded && useContinuousSprint)
                {
                    isSprinting = !isSprinting;
                }
                else if (!isSprinting)
                {
                    isSprinting = true;


                }
            }
            else if (!useContinuousSprint && isSprinting)
            {
                isSprinting = false;
            }
        }
        else if (isSprinting)
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            playerStats.ReduceStamina(playerStats.staminaRunReducePenalty);
        }

    }

    public virtual void Jump()
    {
        if (isAttacking || playerStats.currentStamina <= 0)
            return;

        // trigger jump behaviour
        playerStats.ReduceStamina(playerStats.staminaJumpReducePenalty);
        jumpCounter = playerStats.jumpTimer;
        isJumping = true;


        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }


}