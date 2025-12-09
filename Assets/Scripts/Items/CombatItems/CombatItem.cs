using UnityEngine;

public class CombatItem : Item
{

    Rigidbody rb;
    Collider physicsCollider;

    public float breakdownThreshold;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicsCollider = GetComponent<Collider>();
    }

    public override void PickUp(IAttackSource s) // заглушка
    {
        throw new System.NotImplementedException();
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
