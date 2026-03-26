using UnityEngine;

[System.Serializable]
public class CombatItemData
{
    public float breakdownThreshold;
}

public abstract class CombatItem : Item, ICombatItem , IBreakable
{

    [SerializeField] protected CombatItemData data;
    protected Collider physicsCollider;
    protected MeshRenderer meshRenderer;


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

        physicsCollider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        data.breakdownThreshold = 100f;

    }
    
    public CombatItemData SaveCombatItemData()
    { 
        return new CombatItemData()
        {       
            breakdownThreshold = data.breakdownThreshold, 
        };
    }

    public void LoadData(CombatItemData loadedData)
    {
         data.breakdownThreshold = loadedData.breakdownThreshold;   

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

        data.breakdownThreshold -= amount;

        if (data.breakdownThreshold <= 0f)
        {
            Debug.Log("weapon is broken");
        }
    }

    public void IncreaseDurability(float amount)
    {
        data.breakdownThreshold = Mathf.Clamp01(data.breakdownThreshold + amount);
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
