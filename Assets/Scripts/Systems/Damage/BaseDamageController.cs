using System;
using UnityEngine;


public abstract class BaseDamageController : MonoBehaviour, IDamagable
{

    protected ICharacterStatsController statsController;
    protected HumanoidStats stats;

    public CharacterType characterType;

    [SerializeField] protected Transform aimPosition;

    protected string uniqueID;

    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона

    #region IDamagable Contract
    public bool IsDead { get; set; }
    public string SourceId() => uniqueID;

    public bool IsKnockedOut { get; set; }      
    public bool IsDamaged { get; set; }
    public BalanceDamageType BalancePenalty { get ; set ; }

    public CharacterType CharacterType { get=>characterType; set => characterType = value; }

    public Transform GetAimTransform()=> aimPosition;
    public Transform GetOrigin() => transform;

    public event Action<Transform> DamageTaken;
    #endregion


    public virtual void TakeDamage(DamageData damageData, Transform source )
    {

        BalancePenalty = damageData.balanceDamageType;

        IsDamaged = true;

        statsController.ReduceHealth(damageData.healthDamageMultiplier);
        DamageTaken?.Invoke(source);
       

    }

    public abstract void Die();
   
}
