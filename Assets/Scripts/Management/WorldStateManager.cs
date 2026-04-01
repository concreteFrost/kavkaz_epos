using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager
{
    private Dictionary<string, LevelState> allLevels = new Dictionary<string, LevelState>();

    public void SaveLevel(LevelManager level)
    {
        if (level == null) return;

        LevelState state = level.SaveLevelState();
        allLevels[state.levelId] = state;
    }

    public void LoadLevel(LevelManager level)
    {
        if (level == null) return;

        if (allLevels.TryGetValue(level.GetLevelName(), out LevelState state))
        {
            level.LoadLevelState(state);
        }
    }

    public void LoadFromSaveData(List<SaveLevelData> levelDatas)
    {
        allLevels.Clear();
        foreach (var data in levelDatas)
        {
            allLevels[data.levelName] = data.levelState;
        }
    }

    public List<SaveLevelData> GetSaveData()
    {
        var list = new List<SaveLevelData>();
        foreach (var kv in allLevels)
        {
            list.Add(new SaveLevelData
            {
                levelName = kv.Key,
                levelState = kv.Value
            });
        }
        return list;
    }
}