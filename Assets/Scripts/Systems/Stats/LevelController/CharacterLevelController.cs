using System;
using UnityEngine;


[Serializable]
public class CharacterLevelData
{

    public int currentXP = 0;
    public int xpToNextLevel = 100;

    public int currentCharacterLevel = 1;
    public int unspentPoints = 0;

    public int baseLevelCost = 100;
    public int costMultiplier = 15;

}

public class CharacterLevelController : MonoBehaviour
{
    CharacterStatsController statsController;
    public CharacterLevelData levelData;

    public static Action XpGained;
    public static Action NewLevelReachedWithMessage;
    public static Action PointsSpent;

    public static Action NewLevelReached;

    public CharacterStatsController GetStatsController() => statsController;

    public void Init(CharacterStatsController statsController)
    {
        levelData = new CharacterLevelData();
        this.statsController = statsController;
        CalculateXPToNextLevel();
    }

    public CharacterLevelData SaveLevelData()
    {
        return new CharacterLevelData()
        {
            currentXP = levelData.currentXP,
            currentCharacterLevel = levelData.currentCharacterLevel,
            unspentPoints = levelData.unspentPoints
        };
    }

    public void LoadLevelData(CharacterLevelData data)
    {
        levelData.currentXP = data.currentXP;
        levelData.unspentPoints = data.unspentPoints;
        levelData.currentCharacterLevel = data.currentCharacterLevel;

        CalculateXPToNextLevel();
        XpGained?.Invoke();
        PointsSpent?.Invoke();  
        NewLevelReached?.Invoke();  
       
    }

    public void AddXP(int amount)
    {
        levelData.currentXP += amount;
        XpGained?.Invoke();

        //автоматический апп очков если они превышает порог вхождени€ на следующий уровень
        while (levelData.currentXP >= levelData.xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        levelData.currentXP -= levelData.xpToNextLevel;
        levelData.currentCharacterLevel++;
        levelData.unspentPoints += 3;
        CalculateXPToNextLevel();   

        statsController.Health.ResetCurrent();
        NewLevelReachedWithMessage?.Invoke();  
    }

    private void CalculateXPToNextLevel()
    {
        levelData.xpToNextLevel = levelData.baseLevelCost +
                                  levelData.currentCharacterLevel * levelData.currentCharacterLevel *
                                  levelData.costMultiplier;
    }

    public void SpendPoint(StatType type)
    {
        statsController.IncreaseStatLevel(type);      
    }

    public int GetUnspentPoints()=> levelData.unspentPoints;

    public void ReserveSpendPoint() => levelData.unspentPoints--;

    public void RefundSpendPoint()
    {
        // ћаксимум не больше начального количества очков на уровне
        if (levelData.unspentPoints < 3)
            levelData.unspentPoints++;
    }


}
