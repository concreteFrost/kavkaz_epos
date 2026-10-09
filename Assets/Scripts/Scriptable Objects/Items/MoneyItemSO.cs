using UnityEngine;

[CreateAssetMenu(fileName = "Item Money", menuName = ScriptablePaths.ITEMS_PATH + "/Money")]
public class MoneyItemSO : ItemSO
{
    public override bool IsStackable()
    {
        return true;
    }

    public override bool CanUse() => false;
    public override bool CanEquip() => false;
    public override bool CanAddToSlot() => false;
    public override bool CanRemoveFromSlot() => false;

    public override bool CanDestroy() => false;
}
