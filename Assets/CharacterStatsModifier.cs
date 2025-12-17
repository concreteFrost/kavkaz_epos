using UnityEngine;

public abstract class CharacterStatsModifier : MonoBehaviour, IDamagable , ICharacterStatsModifier
{
    protected string uniqueID;
    protected bool isDead;

    public virtual void Die()
    {
        isDead = true;
        Debug.Log("died");
    }

    public bool IsDead() => isDead;

    public string SourceId() => uniqueID;

    public virtual void TakeDamage(float damage, float balanceDamage)
    {
        //
    }

    protected abstract void ResetBalance();
    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();

}
