using System;
using System.Collections;
using UnityEngine;


public abstract class BaseHumanoidDamageController : MonoBehaviour, IDamagable
{
    protected Transform self;
    protected HumanoidStatsManager statsManager;
    protected IHumanoidMovement motor;

    [SerializeField] protected Transform aimPosition;
    protected BaseHumanoidAnimatorController animatorController;

    #region IDamagable Contract
    public bool IsDead { get; set; }
    public bool IsDamaged { get; set; }
    public CharacterType CharacterType { get; set; }
    public Transform GetAimTransform()=> aimPosition;
    public Transform GetOrigin() => transform;

    public event Action<Transform> DamageTaken;
    public bool IsKnockedOut {  get; set; } 
    #endregion


    public virtual void TakeDamage(DamageData damageData, Transform source)
    {
        if (IsDamagingBlocked()) return;

        statsManager.Controller.ReduceHealth(damageData.healthDamageMultiplier);
        InvokeDamageTaken(source);


    }

    protected void InvokeDamageTaken(Transform source)
    {
        DamageTaken?.Invoke(source);
    }

    protected abstract bool IsDamagingBlocked();
    //public abstract void Die();
    //public abstract void Respawn();

    protected void HandleGetDamaged(BalanceDamageType balanceDamageType)
    {
        string animClipName = GetDamageAnimation(balanceDamageType);

        if (animClipName == null) return;

        animatorController.PlayClipCrossFade(animClipName);
        StartCoroutine(DamagedCoroutine(animClipName));
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
