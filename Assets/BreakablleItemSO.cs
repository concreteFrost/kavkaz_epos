using UnityEngine;


public class BreakablleItemSO : ItemSO
{
    [Header("Поломка")]
    [Tooltip("Сколько снимать от состояния при ударе")]
    public float breakdownPenalty;
}
