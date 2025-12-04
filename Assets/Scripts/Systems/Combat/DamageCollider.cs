using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider col;    
    public List<Collider> collectedColliders = new List<Collider>();

    protected float currentDamage;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void EnableCollider(float _currDamage)
    {  
        col.enabled = true;
        currentDamage = _currDamage;    
    }

    public void DisableCollider()
    {
        col.enabled = false;
        collectedColliders.Clear();
    }

    protected virtual void HandleCollision(Collider other)
    {
        if (collectedColliders.Contains(other))
            return;

        collectedColliders.Add(other);

        var damagable = other.GetComponentInChildren<IDamagable>()
                ?? other.GetComponent<IDamagable>();

        if (damagable == null) return;

        damagable.TakeDamage(currentDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }




}
