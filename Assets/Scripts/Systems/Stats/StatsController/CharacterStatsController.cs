using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatsController : BaseStatsController 
{
    public HumanoidStatsSO statsSO;

    
    [Header("jumping")]
    public float jumpHeight;
    public float jumpTimer;


    public int currentHealthLevel;
    public int currentStaminaLevel;

    public void Init()
    {

        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        currentHealthLevel = statsSO.startHealthLevel;
        currentStaminaLevel = statsSO.startStaminaLevel;

        Health = new HealthModel(statsSO.baseHealth, currentHealthLevel);
        Stamina = new StaminaModel(statsSO.baseStamina,currentStaminaLevel, statsSO.staminaMinRegenDelay, statsSO.staminaMaxRegenDelay, statsSO.staminaRegenRate);
        Speed = new SpeedModel(statsSO.walkSpeed, statsSO.runningSpeed, statsSO.strafeSpeed);

        Health.UpdateMaxAndCurrent(currentHealthLevel, statsSO.baseHealth);
        Stamina.UpdateMaxAndCurrent(currentStaminaLevel, statsSO.baseStamina);


        ResetAllStats();
    }

    public void UpdateStat()
    {
        currentHealthLevel++;
        Health.UpdateMaxAndCurrent(currentHealthLevel, statsSO.baseHealth);
    }


    public void ResetAllStats()
    {
        Health.ResetHealth();
        Stamina.ResetStamina();
    }

    private void Update()
    {
        HandleStaminaRegen();

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateStat();   
        }
    }

    #region Stamina Control

    public void HandleStaminaRegen()
    {
        Stamina.Regen();
    }
    #endregion
}
