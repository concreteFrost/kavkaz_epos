using System.Collections.Generic;
using System;

[Serializable]
public class PlayerState
{
    public float[] playerPosition = new float[3];
    //player stats
    public CharacterStatsData statsData;
    public CharacterLevelData levelData;
    public List<SavedEffectData> effectData;
    public SaveInventoryData spellInventoryData;
    public SaveInventoryData consumableInventoryData;
    public SaveInventoryData weaponsData;

}

[Serializable]
public class EnemyState
{
    public string enemyId;
    public float[] enemyPosition = new float[3];    

    public CharacterStatsData statsData;
    public List<SavedEffectData> effectData;

}