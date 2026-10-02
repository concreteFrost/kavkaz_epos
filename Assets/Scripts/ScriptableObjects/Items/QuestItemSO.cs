
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.ITEMS_PATH + "/Quest Item", fileName = "Quest Item")]
public class QuestItemSO : ConsumableItemSO
{
    public override bool IsStackable() => false;

    public override bool CanUse() => false;
    public override bool CanEquip() => false;
    public override bool CanAddToSlot() => false;
    public override bool CanRemoveFromSlot() => false;

    public override bool CanDestroy() => false;

}

