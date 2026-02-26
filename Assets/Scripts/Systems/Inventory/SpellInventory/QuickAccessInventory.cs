using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class QuickAccessItem
{
    public ItemData itemData;
}

public abstract class QuickAccessInventory : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();                  // полный инвентарь
    public List<ItemData> quickAccessItems = new List<ItemData>(); // быстрые слоты

    public int currentIndex;

    public ItemData CurrentItem =>
        quickAccessItems.Count == 0 ? null : quickAccessItems[currentIndex];

    public event Action<ItemData> OnCurrentItemChanged;

    public void AddToQuickAccess(ItemData item)
    {
        if (quickAccessItems.Any(x => x.itemSO.id == item.itemSO.id)) return;

        if (quickAccessItems.Count >= 5)
        {
            // заменяем последний слот
            quickAccessItems[quickAccessItems.Count - 1] = item;
        }
        else
        {
            quickAccessItems.Add(item);
        }

        Notify();
    }

    public void TryToRemoveFromQuickAccess(ItemSO d)
    {
        var toRemove = quickAccessItems.FirstOrDefault(x => x.itemSO.id == d.id);
        if (toRemove != null)
        {
            int removedIndex = quickAccessItems.IndexOf(toRemove);
            quickAccessItems.Remove(toRemove);

            // Корректируем currentIndex
            if (quickAccessItems.Count == 0)
            {
                currentIndex = 0;
            }
            else if (currentIndex >= quickAccessItems.Count)
            {
                currentIndex = quickAccessItems.Count - 1;
            }

            Notify();
        }
    }

    public List<ItemData> GetQuickAccessData() => quickAccessItems.Select(x => x).ToList();

    public virtual void Change(int direction)
    {
        if (quickAccessItems.Count == 0) return;
        currentIndex = (currentIndex + direction + quickAccessItems.Count) % quickAccessItems.Count;
        Notify();
    }

    protected void Notify() => OnCurrentItemChanged?.Invoke(CurrentItem);

    protected void RemoveAt(int index)
    {
        
        items.RemoveAt(index);

        if (items.Count == 0)
        {
            currentIndex = 0;
        }
        else if (currentIndex >= items.Count)
        {
            currentIndex = items.Count - 1;
        }
    }


    protected void TestQuickSlot()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var item in items)
            {
                // если предмет ещё не в quickAccessItems, добавляем
                if (!quickAccessItems.Any(x => x.itemSO.id == item.itemSO.id))
                {
                    AddToQuickAccess(item);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var item in items)
            {
                // ищем существующий слот с этим предметом
                var existingSlot = quickAccessItems.FirstOrDefault(x => x.itemSO.id == item.itemSO.id);

                if (existingSlot != null)
                {
                    TryToRemoveFromQuickAccess(existingSlot.itemSO);
                    break;

                }
            }
        }
    }
}