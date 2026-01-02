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
    public HealthModel Health;
    public float maxHealth;

    [Header("stamina")]
    public StaminaModel Stamina;
    public float maxStamina;
    public float staminaRunReducePenalty = 0.03f;
    public float staminaJumpReducePenalty = 2f;
    public float staminaMinRegenDelay = 2f;
    public float staminaMaxRegenDelay = 5f;
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

        Health = new HealthModel(statsSO.health);
        maxHealth = Health.Current;

        Stamina = new StaminaModel(statsSO.stamina, staminaMinRegenDelay, staminaMaxRegenDelay, staminaRegenRate);
        maxStamina = Stamina.Current;    

    }


}
