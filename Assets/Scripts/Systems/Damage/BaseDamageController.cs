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


    public virtual void TakeDamage(float damage, BalanceDamageType balanceDamage, Transform source)
    {
        if (isDead) return;

        BalancePenalty = balanceDamage;

        isDamaged = true;


        //if (!combatController.IsShieldRaised)
        //{
        //    isDamaged = true;
        //}


        
        statsController.ReduceHealth(damage);
        DamageTaken?.Invoke(source);
        //StartCoroutine(DamageCooldownCoroutine(maxDamageCooldown));

    }

    public virtual void Die()
    {

        isDead = true;

        //input.controls.Player.Disable();

        //inventory.CurrentWeapon?.DropWeapon();
        //inventory.ShieldWeapon?.ThrowShield();
        //inventory.ResetWeapon();

        //StartCoroutine(RespawnCoroutine(5f));
    }
}
