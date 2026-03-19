using UnityEngine;


[CreateAssetMenu(fileName = "Shield", menuName =ScriptablePaths.ITEMS_PATH + "/Shields/Shield")]
public class ShieldSO : BreakablleItemSO
{

    [Space(10)]
    [Header("Защита")]
    [Tooltip("Степень защиты")]
    public float defenceBonus;

    //[Tooltip("Поглощение утраты баланса")]
    //public float balanceBlockFactor;


}
