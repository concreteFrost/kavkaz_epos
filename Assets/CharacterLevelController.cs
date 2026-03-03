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

        //автоматический апп очков если они превышает порог вхождения на следующий уровень
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
    }

    private void CalculateXPToNextLevel()
    {
        levelData.xpToNextLevel = levelData.baseLevelCost +
                                  levelData.currentCharacterLevel * levelData.currentCharacterLevel *
                                  levelData.costMultiplier;
    }

    public bool SpendPoint(StatType type)
    {
        if (levelData.unspentPoints <= 0)
            return false;

        levelData.unspentPoints--;
        statsController.IncreaseStat(type);
        return true;
    }
}
