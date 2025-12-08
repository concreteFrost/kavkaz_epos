using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider col;
    public List<Collider> collectedColliders = new List<Collider>();

    public bool attackInterrupted = false; // используется при обнаружении щита у цели
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
        attackInterrupted = false;
        collectedColliders.Clear();
    }

    protected bool TargetHasShield(Collider other)
    {
        if (other.GetComponent<DefenceCollider>() != null)
        {
            var defence = other.GetComponent<DefenceCollider>();

            defence.CalculateDamage(currentDamage);
            attackInterrupted = true;
            return true;
        }

        return false;
    }

    protected void PerformNormalDamage(Collider other)
    {
        var damagable = other.GetComponentInChildren<IDamagable>()
              ?? other.GetComponent<IDamagable>();

        if (damagable == null) return;

        damagable.TakeDamage(currentDamage);
    }

    protected virtual void HandleCollision(Collider other, float damage)
    {
        if (attackInterrupted)
            return;


        if (collectedColliders.Contains(other))
            return;

        collectedColliders.Add(other);


        if (TargetHasShield(other)) return;

        PerformNormalDamage(other);

    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other, currentDamage);
    }




}
