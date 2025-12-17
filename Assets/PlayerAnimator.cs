using System;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    public override void SetAnimatorMoveSpeed(ICharacterAnimator IAnim)
    {
        if (animator == null || !animator.enabled) return;

        animator.SetBool(AnimatorParameters.IsStrafing, IAnim.IsLockedOnTarget);
        animator.SetBool(AnimatorParameters.IsDodging, IAnim.IsDodging);
        animator.SetBool(AnimatorParameters.IsSprinting, IAnim.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, IAnim.IsGrounded);
        animator.SetFloat(AnimatorParameters.GroundDistance, IAnim.GroundDistance);
        animator.SetFloat(AnimatorParameters.InputHorizontal, IAnim.StopMove ? 0 : IAnim.HorizontalSpeed, IAnim.AnimationSmooth, Time.deltaTime);
        animator.SetFloat(AnimatorParameters.InputVertical, IAnim.StopMove ? 0 : IAnim.VerticalSpeed, IAnim.AnimationSmooth, Time.deltaTime);
        animator.SetFloat(AnimatorParameters.InputMagnitude, IAnim.StopMove ? 0f : IAnim.InputMagnitude, IAnim.AnimationSmooth, Time.deltaTime);

        //animator.SetInteger(AnimatorParameters.AttackIndex)

        //combat control
        animator.SetBool(AnimatorParameters.IsWeaponed, IAnim.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsAttacking, IAnim.IsAttacking);
        animator.SetBool(AnimatorParameters.IsShieldRaised, IAnim.IsShieldRaised);
        animator.SetInteger(AnimatorParameters.AttackIndex, IAnim.AttackIndex);
        animator.SetInteger(AnimatorParameters.WeaponType, IAnim.WeaponIndex);

        //damage control
        animator.SetBool(AnimatorParameters.IsDamaged, IAnim.IsDamaged);
        animator.SetFloat(AnimatorParameters.BalancePenalty, IAnim.BalancePenalty);
        animator.SetBool(AnimatorParameters.IsDead, IAnim.IsDead);
    }

    public override void UpdateAnimator(ICharacterAnimator IAnim)
    {
        Vector3 relativeInput = transform.InverseTransformDirection(IAnim.MoveDirection);
        IAnim.VerticalSpeed = relativeInput.z;
        IAnim.HorizontalSpeed = relativeInput.x;

        var newInput = new Vector2(IAnim.VerticalSpeed, IAnim.HorizontalSpeed);

        IAnim.InputMagnitude = Mathf.Clamp(newInput.magnitude, 0, IAnim.IsSprinting ? AnimatorConsts.runningSpeed : AnimatorConsts.walkSpeed);
    }
}
