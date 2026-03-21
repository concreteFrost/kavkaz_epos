using System.Collections;
using UnityEngine;

public abstract class CombatItem : Item, ICombatItem , IBreakable
{

    protected Rigidbody rb;
    protected Collider physicsCollider;
    protected MeshRenderer meshRenderer;
    public float breakdownThreshold;

    Coroutine breakCoroutine = null;

    #region ICombatItem Contract
    public ICollector Owner { get; set; } = null;

    #endregion

    #region IBreakable Contract
    public bool IsBreakdownEnabled { get; set; } = true;
    public bool IsBroken { get; set; } = false;
    public float GetDurability() => breakdownThreshold;
    public void SetBreakdownEnabled(bool isEnabled) => IsBreakdownEnabled = isEnabled;

    #endregion

    public override void Init(ItemSO itemData)
    {
        base.Init(itemData);

        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        breakdownThreshold = 100f;    

    }

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
        breakdownThreshold = 100f;

        ToggleInteraction(true);
    }

    public void ReduceDurability(float amount)
    {
        if (Owner == null || !IsBreakdownEnabled) return;

        breakdownThreshold -= amount;

        if (breakdownThreshold <= 0f)
        {
            Owner.CombatInventory.ResetCombatItem(this);
            Drop();
            StartBreakCoroutine();
        }
    }

    public void IncreaseDurability(float amount)
    {
        breakdownThreshold = Mathf.Clamp01(breakdownThreshold + amount);
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
