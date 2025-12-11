using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamagable , ICharacterStats
{

    public BaseCharacterStatsSO statsSO;

    protected bool isDead;

    [Header("movement speed")]
    public float walkSpeed;
    public float runningSpeed;

    [Header("jumping")]
    public float jumpHeight;
    public float jumpTimer;

    [Header("health")]
    public float currentHealth;
    public float maxHealth;

    [Header("stamina")]
    public float currentStamina;
    public float maxStamina;
    public float staminaRunReducePenalty = 0.03f;
    public float staminaJumpReducePenalty = 2f;
    public float staminaRegenDelay = 5f;
    public float staminaRegenRate = 0.1f;
    protected float staminaRegenTimer = 0.0f; 


    [SerializeField] protected string damagableId;
    [SerializeField] protected float currentBalance;

    public string SourceId() => damagableId;

    public float Health() => currentHealth;

    public bool IsDead() => isDead; 

    public abstract void TakeDamage(float d, float b);

    public virtual void Die()
    {
        isDead = true;
        Debug.Log("died");
    }

    protected void ResetBalance()
    {
        currentBalance = 0;
    }

    protected void InitializeStats()
    {
        walkSpeed = statsSO.walkSpeed;
        runningSpeed = statsSO.runningSpeed;
        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        currentHealth = statsSO.health;
        maxHealth = currentHealth;

        currentStamina = statsSO.stamina;
        maxStamina = currentStamina;    

    }

    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();


}
