using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Weapon Upgrader", fileName = "item_weapon_upgrade")]
public class WeaponUpgradeItemSO : ConsumableItemSO
{
    public override bool IsStackable() => true;

    public override bool CanUse() => false;
    public override bool CanEquip() => false;
    public override bool CanAddToSlot() => true;
    public override bool CanRemoveFromSlot() => true;

    public override bool CanDestroy() => false;
}
