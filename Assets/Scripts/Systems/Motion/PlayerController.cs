using Unity.VisualScripting;
using UnityEditor.VersionControl;
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
        Vector3 dir = (rotateWithCamera && input == Vector3.zero) && rotateTarget ? rotateTarget.forward : moveDirection;
        RotateToDirection(dir);
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

    public virtual void Dodge(Vector2 dir)
    {
        if (isAttacking || isDodging)
            return;

        isDodging = true;

        float dodgeX = 0f;
        float dodgeY = 0f;

        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);

        if (relativeInput.sqrMagnitude < 0.01f)
        {
            // без движения — всегда назад
            dodgeY = -1f;
        }
        else if (Mathf.Abs(relativeInput.x) > Mathf.Abs(relativeInput.z)) //
        {
            dodgeX = Mathf.Sign(relativeInput.x);
        }
        else
        {
            dodgeY = Mathf.Sign(relativeInput.z); 
        }

        playerStats.ReduceStamina(playerStats.staminaJumpReducePenalty);

        animator.SetFloat("DodgeX", dodgeX);
        animator.SetFloat("DodgeY", dodgeY);
    }



}