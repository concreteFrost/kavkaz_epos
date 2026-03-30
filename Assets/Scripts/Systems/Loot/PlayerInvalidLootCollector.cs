using System.Collections.Generic;
using UnityEngine;

public class PlayerInvalidLootCollector : MonoBehaviour
{
    PlayerInteractionController collector;


    public void Init(PlayerInteractionController collector)
    {
        this.collector = collector;
        CharacterLootDistributer.LootDroppedInInvalidArea += OnLootDroppedInInvalidArea;
    }

    private void OnDisable()
    {
        CharacterLootDistributer.LootDroppedInInvalidArea -= OnLootDroppedInInvalidArea;    
    }
    public void OnLootDroppedInInvalidArea(List<ItemData> data)
    {
        foreach (var item in data)
        {
            collector.DistributeItemToInventory(item);
        }
       
    }
}
