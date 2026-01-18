using UnityEngine;

public class Shield : CombatItem, IShield
{
    public ShieldSO shieldSO;

    [SerializeField] private DefenceCollider defenceCollider;

    #region IShield Variables
    public ICollector Owner { get;  set; }   
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

        //Debug.Log("on defence");
    }

    public void CancelDefence()
    {
        defenceCollider.DisableCollider();
 
    }

    public override void PickUp(ICollector collector)
    {

        if(breakdownThreshold <= 0)
        {
            Debug.Log("this shield is broken");
            return;
        }

        Owner = collector;

        AssignParent(Owner.AttackSource.GetLeftHand());   
        ToggleInteraction(false);
        collector.AttackSource.SetShield(this);

       

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

        Owner.AttackSource.ResetShield();
        Owner = null;


    }
}
