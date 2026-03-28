using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class DynamicLootState
{
    public string instanceId;
    public float[] lootPosition = new float[3];
    public List<DroppedItemsData> droppedItems = new List<DroppedItemsData>();

}

[Serializable]
public class DroppedItemsData
{
    public string itemId;
    public int quantity;

}

public class DynamicLootManager : MonoBehaviour
{
    public static Action<DynamicLootState> DynamicLootDropped;
    public static Action<string> LootCollected;

    public List<DynamicLootState> dynamicLootData = new();
    public GameObject lootPrefab;

    List<ItemSO> dataBaseItems = new List<ItemSO>();

    public void Init()
    {
        dataBaseItems = Resources.LoadAll<ItemSO>("Items/").ToList();

        LootCollected += RemoveLoot;
        DynamicLootDropped += OnDynamicLootDropped;
    }

    private void OnDisable()
    {
        LootCollected -= RemoveLoot;    
        DynamicLootDropped -= OnDynamicLootDropped;
    }

    public void OnDynamicLootDropped(DynamicLootState data)
    {
        dynamicLootData.Add(data);

        Vector3 pos = new Vector3(
            data.lootPosition[0],
            data.lootPosition[1],
            data.lootPosition[2]);

        DropLoot(data);
    }

    public void OnLootCollected(string id) => RemoveLoot(id);   

    private List<ItemData> GetItemsFromDataBase(List<DroppedItemsData> data)
    {
        List<ItemData> items = new List<ItemData>();    
        foreach (var dropped in data)
        {
            var match = dataBaseItems.Find((x) => x.id == dropped.itemId);

            if (match != null)
            {
                var itemData = new ItemData()
                {
                    itemSO = match,
                    quantity = dropped.quantity,
                };

                items.Add(itemData);
            }
        }

        return items;   
    }

    public List<DynamicLootState> SaveDynamicLoot()
    {
        List<DynamicLootState> dynamic = new List<DynamicLootState>();    

        foreach(var item in dynamicLootData)
        {
            DynamicLootState dynamicLoot = new DynamicLootState();
            dynamicLoot.instanceId = item.instanceId;   
            dynamicLoot.lootPosition = item.lootPosition;
            dynamicLoot.droppedItems = item.droppedItems;   

            dynamic.Add(dynamicLoot);
        }

        return dynamic;
    }

    public void LoadDynamicLootData(List<DynamicLootState> loadedLoot)
    {
        // 1. Удаляем все текущие объекты
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        // 2. Очищаем runtime данные
        dynamicLootData.Clear();

        if (loadedLoot == null || loadedLoot.Count == 0)
            return;

        // 3. Восстанавливаем
        foreach (var data in loadedLoot)
        {
            // защита от кривых данных
            if (data == null || data.droppedItems == null)
                continue;

            dynamicLootData.Add(data);

            DropLoot(data);
        }
    }

    private void DropLoot(DynamicLootState data)
    {
        Vector3 dropPosition = new Vector3(data.lootPosition[0], data.lootPosition[1],data.lootPosition[2]);    
        var go = Instantiate(lootPrefab,dropPosition, Quaternion.identity,this.transform);

        var holder = go.GetComponent<DynamicLootHolder>();
       
        holder.Init();
        holder.SetInstanceId(data.instanceId);
        holder.AddItemsFromDistributer(GetItemsFromDataBase(data.droppedItems));
    }

    private void RemoveLoot(string id)
    {
        dynamicLootData.RemoveAll(x => x.instanceId == id);
    }
}