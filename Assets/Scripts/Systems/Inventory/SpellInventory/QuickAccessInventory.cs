using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryItemSaveData
{
    public string id;
    public int quantity;
    public int quickSlotIndex; // -1 если нет

    public string instanceId;
    public bool isEquiped;
    public float durability;
}

[Serializable]
public class SaveInventoryData
{
    public List<InventoryItemSaveData> items;
    public int currentIndex;
}


public abstract class QuickAccessInventory : MonoBehaviour
{
    public static int QUICK_SLOTS_COUNT = 5;

    public List<ItemData> items = new List<ItemData>(); // основной инвентарь

    protected ItemData[] quickSlots;
    protected int currentIndex; //текущий индекс предмета в быстром слоте

    public ItemData CurrentItem =>
        quickSlots[currentIndex]; // текущий предмет в быстром доступе

    public event Action<ItemData> OnCurrentItemChanged;

    protected void BaseInit()
    {
        quickSlots = new ItemData[QUICK_SLOTS_COUNT];
        //SetDefaultQuickSlotData();
    }

    #region Save/Load
    public SaveInventoryData SaveInventoryData()
    {
        List<InventoryItemSaveData> datas = new List<InventoryItemSaveData>();

        foreach(ItemData item in items)
        {

            int quickSlotIndex = -1;

            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (quickSlots[i] == item)
                {
                    quickSlotIndex = i;
                    break;
                }
            }


            InventoryItemSaveData data = new InventoryItemSaveData()
            {
                id = item.itemSO.id,
                quantity = item.quantity,
                quickSlotIndex = quickSlotIndex,
                isEquiped = item.isEquiped,
                instanceId = item.instanceId,
                durability = item.durability,   
            };

            datas.Add(data);
        }

        return new SaveInventoryData()
        {
            items = datas,
            currentIndex = currentIndex,
        };
    }

    public virtual void LoadInventoryData(SaveInventoryData data)
    {
        if (data == null) return;

        var consumables = Resources.LoadAll<ItemSO>($"Items/");

        Dictionary<string, ItemSO> itemsMap = new Dictionary<string, ItemSO>();

        foreach (var item in consumables)
        {
            itemsMap[item.id] = item;
        }

        items = new List<ItemData>();

        foreach (var item in data.items)
        {
            if (!itemsMap.TryGetValue(item.id, out var so))
            {
                continue;
            }

            items.Add(new ItemData() { itemSO = so, quantity = item.quantity });


        }

        quickSlots = new ItemData[QUICK_SLOTS_COUNT];

        foreach (var savedItem in data.items)
        {
            if (savedItem.quickSlotIndex < 0 || savedItem.quickSlotIndex >= QUICK_SLOTS_COUNT)
                continue;

            var item = items.Find(x => x.itemSO.id == savedItem.id);

            if (item != null)
            {
                quickSlots[savedItem.quickSlotIndex] = item;
            }
        }

        currentIndex = data.currentIndex;

        NormalizeCurrentIndex();

        // 6. Обновляем UI / подписчиков
        Notify();

    }

    #endregion

    #region Quick Slot

    public List<ItemData> GetQuickAccessData()
    {
        
        if (quickSlots.Length == 0) return null;
        return quickSlots.Where(x => x != null).ToList();

    }
    public void SetDefaultQuickSlotData()
    {
        if (items.Count == 0) return;

        AddToQuickAccess(items[0]);
    }

    public void AddToQuickAccess(ItemData item)
    {
        if (item == null) return;

        // Уже есть?
        if (quickSlots.Any(x => x == item))
            return;

        // Ищем свободный слот
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i] == null)
            {
                quickSlots[i] = item;
               
                Notify();
                return;
            }
        }

        // Если все заняты — заменяем последний
        quickSlots[quickSlots.Length - 1] = item;
        currentIndex = quickSlots.Length - 1;
        //Notify();
    }

    public void RemoveFromQuickAccess(ItemData item)
    {
        if (item == null) return;

        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i] == item)
            {
                quickSlots[i] = null;
            }
        }

        NormalizeCurrentIndex();
        Notify();
    }
    #endregion

    #region Inventory

    public void AddItemToInventory(ItemData item)
    {
        if (item == null) return;

        // Если предмет неаккумулируемый — всегда добавляем новый экземпляр
        if (!item.itemSO.IsStackable())
        {
            items.Add(item);
            Notify();
            return;
        }

        // Для аккумулируемых ищем существующий слот
        var match = items.Find(x => x.itemSO.id == item.itemSO.id);
        if (match == null)
        {
            items.Add(item);
        }
        else
        {
            match.quantity += item.quantity;
        }

        Notify();
    }

    public abstract void UseItem(ItemData data);


    protected void RemoveFromInventory(ItemData item)
    {
        if (item == null) return;

        items.Remove(item);
        RemoveFromQuickAccess(item); // автоматическая синхронизация
    }

    public void TopUpCurrentItem(int quantity)
    {
        if (CurrentItem != null)
            CurrentItem.quantity += quantity;
    }


    #endregion

    #region Selection

    public virtual void Change(int direction)

    {
        if (quickSlots.All(x => x == null))
            return;

        int startIndex = currentIndex;

        do
        {
            currentIndex = (currentIndex + direction + quickSlots.Length) % quickSlots.Length;
        }
        while (quickSlots[currentIndex] == null && currentIndex != startIndex);

        Notify();
    }

    protected void NormalizeCurrentIndex()
    {
        if (quickSlots.All(x => x == null))
        {
            currentIndex = 0;
            return;
        }

        if (quickSlots[currentIndex] == null)
        {
            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (quickSlots[i] != null)
                {
                    currentIndex = i;
                    break;
                }
            }
        }
    }

    #endregion

    #region Notify

    protected void Notify()
    {
        OnCurrentItemChanged?.Invoke(CurrentItem);
    }

    #endregion
}