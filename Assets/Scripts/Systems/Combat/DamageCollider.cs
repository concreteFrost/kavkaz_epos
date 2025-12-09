using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider col;
    public List<Collider> collectedColliders = new List<Collider>();

    public bool attackInterrupted = false; // используется при обнаружении щита у цели
    protected float healthDamage;
    protected float balanceDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void EnableCollider(float _healthDamage, float _balanceDamage)
    {
        col.enabled = true;
        healthDamage = _healthDamage;
        balanceDamage = _balanceDamage;
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

            defence.CalculateDamage(healthDamage,balanceDamage);
            return true;
        }

        return false;
    }

    protected void PerformNormalDamage(Collider other)
    {
        var damagable = other.GetComponentInChildren<IDamagable>()
              ?? other.GetComponent<IDamagable>();

        if (damagable == null) return;

        damagable.TakeDamage(healthDamage,balanceDamage);
    }

    protected virtual void HandleCollision(Collider other)
    {
        if (attackInterrupted)
            return;


        if (collectedColliders.Contains(other))
            return;

        collectedColliders.Add(other);

        if (TargetHasShield(other))
        {
            attackInterrupted = true;
            return;
        }

        PerformNormalDamage(other);

    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }




}
