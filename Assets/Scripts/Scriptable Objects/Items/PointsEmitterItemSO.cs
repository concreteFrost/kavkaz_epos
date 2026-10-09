using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Points Emitter", fileName = "Points Emitter")]
public class PointsEmitterItemSO : ConsumableItemSO,IItemStats
{
    [SerializeField] int pointsToGain;

    public override bool IsStackable() => true;
   

    public int GetEmittedAmount() => pointsToGain;


    public void UseItem(PlayerPointsCollector collector)
    {
        collector.AddPoints(GetEmittedAmount());

    }

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("points topup", GetEmittedAmount(), ItemStatFormatType.flat),
        
    };

    public override bool CanUse() => true;
    public override bool CanEquip() => false;
    public override bool CanAddToSlot() => true;
    public override bool CanRemoveFromSlot() => true;

    public override bool CanDestroy() => false;

}





