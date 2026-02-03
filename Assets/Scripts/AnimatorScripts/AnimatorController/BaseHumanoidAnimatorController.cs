
using UnityEngine;

public abstract class BaseHumanoidAnimatorController
{
    protected Animator animator;
    protected AnimatorOverrideController overrideController;

    protected IHumanoidMovement movement;
    protected ITargetLocker targetLocker;
    protected IDamagable damagable;
    protected IHumanoidCombat attackSource;
    public abstract void UpdateAnimatorParameters();

    public virtual void Init(HumanoidAnimatorService service)
    {

        this.animator = service.animator;
        this.overrideController = service.overrideController;

        //overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        this.movement = service.motor;
        this.targetLocker = service.targetLock;
        this.damagable = service.damageController;
        this.attackSource = service.combatController;
    }

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

        animator.SetBool(AnimatorParameters.IsStrafing, locomotion.IsStrafing);

    }

    protected void UpdateCombatState(IHumanoidCombat combatController)
    {
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
    }

    protected void UpdateDamageState(IDamagable damageController)
    {
        animator.SetBool(AnimatorParameters.IsDamaged, damageController.IsDamaged);
        animator.SetInteger(
    AnimatorParameters.BalancePenalty,
    (int)damageController.BalancePenalty
);

        animator.SetBool(AnimatorParameters.IsDead, damageController.IsDead);
    }

    public void OverrideAttack(WeaponAttack attack, string name)
    {
        var stateName = name;
        overrideController[stateName] = attack.animationInfo.clip;

        animator.speed = attack.animationInfo.animationSpeed;

        animator.CrossFade(stateName, AnimatorParameters.transitionSpeed, AnimatorParameters.combatLayer);
    }

    public void OverrideArmed(IWeapon w)
    {
        if (w.WeaponData().idleAnimation == null) return;

        overrideController["Armed"] = w.WeaponData().idleAnimation;
        animator.CrossFade("Armed", AnimatorParameters.transitionSpeed, AnimatorParameters.armedLayer);
    }

    public void PerformThrow()
    {
        animator.CrossFade("Throw weapon", AnimatorParameters.transitionSpeed, AnimatorParameters.combatLayer);
    }

    public void PerformInteract()
    {

        animator.CrossFade("Interact", AnimatorParameters.transitionSpeed);
    }


    #region Push Control
    public void PerformPush()
    {
        animator.CrossFade("Agressive Push", AnimatorParameters.transitionSpeed);
    }

    public void GetPushed(PushDirection dir)
    {
        if(dir == PushDirection.Forward)
        {
            animator.CrossFade("Get Pushed From Front", AnimatorParameters.transitionSpeed);
        }
        else
        {
            animator.CrossFade("Get Pushed From Back", AnimatorParameters.transitionSpeed);
        }

    }

    #endregion


    //protected void UpdateTargetLockState(ITargetLocker targetLock)
    //{
    //    animator.SetBool(AnimatorParameters.IsStrafing, targetLock.IsLockedOnTarget);
    //}
}


