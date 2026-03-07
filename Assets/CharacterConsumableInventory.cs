using UnityEngine;

public class CharacterConsumableInventory : QuickAccessInventory
{
    ICombatInventory combatInventory;
    CharacterStatsModifier statsModifier;
   
    public void Init(ICombatInventory combatInventory, CharacterStatsModifier statsModifier)
    {
        BaseInit();
        this.combatInventory = combatInventory;
        this.statsModifier = statsModifier;  
    }

    public override void UseItem()
    {
        if (CurrentItem == null) return;

        var item = CurrentItem;
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

            case InstantStatModifierItemSO statItem:
                statItem.UseItem(statsModifier);
                break;

            default:
                Debug.LogWarning($"UseItem: непредусмотренный тип предмета {item.itemSO.GetType().Name}");
                break;
        }
    }
}
