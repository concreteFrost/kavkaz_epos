using UnityEngine;

public class PlayerController : PlayerMotor
{
    public void SetLockTarget(Transform target)
    {
        rotateTarget = target;
        isLockedOnTarget = true;
    }

    public void ResetLockTarget()
    {
        rotateTarget = null;
        isLockedOnTarget = false;
    }

    public virtual void Sprint(bool inputSprint)
    {
        isSprinting = CanSprint();
        if (isSprinting)
            playerStatsModifer.ReduceStamina(stats.staminaRunReducePenalty);
    }

    public void Jump()
    {  
        if (!CanJump()) return;

        playerStatsModifer.ReduceStamina(stats.staminaJumpReducePenalty);
        jumpCounter = stats.jumpTimer;
        isJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

    //private void Dodge(Vector2 dir)
    //{
    //    if (!CanDodge())
    //        return;

    //    combatController.isDodging = true;

    //    float dodgeX = 0f;
    //    float dodgeY = 0f;

    //    Vector3 relativeInput = GetInverseTransformDirection();

    //    if (relativeInput.sqrMagnitude < 0.01f)
    //    {
    //        // без движения — всегда назад
    //        dodgeY = -1f;
    //    }
    //    else if (Mathf.Abs(relativeInput.x) > Mathf.Abs(relativeInput.z)) //
    //    {
    //        dodgeX = Mathf.Sign(relativeInput.x);
    //    }
    //    else
    //    {
    //        dodgeY = Mathf.Sign(relativeInput.z); 
    //    }

    //    playerStatsModifer.ReduceStamina(playerStats.staminaJumpReducePenalty);

    //    animator.SetFloat("DodgeX", dodgeX);
    //    animator.SetFloat("DodgeY", dodgeY);
    //}


   

}