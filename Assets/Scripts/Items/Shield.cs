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

    public IAttackSource AttackSource { get; set; } = null;

    #region IShield Variables
    public ShieldSO ShieldData()=>shieldSO;

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
        Init(shieldSO);
    }

    protected override void Init( ItemSO itemData)
    {

        base.Init( itemData);

        rb.isKinematic = false;
        physicsCollider.enabled = true;
       
        breakdownThreshold =shieldSO.breakdownThreshold;
        defenceBonus = shieldSO.defenceBonus;   

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

        AttackSource = s;   

        parent = AttackSource.GetLeftHand();
        transform.SetParent(parent);
        transform.position = AttackSource.GetLeftHand().position;
        transform.rotation = AttackSource.GetLeftHand().rotation;

        defenceCollider.SetOwner(AttackSource);
      
        interactionCollder.DisableCollider();
        rb.isKinematic = true;
        physicsCollider.enabled = false;

        s.SetShield(this);

    }

    public void ThrowShield()
    {
        parent = null;
        transform.SetParent(parent);
        defenceCollider.ResetOwner();
        interactionCollder.EnableCollider();
        rb.isKinematic = false;
        physicsCollider.enabled = true;

        AttackSource = null;
    }
}
