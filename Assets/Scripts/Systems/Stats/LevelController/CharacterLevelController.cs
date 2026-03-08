using System;
using UnityEngine;

[System.Serializable]
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

    public Action XpGained;
    public Action NewLevelReached;
    public Action PointsSpent;

    public CharacterStatsController GetStatsController() => statsController;

    public void Init(CharacterStatsController statsController)
    {
        levelData = new CharacterLevelData();
        this.statsController = statsController; 
    }

    private void Start()
    {
        CalculateXPToNextLevel();
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
        NewLevelReached?.Invoke();  
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
