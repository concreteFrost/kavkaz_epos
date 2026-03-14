using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Points Emitter", fileName = "Points Emitter")]
public class PointsEmitterItemSO : ConsumableItemSO
{
    public int amount =1;

    public void UseItem(PlayerPointsCollector collector)
    {
        collector.AddPoints(amount);

    }
}





