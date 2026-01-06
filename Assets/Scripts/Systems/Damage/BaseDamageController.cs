using UnityEngine;


public abstract class BaseDamageController : MonoBehaviour, IDamagable
{

    protected ICharacterStatsController statsController;
    protected CharacterStats stats;

    public CharacterType characterType;

    [SerializeField] protected Transform aimPosition;

    protected string uniqueID;

    protected bool isDead;

    #region Damage variables
    protected bool isDamaged;
    //protected bool canTakeAnotherDamage = true;
    protected float balancePenalty;

    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона
    #endregion

    #region IDamagable Contract
    public bool IsDead() => isDead;
    public string SourceId() => uniqueID;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
    public float BalancePenalty { get => balancePenalty; set => balancePenalty = value; }

    public CharacterType CharacterType { get=>characterType; set => characterType = value; }

    public Transform GetAimTransform()=> aimPosition;
    public Transform GetOrigin() => transform;
    #endregion

   
    public virtual void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
        if (isDead) return;

        balancePenalty = balanceDamage;

        isDamaged = true;

        //if (!combatController.IsShieldRaised)
        //{
        //    isDamaged = true;
        //}

        statsController.ReduceHealth(damage);
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
