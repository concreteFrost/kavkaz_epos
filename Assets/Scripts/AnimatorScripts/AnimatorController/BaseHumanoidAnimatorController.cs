using UnityEngine;
using Zenject;

public abstract class BaseHumanoidAnimatorController
{
    #region Fields

    protected Animator animator;
    protected AnimatorOverrideController overrideController;
    protected CharacterAudioManager audioManager;

    protected IHumanoidMovement movement;
    protected ITargetLocker targetLocker;
    protected IDamagable damagable;
    protected IHumanoidMeleeCombat attackSource;
    protected IPushable pushReceiver;


    float lastFootstep;
    float footstep;

    #endregion

    #region Abstract

    public abstract void UpdateAnimatorParameters();

    #endregion

    #region Public API

    public Animator Animator() => animator;

    public virtual void Init(
        Animator animator,
        AnimatorOverrideController overrideController,
        CharacterAudioManager audioManager, 
        IHumanoidMovement motor,
        IHumanoidMeleeCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver
    )
    {
        this.animator = animator;
        this.overrideController = overrideController;
        this.audioManager = audioManager;   

        //overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.updateMode = AnimatorUpdateMode.Fixed;
        animator.runtimeAnimatorController = overrideController;

        this.movement = motor;
        this.targetLocker = targetLock;
        this.damagable = damageController;
        this.attackSource = combatController;
        this.pushReceiver = pushReceiver;

       
    }

    #endregion

    #region State Updates

    protected void UpdateLocomotionState(IHumanoidMovement locomotion)
    {
        animator.SetBool(AnimatorParameters.IsDodging, locomotion.IsDodging);
        animator.SetBool(AnimatorParameters.IsSprinting, locomotion.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, locomotion.IsGrounded);
        animator.SetBool(AnimatorParameters.IsTurning, locomotion.IsTurning);

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

        this.footstep = animator.GetFloat("Footstep");

        if (Mathf.Abs(footstep) < .00001f) footstep = 0;

        if (lastFootstep < 0.7f && footstep >= 0.7f)
        {
            audioManager.PlayWalk();
        }


        //Debug.Log(footstep);

        lastFootstep = footstep;
    }

    protected void UpdateCombatState(IHumanoidMeleeCombat combatController)
    {
        animator.SetBool(AnimatorParameters.IsWeaponed, combatController.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsShieldRaised, combatController.IsShieldRaised);
    }

    protected void UpdateDamageState(IDamagable damageController)
    {
        animator.SetBool(AnimatorParameters.IsDamaged, damageController.IsDamaged);
        animator.SetBool(AnimatorParameters.IsDead, damageController.IsDead);
        animator.SetBool(AnimatorParameters.IsPushed, pushReceiver.IsPushed);
    }

    #endregion

    #region Playback

    public void PlayClipCrossFade(string name)
    {
        animator.CrossFade(name, AnimatorParameters.transitionSpeed);
    }

    public void PlayClipImmidiate(string name)
    {
        animator.Play(name);
    }

    public void PlayTalk()
    {
        var randomReaction = Random.Range(0, 3);
        animator.CrossFade(
            $"Reply Dialogue_{randomReaction}",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.motionLayer
        );
    }

    public void PlayThankYou()
    {
        var randomReaction = Random.Range(0, 1);
        animator.CrossFade(
            $"Reply Thank You_{randomReaction}",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.motionLayer
        );
    }

    public void ResetAnimator()
    {
        //animator.CrossFade("Free Locomotion", AnimatorParameters.transitionSpeed, AnimatorParameters.motionLayer);

        animator.CrossFade(
            "Free Locomotion",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.motionLayer
        );
    }

    #endregion

    #region Overrides

    public void OverrideAttack(WeaponAttack attack, string name)
    {
        var stateName = name;

        overrideController[stateName] = attack.animationInfo.clip;
        animator.speed = attack.animationInfo.animationSpeed;

        animator.CrossFade(
            stateName,
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.combatLayer
        );
    }

    public void OverrideSpell(SpellProjectileSO spell)
    {
        overrideController["Spell_Cast"] = spell.castAnimation.clip;
        animator.speed = spell.castAnimation.animationSpeed;

        animator.CrossFade(
            "Spell_Cast",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.combatLayer
        );
    }

    public void OverrideConsume(AnimationInfoSO animation)
    {
        overrideController["Consume"] = animation.clip;
        animator.speed = animation.animationSpeed;

        animator.CrossFade(
            "Consume",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.interractionLayer
        );
    }

    public void OverrideArmed(IWeapon w)
    {
        if (w.WeaponData().idleAnimation == null)
        {
            overrideController["Armed"] = null;
            return;
        }

        overrideController["Armed"] = w.WeaponData().idleAnimation;

        animator.CrossFade(
            "Armed",
            AnimatorParameters.transitionSpeed,
            AnimatorParameters.armedLayer
        );
    }

    #endregion

    #region Push Control

    public void PerformPush()
    {
        animator.CrossFade("Agressive Push", AnimatorParameters.transitionSpeed);
    }

    public void GetPushed(PushDirection dir)
    {
        if (dir == PushDirection.Forward)
        {
            animator.CrossFade("Get Pushed From Front", AnimatorParameters.transitionSpeed);
        }
        else
        {
            animator.CrossFade("Get Pushed From Back", AnimatorParameters.transitionSpeed);
        }
    }

    #endregion
}