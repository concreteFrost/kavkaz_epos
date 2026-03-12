using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatsController : BaseStatsController 
{
    public KnowledgeModel Knowledge;

    public HumanoidStatsSO statsSO;
    public CharacterStatsLevelSO statsLevelSO;

    [Header("jumping")]
    public float jumpHeight;
    public float jumpTimer;

    [Header("Levels")]
    public int healthLevel;
    public int staminaLevel;
    public int knowledgeLevel;

    public void Init()
    {

        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        healthLevel = statsLevelSO.startHealthLevel;
        staminaLevel = statsLevelSO.startStaminaLevel;
        knowledgeLevel = statsLevelSO.startKnowledgeLevel;

        Health = new HealthModel(statsSO.baseHealth, statsSO.statMinRegenDelay, statsSO.statMaxRegenDelay, statsSO.statRegenRate);
        Stamina = new StaminaModel(statsSO.baseStamina, statsSO.statMinRegenDelay, statsSO.statMaxRegenDelay, statsSO.statRegenRate);
        Speed = new SpeedModel(statsSO.walkSpeed, statsSO.runningSpeed, statsSO.strafeSpeed);
        Knowledge = new KnowledgeModel(statsSO.baseKnowledge);

        ResetAllStats();
    }

    private void Start()
    {
        Health.UpdateMaxAndCurrent(healthLevel);
        Stamina.UpdateMaxAndCurrent(staminaLevel);
        Knowledge.UpdateMaxAndCurrent(knowledgeLevel);
    }

    private void Update()
    {
        Stamina.Regen();
    }

    public LevelStatModel GetStatModel(StatType type)
    {
        switch (type)
        {
            case StatType.Health:
                return Health;
            case StatType.Stamina:
                return Stamina;
            case StatType.Knowledge:
                return Knowledge;
            default:
                return null;
        }
    }

    public IStatModifier GetModifiedStat(ModifiedModelType type)
    {
        switch (type)
        {
            case ModifiedModelType.Health:
                return Health;
            case ModifiedModelType.Stamina:
                return Stamina;
            default:
                return null;
        }
    }

    public void IncreaseStatLevel(StatType type)
    {
        switch (type)
        {
            case StatType.Health:
                healthLevel++;
                Health.UpdateMaxAndCurrent(healthLevel);
                break;
            case StatType.Stamina:
                staminaLevel++;
                Stamina.UpdateMaxAndCurrent(staminaLevel);
                break;
            case StatType.Knowledge:
                knowledgeLevel++;
                Knowledge.UpdateMaxAndCurrent(knowledgeLevel);
                break;
        }
    }

    public int GetCurrentStatLevel(StatType model)
    {
        switch (model)
        {
            case StatType.Health: return healthLevel;
            case StatType.Stamina: return staminaLevel;
            case StatType.Knowledge: return knowledgeLevel;
        }

        return 0;
    }



    public void ResetAllStats()
    {
        Health.ResetCurrent();
        Stamina.ResetCurrent(); 
    }


}
