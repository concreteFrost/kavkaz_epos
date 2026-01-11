using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected Transform source;

    protected Collider damageCollider;

    protected readonly HashSet<Collider> hitColliders = new();

    protected float healthDamage;
    protected float balanceDamage;
    List<CharacterType> objectsToIgnore;

    protected bool attackInterrupted;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        DisableCollider();
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

    protected virtual void OnTriggerEnter(Collider other)
    {
       
        HandleCollision(other);
    }

    protected virtual void HandleCollision(Collider other)
    {
        if (attackInterrupted)
            return;

        if (!TryGetDamagable(other, out var damagable))
            return;

        // ХОЗЯИН — полностью игнорируется
        if (NotInTargetList(damagable))
            return;

        // Повторное попадание
        if (!hitColliders.Add(other))
            return;

        // Защита (щит)
        if (TryHandleDefence(other))
            return;

        ApplyDamage(damagable);
    }


    // ---------- Damage ----------

    protected virtual void ApplyDamage(IDamagable target)
    {
        target.TakeDamage(healthDamage, balanceDamage,source);
    }

    // ---------- Defence ----------

    protected bool TryHandleDefence(Collider other)
    {
        if (!other.TryGetComponent(out DefenceCollider defence))
            return false;

        defence.CalculateDamage(healthDamage, balanceDamage);
        attackInterrupted = true;
        return true;
    }

    // ---------- Utils ----------

    protected bool TryGetDamagable(Collider other, out IDamagable damagable)
    {
        damagable = other.GetComponentInChildren<IDamagable>()
                  ?? other.GetComponent<IDamagable>();

        return damagable != null;
    }

    protected bool NotInTargetList(IDamagable damagable)
    {
        if(objectsToIgnore == null) return false;
        if(objectsToIgnore.Count == 0) return false;    
        return objectsToIgnore.Contains(damagable.CharacterType);

    }
}
