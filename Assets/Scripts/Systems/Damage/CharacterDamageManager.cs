using UnityEngine;

public abstract class CharacterDamageManager : MonoBehaviour, IDamagable
{
    [SerializeField] protected string damagableId;
    [SerializeField] protected float currentHealth;
    
    [SerializeField] protected float currentBalance;

    public string SourceId() => damagableId;

    public float Health() => currentHealth;

    public abstract void TakeDamage(float d, float b);

    public void Die()
    {
        Debug.Log("died");
    }

    protected void ResetBalance()
    {
        currentBalance = 0; 
    }


}
