using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    [SerializeField] private DefenceCollider defenceCollider;

    #region IShield Variables
    public ICollector Owner { get; set; }
    public ShieldSO ShieldData() => shieldSO;
    #endregion

    public override void Init(ItemSO itemData)
    {

        base.Init(itemData);
        ToggleInteraction(true);

        defenceCollider.Init();
        defenceCollider.SetShieldData(this);
        defenceCollider.DisableCollider();


    }

    public void PerformDefence()
    {
        defenceCollider.EnableCollider();

    }

    public void CancelDefence()
    {
        defenceCollider.DisableCollider();
    }

    public override void PickUp(ICollector collector)
    {
        if (collector.CombatInventory.ShieldWeapon != null) return;

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

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;

        if (breakdownThreshold <= 0)
        {

            ThrowShield();
        }
    }

    public void ThrowShield()
    {
        ResetParent();
        ToggleInteraction(true);
        defenceCollider.DisableCollider();

        Owner.CombatInventory.ResetShield();
        Owner = null;


    }
}
