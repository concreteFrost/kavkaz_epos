using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Weapon Status Effect Item", fileName = "Weapon Status Effect Item")]
public class WeaponModifierItemSO : ConsumableItemSO
{
    public float amount = 1f;

    public  void UseItem(ICombatInventory ctx)
    {
        if (ctx.CurrentWeapon == null) return;

        ctx.CurrentWeapon.IncreaseDurability(amount);   
    }
}





