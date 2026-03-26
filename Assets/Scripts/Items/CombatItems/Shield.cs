using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    #region IShield Variables
    public bool IsProtectionActive { get; set; } = false;
    public ShieldSO ShieldData() => shieldSO;
    #endregion

    public override void Init()
    {

        base.Init();
        ToggleInteraction(true);

    }



    public override string GetDataId() => ShieldData().id;


    public void PerformDefence()
    {
        IsProtectionActive = true;
    }

    public void CancelDefence()
    {
        IsProtectionActive = false; 
    }

    public void ThrowShield()
    {
        Drop();
    }

    public override void Interact(ICollector collector)
    {
        if (collector.CombatInventory.ShieldWeapon != null) return;

        var currentWeaponData = collector.CombatInventory.CurrentWeapon.WeaponData();
        if (currentWeaponData.weaponType == WeaponType.TwoHands) return;

        if (GetDurability() <= 0)
        {
            Debug.Log("this shield is broken");
            return;
        }

        AssignToOwner(collector);

    }

    public override void AssignToOwner(ICollector collector)
    {
        Owner = collector;

        AssignParent(Owner.CombatInventory.GetLeftHand());
        ToggleInteraction(false);
        collector.CombatInventory.SetShield(this);

    }

}
