using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    [SerializeField] private DefenceCollider defenceCollider;
    public IAttackSource AttackSource { get; set; } = null;

    #region IShield Variables
    public ShieldSO ShieldData()=>shieldSO;

    #endregion

    void Start()
    {
        Init(shieldSO);
    }

    protected override void Init(ItemSO itemData)
    {

        base.Init( itemData);
        ToggleInteraction(true);

        base.breakdownThreshold = 100f;
       
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

    public override void PickUp(IAttackSource s)
    {

        if(breakdownThreshold <= 0)
        {
            Debug.Log("this shield is broken");
            return;
        }

        AttackSource = s;   

        AssignParent(AttackSource.GetLeftHand());   

        defenceCollider.SetOwner(AttackSource.Damagable);

        ToggleInteraction(false);
        s.SetShield(this);

    }

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;

        if (breakdownThreshold <= 0)
        {
            AttackSource.ResetShield(); 
            ThrowShield();
        }
    }

    public void ThrowShield()
    {
        ResetParent();
        ToggleInteraction(true);

        defenceCollider.ResetOwner();
        AttackSource.ResetShield();
        AttackSource = null;
    }
}
