using System;
using UnityEngine;


public abstract class BaseDamageController : MonoBehaviour, IDamagable
{

    protected ICharacterStatsController statsController;
    protected HumanoidStats stats;

    public CharacterType characterType;

    [SerializeField] protected Transform aimPosition;

    #region IDamagable Contract
    public bool IsDead { get; set; }

    public bool IsDamaged { get; set; }
    public BalanceDamageType BalancePenalty { get ; set ; }

    public CharacterType CharacterType { get=>characterType; set => characterType = value; }

    public Transform GetAimTransform()=> aimPosition;
    public Transform GetOrigin() => transform;

    public event Action<Transform> DamageTaken;

    public bool IsKnockedOut {  get; set; } 
    #endregion


    public virtual void TakeDamage(DamageData damageData, Transform source)
    {
        if(IsDamagingBlocked() || IsDead) return;

        BalancePenalty = damageData.balanceDamageType;
        IsDamaged = true;

        statsController.ReduceHealth(damageData.healthDamageMultiplier);
        InvokeDamageTaken(source);


    }

    protected void InvokeDamageTaken(Transform source)
    {
        DamageTaken?.Invoke(source);
    }

    protected abstract bool IsDamagingBlocked();

    public abstract void Die();

   
}
