using NUnit.Framework.Interfaces;
using UnityEngine;

public abstract class CombatItem : Item, IPickable
{

    protected Rigidbody rb;
    protected Collider physicsCollider;

    public float breakdownThreshold;


    public abstract void PickUp(ICollector interractor);
   

    public override void Init(ItemSO itemData)
    {
       base.Init(itemData);

        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>();
        breakdownThreshold = 100;

     
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

}
