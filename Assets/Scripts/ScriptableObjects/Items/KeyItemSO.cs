using UnityEngine;

[CreateAssetMenu(fileName = "item_key", menuName = ScriptablePaths.ITEMS_PATH + "/Keys")]
public class KeyItemSO : ItemSO
{
    public override bool IsStackable()
    {
        return false;
    }
}
