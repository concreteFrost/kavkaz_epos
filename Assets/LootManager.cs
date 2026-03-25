using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public List<BaseLootHolder> loots = new List<BaseLootHolder>(); 

    public void Init()
    {
        loots.Clear();
        loots.AddRange(GetComponentsInChildren<BaseLootHolder>());
            

        foreach(var loot in loots)
        {
            loot.Init();
        }
    }

    public List<LootState> SaveLootData()
    {
        List<LootState> savedLoot = new List<LootState>();
        foreach(var loot in loots)
        {
            savedLoot.Add(loot.SaveLootData());
        }

        return savedLoot;
    }

    public void LoadLootData(List<LootState> savedLoot)
    {
        foreach (var loot in loots)
        {
            var state = savedLoot.Find(x => x.lootId == loot.id);
            if (state != null)
            {
                loot.LoadLootData(state);   
            }
        }
    }
}
