using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield", menuName = ScriptablePaths.ITEMS_PATH + "/Shields/Shield")]
public class ShieldSO : BreakablleItemSO, IItemStats
{

    [Space(10)]

    [Tooltip("Степень защиты в процентах")]
    [SerializeField] float defenceAmount;

    public float GetDefenceBonus() => defenceAmount;

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat(ItemStatType.defence, GetDefenceBonus(), ItemStatFormatType.percent),
        new ItemStat(ItemStatType.perHitCost, GetBreakdownPenalty(), ItemStatFormatType.flat)
    };

    //[Tooltip("Поглощение утраты баланса")]
    //public float balanceBlockFactor;


}
