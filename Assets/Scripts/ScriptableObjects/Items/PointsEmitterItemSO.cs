using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Points Emitter", fileName = "Points Emitter")]
public class PointsEmitterItemSO : ConsumableItemSO,IItemStats
{
    [SerializeField] int pointsToGain;

    public int GetEmittedAmount() => pointsToGain;


    public void UseItem(PlayerPointsCollector collector)
    {
        collector.AddPoints(GetEmittedAmount());

    }

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat(ItemStatType.pointsTopUp, GetEmittedAmount(), ItemStatFormatType.flat),
        
    };

}





