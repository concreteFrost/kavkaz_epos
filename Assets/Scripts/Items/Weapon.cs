using UnityEngine;

public class Weapon : Item, IWeapon
{

    public WeaponSO weaponSO;

    Rigidbody rb;
    private Collider physicsCollider;
    [SerializeField] private WeaponDamageCollider damageCollider;

    public float damageAmount;
    public float breakdownThreshold;

    #region IWeapon variables
    public WeaponSO WeaponData() => weaponSO;

    public IAttackSource AttackSource { get;  set; }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>(); 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(weaponSO);
    }

    protected override void Init(ItemSO itemData)
    {

        base.Init(itemData);

        rb.isKinematic = false;
        physicsCollider.enabled = true;
        damageAmount = weaponSO.damageAmount;
        breakdownThreshold = weaponSO.breakdownThreshold;  
        damageCollider.DisableCollider();
        damageCollider.SetWeapon(this);
     
    }

    public void PerformAttack()
    {
        damageCollider.EnableCollider(damageAmount);
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }
    public override void PickUp(IAttackSource target)
    {

        if(breakdownThreshold <= 0)
        {
            Debug.Log("this weapon is broken");
            return;
        }


        if (!target.CurrentWeapon.WeaponData().canOverride)
            return;

        AttackSource = target;
 
        transform.SetParent(AttackSource.GetRightHand());
        transform.position = AttackSource.GetRightHand().position;  
        transform.rotation = AttackSource.GetRightHand().rotation; 
       
        interactionCollder.DisableCollider();
        rb.isKinematic = true;  
        physicsCollider.enabled = false;

        target.SetWeapon(this); 

    }

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;   

        if(breakdownThreshold <= 0)
        {
            AttackSource.ResetWeapon();
            ThrowWeapon();    
        }
    }

    public void ThrowWeapon()
    {
       
        transform.SetParent(null);  
        damageCollider.DisableCollider();
        interactionCollder.EnableCollider();
        rb.isKinematic = false;
        physicsCollider.enabled = true;

        AttackSource = null;

    }


}
