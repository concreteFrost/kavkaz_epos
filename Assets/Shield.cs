using UnityEngine;

public class Shield : Item, IShield
{
    public ShieldSO shieldSO;

    Rigidbody rb;
    private Collider physicsCollider;
    [SerializeField] private DefenceCollider defenceCollider;
    Transform parent;

    public float breakdownThreshold;
    public float defenceBonus;
    protected string owner { get; set; }

    #region IShield Variables
    public ShieldSO ShieldData()=>shieldSO;
    public string Owner { get => owner; set => owner = value; }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(this.gameObject, shieldSO);
    }

    protected override void Init(GameObject instance, ItemSO itemData)
    {

        base.Init(instance, itemData);

        rb.isKinematic = false;
        physicsCollider.enabled = true;
       
        breakdownThreshold =shieldSO.breakdownThreshold;
        defenceBonus = shieldSO.defenceBonus;   

        defenceCollider.DisableCollider();  
    }

    public void PerformDefence()
    {
        physicsCollider.enabled = true;
    }

    public void CancelDefence()
    {
        physicsCollider.enabled = false;
    }

    public override void PickUp(Transform target)
    {

        parent = target;
        transform.SetParent(parent);
        transform.position = target.position;
        transform.rotation = target.rotation;
        owner = target.name;    //replace with actual id
        defenceCollider.SetOwner(owner);
      
        interactionCollder.DisableCollider();
        rb.isKinematic = true;
        physicsCollider.enabled = false;

    }

    public void ThrowShield()
    {
        parent = null;
        transform.SetParent(parent);
        owner = null;
        defenceCollider.ResetOwner();
        interactionCollder.EnableCollider();
        rb.isKinematic = false;
        physicsCollider.enabled = true;
    }
}
