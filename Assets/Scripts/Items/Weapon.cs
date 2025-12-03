using UnityEngine;

public class Weapon : Item, IWeapon
{

    public WeaponSO weaponSO;

    Rigidbody rb;
    private Collider physicsCollider;
    [SerializeField] private DamageCollider damageCollider;
    Transform parent;

    public float damageAmount;
    public float breakdownThreshold;
    protected string owner { get; set; }

    #region IWeapon variables
    public WeaponSO WeaponData() => weaponSO;

    public string Owner { get => owner; set => owner = value; }

    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>(); 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(this.gameObject, weaponSO);
    }

    protected override void Init(GameObject instance, ItemSO itemData)
    {

        base.Init(instance,itemData);

        rb.isKinematic = false;
        physicsCollider.enabled = true;
        damageAmount = weaponSO.damageAmount;
        breakdownThreshold = weaponSO.breakdownThreshold;  
        damageCollider.DisableCollider();
     
    }

    public void PerformAttack()
    {
        damageCollider.EnableCollider();
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }
    public override void PickUp(Transform target)
    {
       
        parent = target;
        transform.SetParent(parent);
        transform.position = target.position;  
        transform.rotation = target.rotation; 
        owner = target.name;    //replace with actual id
        damageCollider.SetOwner(owner);
        
        interactionCollder.DisableCollider();
        rb.isKinematic = true;  
        physicsCollider.enabled = false;

    }

    public void ThrowWeapon()
    {
        parent = null;
        transform.SetParent(parent);
        owner = null;   
        damageCollider.ResetOwner();    
        interactionCollder.EnableCollider();
        rb.isKinematic = false;
        physicsCollider.enabled = true;

    }


}
