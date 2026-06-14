using UnityEngine;

[CreateAssetMenu(fileName = "Item Money", menuName = ScriptablePaths.ITEMS_PATH + "/Money")]
public class MoneyItemSO : ItemSO
{
    public override bool IsStackable()
    {
        return true;
    }
}
