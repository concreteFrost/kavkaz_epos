using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    #region IShield Variables
    public bool IsProtectionActive { get; set; } = false;
    public ShieldSO ShieldData() => shieldSO;
    #endregion

    public void PerformDefence()
    {
        IsProtectionActive = true;
    }

    public void CancelDefence()
    {
        IsProtectionActive = false; 
    }


    public override void AssignToOwner(ICollector collector)
    {
        Owner = collector;
        AssignParent(Owner.CombatInventory.GetLeftHand());

    }

}
