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
    public string InstanceID() => data.instanceId;
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

    public void SetEquiped(bool equiped)
    {
        meshRenderer.enabled = equiped;
        data.isEquiped = equiped;   
    }

    protected void AssignParent(Transform t)
    {
        transform.SetParent(t);
        transform.position = t.position;
        transform.rotation = t.rotation;
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

   
   


}
