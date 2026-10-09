using UnityEngine;


public abstract class CombatItemSO : ItemSO
{

    [Tooltip("—колько снимать от состо€ни€ при ударе (в единицах)")]

    [SerializeField] float brakdownPenalty;

    public float GetBreakdownPenalty() => brakdownPenalty;

    public override bool CanUse() => false;
    public override bool CanEquip() => true;
    public override bool CanAddToSlot() => false;
    public override bool CanRemoveFromSlot() => false;
    public override bool CanDestroy() => true;
   
}
