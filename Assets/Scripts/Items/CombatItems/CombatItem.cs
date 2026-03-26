using UnityEngine;


public abstract class CombatItem : MonoBehaviour, ICombatItem , IBreakable
{

    protected ItemData data;
    protected Collider physicsCollider;
    protected MeshRenderer meshRenderer;


    #region ICombatItem Contract
    public ICollector Owner { get; set; } = null;

    #endregion

    #region IBreakable Contract
    public bool IsBreakdownEnabled { get; set; } = true;
    public bool IsBroken { get; set; } = false;
    public float GetDurability() => data.durability;
    public void SetBreakdownEnabled(bool isEnabled) => IsBreakdownEnabled = isEnabled;

    #endregion

    public virtual void Init(ItemData data)
    {
        this.data = data;
        physicsCollider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    
    }

    public void ToggleVisibility(bool enabled)
    {
        meshRenderer.enabled = enabled;
       
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


    public void ReduceDurability(float amount)
    {
        if (Owner == null || !IsBreakdownEnabled) return;

        data.durability -= amount;

        if (data.durability <= 0f)
        {
            Debug.Log("weapon is broken");
        }
    }

    public void IncreaseDurability(float amount)
    {
        data.durability = Mathf.Clamp01(data.durability + amount);
    }

    public abstract void AssignToOwner(ICollector target);
   
    protected void ResetOwner()
    {
        if(Owner == null) return;   

        //Owner.CombatInventory.ResetCombatItem(this);
        Owner = null;
        //damageCollider.SetDamageSource(null);
    }




}
