using UnityEngine;

public class PlayerAnimator : BaseHumanoidAnimator
{
    PlayerMotor locomotion;
    HumanoidCombatController combatController;
    PlayerStatsModifier playerStatsModifier;
    PlayerTargetLock targetLock;
    PlayerClimbing climbing;
    public void Init(PlayerAnimatorServiceProvider provider)
    {
        this.animator = provider.animator;
        this.locomotion = provider.motor;
        this.combatController = provider.combatController;
        this.playerStatsModifier = provider.statsModifier;
        this.targetLock = provider.targetLock;
        this.climbing = provider.climbing;  

    }
    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }
       
        //locomotion
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

        // combat
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsAttacking, combatController.IsAttacking);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
        animator.SetBool(AnimatorParameters.IsThrowingWeapon, combatController.IsThrowingWeapon);

        // damage
        animator.SetBool(AnimatorParameters.IsDamaged, playerStatsModifier.IsDamaged);
        animator.SetFloat(AnimatorParameters.BalancePenalty, playerStatsModifier.BalancePenalty);
        animator.SetBool(AnimatorParameters.IsDead, playerStatsModifier.IsDead());

        //target lock
        animator.SetBool(AnimatorParameters.IsStrafing, targetLock.IsLockedOnTarget);

        //climbing
        animator.SetBool(AnimatorParameters.IsClimbing, climbing.IsClimbing);


    }


}