using UnityEngine;

public abstract class CharacterStatsController : MonoBehaviour, IDamagable , ICharacterStatsModifier
{
    protected string uniqueID;

    protected bool isDead;

    #region Damage variables
    protected bool isDamaged;
    protected bool canTakeAnotherDamage = true;
    protected float balancePenalty;
  
    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона
    #endregion

    #region IDamagable Contract
    public abstract void Die();
    public bool IsDead() => isDead;
    public string SourceId() => uniqueID;
    public abstract void TakeDamage(float damage, float balanceDamage, IAttackSource source);
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
    public float BalancePenalty { get => balancePenalty; set => balancePenalty=value; }
    #endregion

    #region ICharacterStatsModifier Contract
    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();
    #endregion

}
