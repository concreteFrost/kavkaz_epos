using System.Collections.Generic;
using UnityEngine;

public class CharacterLootDistributer : MonoBehaviour
{
    [SerializeField] private LootContainerSO listSO;
    public bool hasDroped = false;

    public void Init()
    {
    }
   
    private List<ItemData> GenerateItemsToDrop(List<DropableItem> possibleItems)
    {

        List<ItemData> itemsToDrop = new List<ItemData>();
        foreach (var item in possibleItems)
        {
            
            var quantityToGet = Random.Range(item.minQuantity, item.maxQuantity + 1);
            if (Random.value <= item.dropChance)
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

        DropLoot(dropPosition,generatedItems);

    }

    private void DropLoot(Vector3 dropPosition, List<ItemData> generatedItems)
    {
        var go = Instantiate(listSO.lootContainerPrefab, dropPosition, Quaternion.identity);

        var holder = go.GetComponent<DynamicLootHolder>();
        holder.Init();
        holder.AddItemsFromDistributer(generatedItems);
    }



}
