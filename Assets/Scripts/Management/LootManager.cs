using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public List<BaseLootHolder> loots = new List<BaseLootHolder>(); 
    [SerializeField] private DynamicLootManager dynamicLootManager;

    public static Action StaticLootDataUpdated; // для ui обновлений


    public void Init()
    {

        if (dynamicLootManager == null)
        {
            dynamicLootManager = FindAnyObjectByType<DynamicLootManager>();
        }

        dynamicLootManager?.Init();  

        loots.Clear();
        loots.AddRange(GetComponentsInChildren<BaseLootHolder>());
            

        foreach(var loot in loots)
        {
            loot.Init();
        }

        StaticLootDataUpdated?.Invoke();
    }


    public void ClearDynamicLoot() => dynamicLootManager.CleadDynamicLoot();


    #region Save/Load
    public List<LootState> SaveLootData()
    {
        List<LootState> savedLoot = new List<LootState>();
        foreach(var loot in loots)
        {
            savedLoot.Add(loot.SaveLootData());
        }

        return savedLoot;
    }

    public List<DynamicLootState> SaveDynamicLoot()=> dynamicLootManager.SaveDynamicLoot();

    public void LoadDynamicLoot(LevelState state) => dynamicLootManager.LoadDynamicLootData(state.dynamicLootDatas);

    public void LoadLootData(LevelState state)
    {
        var savedLoot = state.staticLootDatas;
        foreach (var loot in loots)
        {
            var match = savedLoot.Find(x => x.lootId == loot.id);
            if (match != null)
            {
                loot.LoadLootData(match);   
            }
        }

        StaticLootDataUpdated?.Invoke();
    }
    #endregion 
}
