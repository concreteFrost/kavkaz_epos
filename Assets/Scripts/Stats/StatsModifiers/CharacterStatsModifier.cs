using UnityEngine;

public abstract class CharacterStatsModifier : MonoBehaviour, IDamagable , ICharacterStatsModifier
{
    protected string uniqueID;

    protected bool isDead;

    #region Damage variables

    protected bool isDamaged;
    protected bool canTakeAnotherDamage = true;
    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона
    #endregion


    #region IDamagable Contract
    public virtual void Die()
    {
        isDead = true; 
    }
    public bool IsDead() => isDead;
    public string SourceId() => uniqueID;
    public abstract void TakeDamage(float damage, float balanceDamage);
    #endregion

    #region ICharacterStatsModifier Contract
    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();
    #endregion

}
