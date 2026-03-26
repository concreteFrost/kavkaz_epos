using System.Collections;
using UnityEngine;

[System.Serializable]
public class CombatItemData
{
    public float[] initialPosition = new float[3];  
    public float[] currentPosition = new float[3];
    public float[] initialRotation = new float[3];
    public float[] currentRotation = new float[3];  
    public string itemSOid;
    public string itemInstanceId;
    public float breakdownThreshold;
    public string ownerId;
    public bool isStaticItem;
}

public abstract class CombatItem : Item, ICombatItem , IBreakable
{

    [SerializeField] protected CombatItemData data;
    protected Rigidbody rb;
    protected Collider physicsCollider;
    protected MeshRenderer meshRenderer;

    Coroutine breakCoroutine = null;

    #region ICombatItem Contract
    public ICollector Owner { get; set; } = null;

    #endregion

    #region IBreakable Contract
    public bool IsBreakdownEnabled { get; set; } = true;
    public bool IsBroken { get; set; } = false;
    public float GetDurability() => data.breakdownThreshold;
    public void SetBreakdownEnabled(bool isEnabled) => IsBreakdownEnabled = isEnabled;

    #endregion

    public override void Init()
    {
        base.Init();

        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>(); 

    }
    
    public void SetCombatItemData(CombatItemData data)=>this.data = data;   

    public CombatItemData SaveCombatItemData()
    {
       
        return new CombatItemData()
        {
            initialPosition = data.initialPosition,
            initialRotation = data.initialRotation,
            currentPosition = new float[3] { transform.position.x, transform.position.y, transform.position.z },
            currentRotation = new float[3] { transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z },
            itemSOid = data.itemSOid,
            itemInstanceId = data.itemInstanceId,
            ownerId = Owner != null ? Owner.CollectorId() : null,
            breakdownThreshold = data.breakdownThreshold,
            isStaticItem = data.isStaticItem,
        };
    }

    public void LoadData(CombatItemData loadedData)
    {
        this.data = loadedData;
        Drop();
        transform.position = new Vector3(data.currentPosition[0], data.currentPosition[1], data.currentPosition[2]);
        transform.eulerAngles = new Vector3(data.currentRotation[0], data.currentRotation[1], data.currentRotation[2]);

    }

    public abstract string GetDataId();

    protected void ToggleInteraction(bool canInteract)
    {
        if (canInteract)
        {
            interactionCollder.EnableCollider();
        }
        else
        {
            interactionCollder.DisableCollider();
        }

        rb.isKinematic = !canInteract;
        physicsCollider.enabled = canInteract;
    }

    protected void AssignParent(Transform t)
    {
        transform.SetParent(t);
        transform.position = t.position;
        transform.rotation = t.rotation;
    }

    protected void ResetParent()
    {
        transform.SetParent(null);
    }

    public void Recover()
    {
        if(breakCoroutine != null)
        {
            StopCoroutine(BreakCoroutine());
            breakCoroutine = null;  
        }

        transform.position = InitialPosition;
        meshRenderer.enabled = true;
        data.breakdownThreshold = 100f;

        ToggleInteraction(true);
    }

    public void ReduceDurability(float amount)
    {
        if (Owner == null || !IsBreakdownEnabled) return;

        data.breakdownThreshold -= amount;

        if (data.breakdownThreshold <= 0f)
        {
            Owner.CombatInventory.ResetCombatItem(this);
            Drop();
            StartBreakCoroutine();
        }
    }

    public void IncreaseDurability(float amount)
    {
        data.breakdownThreshold = Mathf.Clamp01(data.breakdownThreshold + amount);
    }

    public void Drop()
    {
        ResetParent();
        ResetOwner();
        ToggleInteraction(true);

    }

    public abstract void AssignToOwner(ICollector target);
   
    protected void ResetOwner()
    {
        if(Owner == null) return;   

        Owner.CombatInventory.ResetCombatItem(this);
        Owner = null;
        //damageCollider.SetDamageSource(null);
    }


    #region Break Methods

    protected void StartBreakCoroutine()
    {
        if(breakCoroutine == null)
        {
            breakCoroutine = StartCoroutine(BreakCoroutine());
        }
        
    }

    private IEnumerator BreakCoroutine()
    {
        yield return new WaitForSeconds(5);
        Break();
        yield return new WaitForSeconds(3);
        Recover();
        breakCoroutine = null;
    }

    public void Break()
    {
        ToggleInteraction(false);
        meshRenderer.enabled = false;
        IsBroken = true;

    }

    #endregion


}
