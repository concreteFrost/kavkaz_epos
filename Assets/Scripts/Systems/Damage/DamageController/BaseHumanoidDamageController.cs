using System;
using System.Collections;
using UnityEngine;


public abstract class BaseHumanoidDamageController : MonoBehaviour, IDamagable
{
    protected Transform self;
    protected CharacterStatsController stats;
    protected IHumanoidMovement motor;
    protected Collider damagableCollider;

    [SerializeField] protected Transform aimPosition;
    protected BaseHumanoidAnimatorController animatorController;
    protected CharacterStatsModifier statsModifier;

    [SerializeField] protected Transform defaultTransformParent;

    #region IDamagable Contract
    public Collider DamageCollider() => damagableCollider;
    public IShield Protection { get; set; } = null;
    public bool IsDead { get; set; }
    public bool IsDamaged { get; set; }
    public CharacterType CharacterType { get; set; }
    public Transform GetAimTransform() => aimPosition;
    public Transform GetOrigin() => transform;

    public event Action<IAttackSource> DamageTaken;
    public bool IsKnockedOut { get; set; }
    public bool InBlockingWindow { get; set; }
    public bool CanPlayDamagedAnimation { get; set; }
    public IUiProvider HealthProviderUI { get; set; }
    #endregion

    protected abstract float DamageCooldown();
    protected bool damageBlocked = false;

    protected void BaseInit(BaseHumanoidAnimatorController animatorController, CharacterStatsModifier statsModifier, CharacterStatsController statsController, Transform self, IHumanoidMovement motor)
    {
        this.animatorController = animatorController;
        this.statsModifier = statsModifier;
        this.stats = statsController;
        this.self = self;
        this.motor = motor;
        CanPlayDamagedAnimation = true;

        damagableCollider = GetComponent<Collider>();

        CharacterType = this.stats.statsSO.characterType;

        if (aimPosition == null)
        {
            Debug.Log("no aim position assigned");
        }

    }

    public virtual void PerformKnockout(Vector3 source, float impactForce) { }


    public void ToggleDamagableCollider(bool isActive) => damagableCollider.enabled = GameStateManager.Instance.CurrentState != GameState.Bonfire;

    public void ResetOriginPosition()
    {
        GetOrigin().SetParent(defaultTransformParent);
        GetOrigin().localPosition = Vector3.zero;
        GetOrigin().localRotation = Quaternion.identity;
    }

    public virtual void TakeDamage(DamageData damageData, IAttackSource source)
    {  

        if (damageData.statusEffectData != null)
        {
            statsModifier.GetAndApplyStatusEffect(damageData.statusEffectData);
        }

        stats.Health.ChangeCurrent(damageData.finalDamage, OperationType.Negative);
       
        InvokeDamageTaken(source);


    }

    public void TakeMaxDamage()
    {
        stats.Health.ChangeCurrent(stats.Health.CurrentMax, OperationType.Negative);
        InvokeDamageTaken(null);
    }

    protected void InvokeDamageTaken(IAttackSource source)
    {
        DamageTaken?.Invoke(source);
    }

    protected abstract bool IsDamagingBlocked();

    protected void HandleGetDamaged(BalanceDamageType balanceDamageType)
    {
        if (!CanPlayDamagedAnimation)
        {
           
            return;
        }

        string animClipName = GetDamageAnimation(balanceDamageType);
       

        if (animClipName == null) return;

        animatorController.PlayClipCrossFade(animClipName);
        StartCoroutine(DamagedCoroutine(animClipName));
    }

    protected IEnumerator DamageCooldownCoroutine()
    {
        damageBlocked = true;
        yield return new WaitForSeconds(DamageCooldown());
        damageBlocked = false;

    }

    protected IEnumerator DamagedCoroutine(string animationName)
    {


        animatorController.Animator().applyRootMotion = true;
        IsDamaged = true;
        yield return AnimatorUtils.WaitForAnimationEnd(animatorController.Animator(), animationName, AnimatorParameters.damageLayer);
        animatorController.Animator().applyRootMotion = false;
        IsDamaged = false;
    }

    private string GetDamageAnimation(BalanceDamageType balanceDamage)
    {
        switch (balanceDamage)
        {
            case BalanceDamageType.None: return null;
            case BalanceDamageType.Low: return AnimatorParameters.lowDamageClip;
            case BalanceDamageType.High: return AnimatorParameters.midDamageClip;
            case BalanceDamageType.Extreme: return AnimatorParameters.highDamageClip;
            case BalanceDamageType.Blocked: return AnimatorParameters.shieldDamageClip;
            default: return null;
        }
    }


}