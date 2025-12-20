using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class TargetLock : MonoBehaviour
{
    /// <summary>
    /// —брасывает цель на рассто€нии или при потери цели
    /// </summary>
    public static Action<Transform, Transform> OnTargetLost;
    protected Transform targetSeeker;
    public Transform currentTarget;

    [SerializeField] protected float checkTargetRadius = 10f;
    [SerializeField] protected float targetResetDistance = 15f;

    protected bool wasTargetSearched = false;

    protected virtual void CalculateDistanceToTarget()
    {
        var dist = Vector3.Distance(targetSeeker.position, currentTarget.position);

        if (dist > targetResetDistance)
        {
            OnTargetLost?.Invoke(currentTarget, targetSeeker);
            ResetLockTarget();
        }
    }

    public virtual void ResetLockTarget()
    {
        currentTarget = null;
        wasTargetSearched = false;
    }

    public abstract Transform GetLockedTarget();

    protected Transform CheckNearestTarget()
    {
        var targets = Physics.OverlapSphere(targetSeeker.position, checkTargetRadius);

        if (targets.Length > 0)
        {
            return GetNearestTarget(targets);
        }

        return null;
    }

    protected Transform GetNearestTarget(Collider[] targets)
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
        return min.GetTargetTransform();
    }

}
