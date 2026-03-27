using UnityEngine;


public abstract class CombatItemSO : ItemSO
{

    [Tooltip("—колько снимать от состо€ни€ при ударе (в единицах)")]

    [SerializeField] float brakdownPenalty;

    public float GetBreakdownPenalty() => brakdownPenalty;
}
