using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public HumanoidStatsSO statsSO;
    
    public SpeedModel Speed;
    public HealthModel Health;
    public StaminaModel Stamina;

    [Header("jumping")]
    public float jumpHeight;
    public float jumpTimer;

    public void Init()
    {

        Speed = new SpeedModel(statsSO.walkSpeed, statsSO.runningSpeed, statsSO.strafeSpeed);

        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        Health = new HealthModel(statsSO.health);
        Stamina = new StaminaModel(statsSO.stamina, statsSO.staminaMinRegenDelay, statsSO.staminaMaxRegenDelay, statsSO.staminaRegenRate);

        ResetAllStats();
    }

    public void ResetAllStats()
    {
        Health.ResetHealth();
        Stamina.ResetStamina();
    }

    private void Update()
    {
        HandleStaminaRegen();
    }

    #region Stamina Control

    public void HandleStaminaRegen()
    {
        Stamina.Regen();
    }
    #endregion
}
