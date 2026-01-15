using UnityEngine;

public class HumanoidStats : MonoBehaviour
{

    public HumanoidStatsSO statsSO;

    [Header("movement speed")]
    public float walkSpeed;
    public float runningSpeed;
    public float strafeSpeed;

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
    public float staminaJumpReducePenalty = 7f;
    public float staminaDodgeReducePenalty = 10f;
    public float staminaMinRegenDelay = 2f;
    public float staminaMaxRegenDelay = 6f;
    public float staminaRegenRate = 15f;

    [Header("balance")]
    public float currentBalance = 0f;

    [Header("stats debug")]
    public float d_stamina;
    public float d_health;

    protected void InitializeStats()
    {
        walkSpeed = statsSO.walkSpeed;
        runningSpeed = statsSO.runningSpeed;
        strafeSpeed = statsSO.strafeSpeed;  
        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        Health = new HealthModel(statsSO.health);
        maxHealth = Health.Current;

        Stamina = new StaminaModel(statsSO.stamina, staminaMinRegenDelay, staminaMaxRegenDelay, staminaRegenRate);
        maxStamina = Stamina.Current;

    }

    public void Init()
    {
        InitializeStats();
    }

    private void Update()
    {
        d_stamina = Stamina.Current;
        d_health = Health.Current;  
    }



}
