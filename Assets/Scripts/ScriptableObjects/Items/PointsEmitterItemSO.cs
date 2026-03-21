using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Points Emitter", fileName = "Points Emitter")]
public class PointsEmitterItemSO : ConsumableItemSO
{
    [SerializeField] int pointsToGain;

    public int GetEmittedAmount() => pointsToGain;


    public void UseItem(PlayerPointsCollector collector)
    {
        collector.AddPoints(GetEmittedAmount());

    }
}





