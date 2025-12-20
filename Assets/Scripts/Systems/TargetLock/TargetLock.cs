using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class TargetLock : MonoBehaviour, ITargetLocker
{
    protected Transform targetSeeker;
    public ITargetLockable currentTarget;

    [SerializeField] protected float checkTargetRadius = 10f;
    [SerializeField] protected float targetResetDistance = 15f;

    protected bool wasTargetSearched = false;

    public bool IsLockedOnTarget { get => currentTarget != null; }

    public abstract void SetLockedTarget();

    protected virtual void CalculateDistanceToTarget()
    {
        if (!currentTarget.IsActive())
        {
            ResetLockTarget();
            return;
        }
        var dist = Vector3.Distance(targetSeeker.position, currentTarget.GetTargetTransform().position);

        if (dist > targetResetDistance)
        {
            ResetLockTarget();
        }
    }

    public virtual void ResetLockTarget()
    {
        currentTarget = null;
        wasTargetSearched = false;
    }

    public abstract ITargetLockable TryGetLockedTarget();

    protected ITargetLockable CheckNearestTarget()
    {
        var targets = Physics.OverlapSphere(targetSeeker.position, checkTargetRadius);

        if (targets.Length > 0)
        {
            return GetNearestTarget(targets);
        }

        return null;
    }

    protected ITargetLockable GetNearestTarget(Collider[] targets)
    {
        Dictionary<ITargetLockable, float> objectsDistances = new Dictionary<ITargetLockable, float>();

        foreach (var target in targets)
        {
            if (target.TryGetComponent<ITargetLockable>(out var lockable))
            {
                float distance = Vector3.Distance(targetSeeker.position, lockable.GetTargetTransform().position);

                if (distance < checkTargetRadius)
                    objectsDistances.Add(lockable, distance);
            }
        }

        if (objectsDistances.Count == 0) return null;

        var min = objectsDistances.OrderBy((x) => x.Value).FirstOrDefault().Key;
        return min;
    }



}
