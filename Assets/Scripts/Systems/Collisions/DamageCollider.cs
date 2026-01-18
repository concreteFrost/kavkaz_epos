using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected Transform source;

    protected Collider damageCollider;

    protected readonly HashSet<Collider> hitColliders = new();

    [HideInInspector] public float healthDamage;
    [HideInInspector] public float balanceDamage;
    List<CharacterType> objectsToIgnore;

    [HideInInspector] public bool attackInterrupted;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        DisableCollider();
    }

    private void Update()
    {
        if (!damageCollider || !damageCollider.enabled)
            return;

        Vector3 size = damageCollider.bounds.size;
        float radius = Mathf.Max(size.x, size.y, size.z) * 0.5f;

        Collider[] hits = Physics.OverlapSphere(damageCollider.bounds.center, radius);

        foreach (var col in hits)
            HandleCollision(col);
    }

    public virtual void EnableCollider(float health, float balance, List<CharacterType> targetsToIgnore)
    {
        healthDamage = health;
        balanceDamage = balance;
        objectsToIgnore = targetsToIgnore;

        attackInterrupted = false;
        hitColliders.Clear();

        damageCollider.enabled = true;
    }

    public virtual void DisableCollider()
    {
        damageCollider.enabled = false;
        attackInterrupted = false;
        hitColliders.Clear();
        objectsToIgnore = null;
    }

    //protected virtual void OnTriggerEnter(Collider other)
    //{

    //    HandleCollision(other);
    //}



    protected virtual void HandleCollision(Collider other)
    {
        if (attackInterrupted)
            return;

        if (!TryGetDamagable(other, out var damagable))
            return;


        if (NotInTargetList(damagable))
            return;

        if (!hitColliders.Add(other))
            return;


        ApplyDamage(damagable);
    }


    // ---------- Damage ----------

    protected virtual void ApplyDamage(IDamagable target)
    {
        target.TakeDamage(healthDamage, balanceDamage, source);
    }

    // ---------- Defence ----------

    protected bool TryGetDamagable(Collider other, out IDamagable damagable)
    {
        damagable = other.GetComponentInChildren<IDamagable>()
                  ?? other.GetComponent<IDamagable>();

        return damagable != null;
    }

    protected bool NotInTargetList(IDamagable damagable)
    {
        if (objectsToIgnore == null) return false;
        if (objectsToIgnore.Count == 0) return false;
        return objectsToIgnore.Contains(damagable.CharacterType);

    }
}