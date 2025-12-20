using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    PlayerMotor motor;
    PlayerCombatController combatController;
    PlayerStatsModifier playerStatsModifier;  
    PlayerTargetLock targetLock;
    public void Init(PlayerAnimatorServiceProvider provider)
    {
        this.animator = provider.animator;
        this.motor = provider.motor;
        this.combatController = provider.combatController;
        this.playerStatsModifier = provider.statsModifier; 
        this.targetLock = provider.targetLock;
       
    }
    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        
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

        animator.SetFloat(AnimatorParameters.DodgeX, motor.DodgeX);
        animator.SetFloat(AnimatorParameters.DodgeY, motor.DodgeY);

        // combat
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsAttacking, combatController.IsAttacking);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
        animator.SetInteger(AnimatorParameters.AttackIndex, combatController.AttackIndex);
        animator.SetInteger(AnimatorParameters.WeaponType, combatController.WeaponIndex);


        // damage
        animator.SetBool(AnimatorParameters.IsDamaged, playerStatsModifier.IsDamaged);
        animator.SetFloat(AnimatorParameters.BalancePenalty, playerStatsModifier.BalancePenalty);
        animator.SetBool(AnimatorParameters.IsDead, playerStatsModifier.IsDead());

        //target lock
        animator.SetBool(AnimatorParameters.IsStrafing, targetLock.IsLockedOnTarget);

    }


}
