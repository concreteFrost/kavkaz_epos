using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CharacterStatsData
{
    public int healthLevel;
    public int staminaLevel;
    public int knowledgeLevel;
    public int strengthLevel;

    public float currentHealth;
    public float currentStamina;
    public float currentKnowledge;
    public float currentStrength;
}


public class CharacterStatsController : BaseStatsController
{
    public KnowledgeModel Knowledge;
    public StrengthModel Strength;

    public HumanoidStatsSO statsSO;
    //public CharacterStatsLevelSO statsLevelSO;

    [Header("jumping")]
    public float jumpHeight;
    public float jumpTimer;

    [Header("Initial Levels")]
    public int initialHealthLevel=1;
    public int initialStaminaLevel=1;
    public int initialKnowledgeLevel=1;
    public int initialStrengthLevel=1;

    [Header("Levels")]
    public int healthLevel;
    public int staminaLevel;
    public int knowledgeLevel;
    public int strengthLevel;

    public void Init()
    {

        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;

        healthLevel =initialHealthLevel;
        staminaLevel = initialStaminaLevel;
        knowledgeLevel = initialKnowledgeLevel;
        strengthLevel = initialStrengthLevel;

        Health = new HealthModel(statsSO.baseHealth, statsSO.statMinRegenDelay, statsSO.statMaxRegenDelay, statsSO.statRegenRate);
        Stamina = new StaminaModel(statsSO.baseStamina, statsSO.statMinRegenDelay, statsSO.statMaxRegenDelay, statsSO.statRegenRate);
        Speed = new SpeedModel(statsSO.walkSpeed, statsSO.runningSpeed, statsSO.strafeSpeed);
        Knowledge = new KnowledgeModel(statsSO.baseKnowledge);
        Strength = new StrengthModel(statsSO.baseStrength);

        Health.UpdateMaxAndCurrent(healthLevel);
        Stamina.UpdateMaxAndCurrent(staminaLevel);
        Knowledge.UpdateMaxAndCurrent(knowledgeLevel);
        Strength.UpdateMaxAndCurrent(strengthLevel);

        ResetAllStats();


    }

    private void Start()
    {
       
    }

    private void Update()
    {
        Stamina.Regen();
    }


    public CharacterStatsData SaveStatsData()
    {
        return new CharacterStatsData()
        {
            healthLevel = healthLevel,
            staminaLevel = staminaLevel,
            knowledgeLevel = knowledgeLevel,
            strengthLevel = strengthLevel,
            currentHealth = Health.Current,
            currentStamina = Stamina.Current,
            currentKnowledge = Knowledge.Current,
            currentStrength = Strength.Current
        };
    }

    public void LoadStatsData(CharacterStatsData statsData)
    {

        
        healthLevel = statsData.healthLevel;
        staminaLevel = statsData.staminaLevel;
        knowledgeLevel = statsData.knowledgeLevel;
        strengthLevel = statsData.strengthLevel;


        Health.UpdateMaxAndCurrent(healthLevel);
        Health.Current = statsData.currentHealth;
        Health.NotifyCurrentChange(Health.Current);
        Health.CalculateNextLevel(healthLevel);

        Strength.UpdateMaxAndCurrent(strengthLevel);
        Strength.Current = statsData.currentStrength;
        Strength.NotifyCurrentChange(Strength.Current);
        Strength.CalculateNextLevel(strengthLevel);

        Stamina.UpdateMaxAndCurrent(staminaLevel);
        Stamina.Current = statsData.currentStamina;
        Stamina.NotifyCurrentChange(Stamina.Current);
        Stamina.CalculateNextLevel(staminaLevel);

        Knowledge.UpdateMaxAndCurrent(knowledgeLevel);
        Knowledge.Current = statsData.currentKnowledge;
        Knowledge.NotifyCurrentChange(Knowledge.Current);
        Knowledge.CalculateNextLevel(knowledgeLevel);

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
            case StatType.Strength:
                return Strength;
            default:
                return null;
        }
    }

    public IModifiedStat GetModifiedStat(ModifiedModelType type)
    {
        switch (type)
        {
            case ModifiedModelType.Health:
                return Health;
            case ModifiedModelType.Stamina:
                return Stamina;
            case ModifiedModelType.Strength:
                return Strength;
            case ModifiedModelType.Knowledge:
                return Knowledge;
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
            case StatType.Strength:
                strengthLevel++;
                Strength.UpdateMaxAndCurrent(strengthLevel);
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
            case StatType.Strength: return strengthLevel;
        }

        return 0;
    }



    public void ResetAllStats()
    {
        Health.ResetCurrent();
        Stamina.ResetCurrent();
        Strength.ResetCurrent();
    }


}
