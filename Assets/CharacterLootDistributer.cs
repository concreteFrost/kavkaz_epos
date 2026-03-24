using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterLootDistributer : MonoBehaviour
{
    [SerializeField] private LootContainerSO listSO;
    public bool hasDroped = false;

    public static Action<List<ItemData>> LootDroppedInInvalidArea;

    public void Init()
    {
    }
   
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
            DropLoot(validPosition, generatedItems);
        }
        else
        {
            // fallback Ч тут уже тво€ логика
            Debug.Log("loot spawned in invalid area");
            LootDroppedInInvalidArea?.Invoke(generatedItems); // лучше List<ItemData>
           
        }
    }

    private void DropLoot(Vector3 dropPosition, List<ItemData> generatedItems)
    {
        var go = Instantiate(listSO.lootContainerPrefab, dropPosition, Quaternion.identity);

        var holder = go.GetComponent<DynamicLootHolder>();
        holder.Init();
        holder.AddItemsFromDistributer(generatedItems);
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
