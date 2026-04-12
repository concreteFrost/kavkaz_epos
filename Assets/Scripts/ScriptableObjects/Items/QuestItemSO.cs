
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.ITEMS_PATH + "/Quest Item", fileName = "Quest Item")]
public class QuestItemSO : ConsumableItemSO
{
    public override bool IsStackable() => false;

}

