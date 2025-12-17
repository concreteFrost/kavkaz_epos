using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    PlayerMotor motor;
    public void Init(Animator animator, PlayerMotor motor)
    {
        this.animator = animator;
        this.motor = motor;
       
    }
    public override void SetAnimatorMoveSpeed()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        animator.SetBool(AnimatorParameters.IsStrafing, motor.IsLockedOnTarget);
        animator.SetBool(AnimatorParameters.IsDodging, motor.IsDodging);
        animator.SetBool(AnimatorParameters.IsSprinting, motor.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, motor.IsGrounded);

        animator.SetFloat(AnimatorParameters.GroundDistance, motor.GroundDistance);

        animator.SetFloat(
            AnimatorParameters.InputHorizontal,
            motor.StopMove ? 0 : motor.HorizontalSpeed,
            motor.AnimationSmooth,
            Time.deltaTime
        );

        animator.SetFloat(
            AnimatorParameters.InputVertical,
            motor.StopMove ? 0 : motor.VerticalSpeed,
            motor.AnimationSmooth,
            Time.deltaTime
        );

        animator.SetFloat(
            AnimatorParameters.InputMagnitude,
            motor.StopMove ? 0 : motor.InputMagnitude,
            motor.AnimationSmooth,
            Time.deltaTime
        );

        // combat
        animator.SetBool(AnimatorParameters.IsWeaponed, motor.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsAttacking, motor.IsAttacking);
        animator.SetBool(AnimatorParameters.IsShieldRaised, motor.IsShieldRaised);
        animator.SetInteger(AnimatorParameters.AttackIndex, motor.AttackIndex);
        animator.SetInteger(AnimatorParameters.WeaponType, motor.WeaponIndex);

        // damage
        animator.SetBool(AnimatorParameters.IsDamaged, motor.IsDamaged);
        animator.SetFloat(AnimatorParameters.BalancePenalty, motor.BalancePenalty);
        animator.SetBool(AnimatorParameters.IsDead, motor.IsDead);
    }

}
