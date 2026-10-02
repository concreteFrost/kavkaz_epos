using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumableInventory : BaseInventory<ConsumableData>
{
    IWeaponSetter combatInventory;
    CharacterStatsModifier statsModifier;
    PlayerPointsCollector pointsCollector;
   
    public void Init(IWeaponSetter combatInventory, CharacterStatsModifier statsModifier, PlayerPointsCollector pointsCollector)
    {
        BaseInit();
        this.combatInventory = combatInventory;
        this.statsModifier = statsModifier; 
        this.pointsCollector = pointsCollector; 
    }

    private void Start()
    {
        Notify();
    }


    public override void UseItem(ConsumableData data)
    {
        if (data == null) return;

        var item = data;
        item.quantity--;

        Notify(); //уведомл€ет

        ApplyItemEffect(item);

        Debug.Log("item used");

        if (item.quantity <= 0)
        {
            RemoveFromInventory(item);
            return;
        }
    }

    private void ApplyItemEffect(ConsumableData item)
    {
       
        switch (item.itemSO)
        {
            case WeaponModifierItemSO weaponItem:
                weaponItem.UseItem(combatInventory);
                break;
            case StatModifierItemSO continuousItem:
                continuousItem.UseItem(statsModifier);
                break;
            case PointsEmitterItemSO pointsEmitter:
                pointsEmitter.UseItem(pointsCollector);
                break;
            default:
                Debug.LogWarning($"UseItem: непредусмотренный тип предмета {item.itemSO.GetType().Name}");
                break;
        }
    }

    public void AddAllItemsOnStart()
    {
        var allItems = Resources.LoadAll<ConsumableItemSO>("Items/Consumable/");


        foreach (var item in allItems)
        {
            var data = new ConsumableData
            {
                itemSO = item,
                quantity = 20
            };

            AddItemToInventory(data);
            AddToQuickAccess(data);

        }
    }

}
