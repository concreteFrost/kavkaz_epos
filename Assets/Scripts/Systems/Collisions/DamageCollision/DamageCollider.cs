using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected Transform source;
    protected Collider damageCollider;

    protected readonly HashSet<Collider> hitColliders = new();

    DamageData damageData;
    List<CharacterType> objectsToIgnore;

    [HideInInspector] public bool attackInterrupted;

    private Vector3 lastPosition;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        lastPosition = transform.position;

        DisableCollider();
    }

    private void FixedUpdate()
    {
        if (!damageCollider.enabled || attackInterrupted) return;

        Vector3 moveDir = transform.position - lastPosition;
        float moveDist = moveDir.magnitude;

        if (moveDist > 0f)
        {
            Collider[] hits = SweepColliders(lastPosition, moveDir.normalized, moveDist);

            foreach (var col in hits)
            {
                HandleCollision(col);
                if (attackInterrupted) break; // удар прервался щитом или нанесен урон
            }
        }

        lastPosition = transform.position;
    }

    public virtual void EnableCollider(DamageData damageData,List<CharacterType> targetsToIgnore)
    {
        this.damageData = damageData;
        objectsToIgnore = targetsToIgnore;

        attackInterrupted = false;
        hitColliders.Clear();

        damageCollider.enabled = true;
        lastPosition = transform.position;
    }

    public virtual void DisableCollider()
    {
        damageCollider.enabled = false;
        attackInterrupted = false;
        hitColliders.Clear();
        objectsToIgnore = null;
    }

    protected virtual Collider[] SweepColliders(Vector3 origin, Vector3 direction, float distance)
    {
        if (damageCollider == null) return System.Array.Empty<Collider>();

        switch (damageCollider)
        {
            case BoxCollider box:
                Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);
                return Physics.BoxCastAll(origin + box.center, halfExtents, direction, transform.rotation, distance).ConvertHitsToColliders();

            case SphereCollider sphere:
                float radius = sphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                return Physics.SphereCastAll(origin + sphere.center, radius, direction, distance).ConvertHitsToColliders();

            case CapsuleCollider capsule:
                Vector3 up = Vector3.up;
                switch (capsule.direction)
                {
                    case 0: up = transform.right; break;
                    case 1: up = transform.up; break;
                    case 2: up = transform.forward; break;
                }
                Vector3 point1 = origin + capsule.center + up * (capsule.height / 2 - capsule.radius);
                Vector3 point2 = origin + capsule.center - up * (capsule.height / 2 - capsule.radius);
                float capRadius = capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                return Physics.CapsuleCastAll(point1, point2, capRadius, direction, distance).ConvertHitsToColliders();

            default:
                Debug.LogWarning("Unsupported collider type: " + damageCollider.GetType());
                return System.Array.Empty<Collider>();
        }
    }

    protected virtual void HandleCollision(Collider other)
    {
        if (attackInterrupted) return;

        var defence = other.GetComponent<DefenceCollider>();
        if (defence != null)
        {
            var owner = defence.Shield.Owner.Damagable;
            if (NotInTargetList(owner)) return;

            defence.ProcessDamage(damageData, source);
            attackInterrupted = true;
            return;
        }

        if (!TryGetDamagable(other, out var damagable)) return;
        if (NotInTargetList(damagable)) return;
        if (!hitColliders.Add(other)) return;

        ApplyDamage(damagable);
        //attackInterrupted = true;
    }

    protected virtual void ApplyDamage(IDamagable target)
    {
        target.TakeDamage(damageData, source);
    }

    protected bool TryGetDamagable(Collider other, out IDamagable damagable)
    {
        damagable = other.GetComponentInChildren<IDamagable>() ?? other.GetComponent<IDamagable>();
        return damagable != null;
    }

    protected bool NotInTargetList(IDamagable damagable)
    {
        if (objectsToIgnore == null || objectsToIgnore.Count == 0) return false;
        return objectsToIgnore.Contains(damagable.CharacterType);
    }
}

public static class ColliderExtensions
{
    public static Collider[] ConvertHitsToColliders(this RaycastHit[] hits)
    {
        var cols = new Collider[hits.Length];
        for (int i = 0; i < hits.Length; i++)
            cols[i] = hits[i].collider;
        return cols;
    }
}