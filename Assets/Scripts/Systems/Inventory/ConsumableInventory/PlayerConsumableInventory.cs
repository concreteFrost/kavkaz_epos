using UnityEngine;

public class PlayerConsumableInventory : QuickAccessInventory
{
    ICombatInventory combatInventory;
    CharacterStatsModifier statsModifier;
    PlayerPointsCollector pointsCollector;
   
    public void Init(ICombatInventory combatInventory, CharacterStatsModifier statsModifier, PlayerPointsCollector pointsCollector)
    {
        BaseInit();
        this.combatInventory = combatInventory;
        this.statsModifier = statsModifier; 
        this.pointsCollector = pointsCollector; 
    }


    public override void UseItem(ItemData data)
    {
        if (data == null) return;

        var item = data;
        item.quantity--;

        if (item.quantity <= 0)
        {
            RemoveFromInventory(item);
            return;
        }

        Notify(); //уведомл€ет

        ApplyItemEffect(item);
    }

    private void ApplyItemEffect(ItemData item)
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
        var allItems = Resources.LoadAll<ItemSO>("Items/Consumable_Items/");


        foreach (var item in allItems)
        {
            var data = new ItemData
            {
                itemSO = item,
                quantity = 20
            };

            AddItemToInventory(data);
            AddToQuickAccess(data);

        }
    }
}
