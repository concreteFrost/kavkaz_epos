using UnityEngine;

[CreateAssetMenu(fileName = "item_key", menuName = ScriptablePaths.ITEMS_PATH + "/Keys")]
public class KeyItemSO : ConsumableItemSO
{
    public override bool IsStackable() => false;
    public override bool CanUse() => false;
    public override bool CanEquip() => false;
    public override bool CanAddToSlot() => false;
    public override bool CanRemoveFromSlot() => false;
    public override bool CanDestroy() => false;
}
