using UnityEngine;

public class PlayerController : PlayerMotor
{
    public void SetLockTarget(Transform target)
    {
        rotateTarget = target;
        IsLockedOnTarget = true;
    }

    public void ResetLockTarget()
    {
        rotateTarget = null;
        IsLockedOnTarget = false;
    }

    public virtual void Sprint(bool inputSprint)
    {
        bool isMoving = input.sqrMagnitude > 0.1f;
        bool hasStamina = playerStats.currentStamina > 0;

        // локальное направление движения
        Vector3 localDir = transform.InverseTransformDirection(moveDirection);
        bool isMovingForward = localDir.z > 0.1f; // спринт только вперёд

        bool canSprint =
            inputSprint &&
            isMoving &&
            isMovingForward &&
            hasStamina &&
            IsGrounded &&
            !IsAttacking &&
            !IsDamaged;

        IsSprinting = canSprint;

        if (IsSprinting)
            playerStats.ReduceStamina(playerStats.staminaRunReducePenalty);
    }


    public void HandleJumpOrDodge(Vector2 dir)
    {
        if (IsLockedOnTarget)
        {
            Dodge(dir);
            return;
        }

        Jump();
    }

    private void Jump()
    {
        bool canJump =  IsGrounded &&
               GroundAngle() < slopeLimit &&
               !IsJumping &&
               !IsAttacking &&
               !IsDamaged &&
               !StopMove &&
               playerStats.currentStamina > 0;

        if (!canJump) return;

        playerStats.ReduceStamina(playerStats.staminaJumpReducePenalty);
        jumpCounter = playerStats.jumpTimer;
        IsJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

    private void Dodge(Vector2 dir)
    {
        if (IsAttacking || IsDodging)
            return;

        IsDodging = true;

        float dodgeX = 0f;
        float dodgeY = 0f;

        Vector3 relativeInput = GetInverseTransformDirection();

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