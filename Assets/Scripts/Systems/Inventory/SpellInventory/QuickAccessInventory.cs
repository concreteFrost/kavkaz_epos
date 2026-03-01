using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class QuickAccessInventory : MonoBehaviour
{
    public static int QUICK_SLOTS_COUNT = 5;

    public List<ItemData> items = new List<ItemData>(); // основной инвентарь

    private ItemData[] quickSlots;
    private int currentIndex; //текущий индекс предмета в быстром слоте

    public ItemData CurrentItem =>
        quickSlots[currentIndex]; // текущий предмет в быстром доступе

    public event Action<ItemData> OnCurrentItemChanged;

    #region Quick Access

    public List<ItemData> GetQuickAccessData() => quickSlots.Where(x => x != null).ToList();

    public void Init()
    {
        quickSlots = new ItemData[QUICK_SLOTS_COUNT];
        SetDefaultQuickSlotData();
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

    protected void RemoveFromInventory(ItemData item)
    {
        if (item == null) return;

        items.Remove(item);
        RemoveFromQuickAccess(item); // автоматическая синхронизация
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

    private void NormalizeCurrentIndex()
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