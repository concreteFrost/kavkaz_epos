using UnityEngine;

public class Shield : CombatItem, IShield, IBreakable
{
    public ShieldSO shieldSO;

    #region IShield Variables
    public bool IsProtectionActive { get; set; } = false;
    public ICollector Owner { get; set; }
    public ShieldSO ShieldData() => shieldSO;
    #endregion

    #region IBreakable Contract
    public float GetDurability() => breakdownThreshold;
    #endregion

    public override void Init(ItemSO itemData)
    {

        base.Init(itemData);
        ToggleInteraction(true);

    }

    public void PerformDefence()
    {
        //defenceCollider.EnableCollider();
        IsProtectionActive = true;
        //Owner.Damagable.IsDefended = true;
        //Owner.Damagable.DefenceBonus = shieldSO.defenceBonus;

    }

    public void CancelDefence()
    {
        //defenceCollider.DisableCollider();
        IsProtectionActive = false; 
        //Owner.Damagable.IsDefended = false;
        //Owner.Damagable.DefenceBonus = 0;
    }

    public override void PickUp(ICollector collector)
    {
        if (collector.CombatInventory.ShieldWeapon != null) return;

        var currentWeaponData = collector.CombatInventory.CurrentWeapon.WeaponData();
        if (currentWeaponData.weaponType == WeaponType.TwoHands) return;

        if (breakdownThreshold <= 0)
        {
            Debug.Log("this shield is broken");
            return;
        }

        AssignToOwner(collector);

    }

    public void AssignToOwner(ICollector collector)
    {
        Owner = collector;

        AssignParent(Owner.CombatInventory.GetLeftHand());
        ToggleInteraction(false);
        collector.CombatInventory.SetShield(this);

    }

    public void ReduceDurability()
    {
        if (Owner == null) return;
        //if (Owner.CanPreventWeaponDamage()) return;

        breakdownThreshold -= shieldSO.breakdownPenalty;

        if (breakdownThreshold <= 0)
        {

            ThrowShield();
        }
    }

    public void ThrowShield()
    {
        ResetParent();
        ToggleInteraction(true);
       
        Owner.CombatInventory.ResetShield();
        Owner = null;


    }
}
