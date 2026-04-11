using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterLootDistributer : MonoBehaviour
{
    [SerializeField] private LootContainerSO listSO;
    public bool hasDroped = false;

    public static Action<List<ItemData>> LootDroppedInInvalidArea;

   
    private List<ItemData> GenerateItemsToDrop(List<DropableItem> possibleItems)
    {

        List<ItemData> itemsToDrop = new List<ItemData>();
        foreach (var item in possibleItems)
        {
            
            var quantityToGet = UnityEngine.Random.Range(item.minQuantity, item.maxQuantity + 1);
            if (UnityEngine.Random.value <= item.dropChance)
            { 
                itemsToDrop.Add(new ItemData() { itemSO = item.itemSO, quantity= quantityToGet});
            }
        }

        return itemsToDrop;
    }


    public void HandleLootGenerate(Vector3 dropPosition)
    {
       
        hasDroped = true;

        var generatedItems = GenerateItemsToDrop(listSO.possibleItems);
        if (generatedItems.Count == 0) return;

        if (TryGetValidDropPosition(dropPosition, out var validPosition))
        {
            var lootData = new DynamicLootState();
            lootData.instanceId = Guid.NewGuid().ToString();
            lootData.lootPosition = new float[3]
            {
            validPosition.x,
            validPosition.y,
            validPosition.z
            };

            foreach (var item in generatedItems)
            {
                lootData.droppedItems.Add(new DroppedItemsData
                {
                    itemId = item.itemSO.id,
                    quantity = item.quantity
                });
            }

            DynamicLootManager.DynamicLootDropped?.Invoke(lootData);
        }
        else
        {
            LootDroppedInInvalidArea?.Invoke(generatedItems);
        }
    }


    private bool TryGetValidDropPosition(Vector3 originalPosition, out Vector3 result)
    {
        NavMeshHit hit;

        float searchRadius = 5f; // можно вынести в SerializeField

        if (NavMesh.SamplePosition(originalPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            result = hit.position + Vector3.up * 0.1f;
            return true;
        }

        result = Vector3.zero;
        return false;
    }



}
