using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{

    public BaseCharacterStatsSO statsSO;

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
    public float staminaRegenTimer = 0.0f;

    [Header("balance")]
    public float currentBalance = 0f;

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

}
