
using UnityEngine;

public abstract class BaseHumanoidAnimator 
{
    protected Animator animator;
    public abstract void UpdateAnimatorParameters();

    protected void UpdateLocomotionState(IHumanoidMovement locomotion)
    {
        animator.SetBool(AnimatorParameters.IsDodging, locomotion.IsDodging);
        animator.SetBool(AnimatorParameters.IsSprinting, locomotion.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, locomotion.IsGrounded);
        animator.SetFloat(AnimatorParameters.GroundDistance, locomotion.GroundDistance);

        animator.SetFloat(
            AnimatorParameters.InputHorizontal,
            locomotion.HorizontalSpeed,
            locomotion.AnimationSmooth,
            Time.deltaTime
        );

        animator.SetFloat(
            AnimatorParameters.InputVertical,
            locomotion.VerticalSpeed,
            locomotion.AnimationSmooth,
            Time.deltaTime
        );

        animator.SetFloat(
            AnimatorParameters.InputMagnitude,
            locomotion.InputMagnitude,
            locomotion.AnimationSmooth,
            Time.deltaTime
        );

        animator.SetFloat(AnimatorParameters.DodgeX, locomotion.DodgeX);
        animator.SetFloat(AnimatorParameters.DodgeY, locomotion.DodgeY);

    }

    protected void UpdateCombatState(IHumanoidCombat combatController)
    {
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
    }

    protected void UpdateDamageState(IDamagable damageController)
    {
        animator.SetBool(AnimatorParameters.IsDamaged, damageController.IsDamaged);
        animator.SetFloat(AnimatorParameters.BalancePenalty, damageController.BalancePenalty);
        animator.SetBool(AnimatorParameters.IsDead, damageController.IsDead());
    }

    protected void UpdateTargetLockState(ITargetLocker targetLock)
    {
        animator.SetBool(AnimatorParameters.IsStrafing, targetLock.IsLockedOnTarget);
    }
}


