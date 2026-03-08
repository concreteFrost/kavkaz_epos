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
            case ContinuousStatModifierItemSO continuousItem:
                continuousItem.UseItem(statsModifier);
                break;
            case InstantStatModifierItemSO instantItem:
               instantItem.UseItem(statsModifier);
                break;

            default:
                Debug.LogWarning($"UseItem: непредусмотренный тип предмета {item.itemSO.GetType().Name}");
                break;
        }
    }
}
