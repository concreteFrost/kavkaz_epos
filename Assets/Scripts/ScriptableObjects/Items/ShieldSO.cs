using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield", menuName =ScriptablePaths.ITEMS_PATH + "/Shields/Shield")]
public class ShieldSO : BreakablleItemSO
{

    [Space(10)]

    [Tooltip("Степень защиты в процентах")]
    [SerializeField] float defenceAmount;

    public float GetDefenceBonus() => defenceAmount;

    //[Tooltip("Поглощение утраты баланса")]
    //public float balanceBlockFactor;


}
