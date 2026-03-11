
using UnityEngine;
using Zenject;

public abstract class BaseHumanoidAnimatorController
{
    
    protected Animator animator;
    protected AnimatorOverrideController overrideController;

    protected IHumanoidMovement movement;
    protected ITargetLocker targetLocker;
    protected IDamagable damagable;
    protected IHumanoidMeleeCombat attackSource;
    protected IPushable pushReceiver;
    public abstract void UpdateAnimatorParameters();

    public Animator Animator() => animator;

    public virtual void Init(
         Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidMeleeCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver
        )
    {
        
        this.animator =animator;
        this.overrideController = overrideController;

        //overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.updateMode = AnimatorUpdateMode.Fixed;
        animator.runtimeAnimatorController = overrideController;

        this.movement = motor;
        this.targetLocker = targetLock;
        this.damagable = damageController;
        this.attackSource = combatController;
        this.pushReceiver = pushReceiver;
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

    protected void UpdateCombatState(IHumanoidMeleeCombat combatController)
    {
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
    }

    protected void UpdateDamageState(IDamagable damageController)
    {
        animator.SetBool(AnimatorParameters.IsDamaged, damageController.IsDamaged);
        //        animator.SetInteger(
        //    AnimatorParameters.BalancePenalty,
        //    (int)damageController.BalancePenalty
        //);

        animator.SetBool(AnimatorParameters.IsDead, damageController.IsDead);
        animator.SetBool(AnimatorParameters.IsPushed, pushReceiver.IsPushed);   

    }

    public void PlayClipCrossFade(string name)
    {
        animator.CrossFade(name, AnimatorParameters.transitionSpeed);
    }

    public void PlayClipImmidiate(string name)
    {
        animator.Play(name);
    }

    public void OverrideAttack(WeaponAttack attack, string name)
    {
        var stateName = name;
        overrideController[stateName] = attack.animationInfo.clip;

        animator.speed = attack.animationInfo.animationSpeed;

        animator.CrossFade(stateName, AnimatorParameters.transitionSpeed, AnimatorParameters.combatLayer);
    }

    public void OverrideSpell(SpellProjectileSO spell)
    {
        overrideController["Spell_Cast"] = spell.animation.clip;

        animator.speed = spell.animation.animationSpeed;
        animator.CrossFade("Spell_Cast", AnimatorParameters.transitionSpeed, AnimatorParameters.combatLayer);
    }

    public void OverrideConsume(AnimationInfoSO animation)
    {
        overrideController["Consume"] =animation.clip;
        animator.speed = animation.animationSpeed;
        animator.CrossFade("Consume", AnimatorParameters.transitionSpeed, AnimatorParameters.interractionLayer);
      
    }

    public void OverrideArmed(IWeapon w)
    {
        if (w.WeaponData().idleAnimation == null) return;

        overrideController["Armed"] = w.WeaponData().idleAnimation;
        animator.CrossFade("Armed", AnimatorParameters.transitionSpeed, AnimatorParameters.armedLayer);
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


