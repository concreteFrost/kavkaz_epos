using System.Collections.Generic;
using System;

[Serializable]
public class PlayerState
{
    public float[] playerPosition = new float[3];
    public float[] respawnPosition = new float[3];  
    //player stats
    public CharacterStatsData statsData;
    public CharacterLevelData levelData;
    public List<SavedEffectData> effectData;
    public SaveInventoryData spellInventoryData;
    public SaveInventoryData consumableInventoryData;
    public SaveInventoryData weaponsData;
    public SaveInventoryData questItemsData;

}
