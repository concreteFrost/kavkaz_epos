using System;
using UnityEngine;


public abstract class BaseDamageController : MonoBehaviour, IDamagable
{

    protected ICharacterStatsController statsController;
    protected HumanoidStats stats;

    public CharacterType characterType;

    [SerializeField] protected Transform aimPosition;

    protected string uniqueID;

    protected bool isDead;

    #region Damage variables
    protected bool isDamaged;
    //protected bool canTakeAnotherDamage = true;


    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона
    #endregion

    #region IDamagable Contract
    public bool IsDead() => isDead;
    public string SourceId() => uniqueID;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
    public BalanceDamageType BalancePenalty { get ; set ; }

    public CharacterType CharacterType { get=>characterType; set => characterType = value; }

    public Transform GetAimTransform()=> aimPosition;
    public Transform GetOrigin() => transform;

    public event Action<Transform> DamageTaken;
    #endregion


    public virtual void TakeDamage(DamageData damageData, Transform source )
    {

        BalancePenalty = damageData.balanceDamageType;

        isDamaged = true;

        statsController.ReduceHealth(damageData.healthDamageMultiplier);
        DamageTaken?.Invoke(source);
       

    }

    public abstract void Die();
   
}
