using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    #region IShield Variables
    public bool IsProtectionActive { get; set; } = false;
    public ShieldSO ShieldData() => shieldSO;

    public override void Init(CombatItemData data)
    {
        //if (data is not ShieldData) throw new System.ArgumentException("Shield requires ShieldData", nameof(data));
        base.Init(data);
    }
    #endregion

    public void PerformDefence()
    {
        IsProtectionActive = true;
    }

    public void CancelDefence()
    {
        IsProtectionActive = false; 
    }


    public override void AssignToOwner(IInteractor collector)
    {
        Owner = collector;
        AssignParent(Owner.WeaponSetter.GetLeftHand());

    }

}
