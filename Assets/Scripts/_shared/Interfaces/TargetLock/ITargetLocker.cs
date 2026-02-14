using UnityEngine;

public interface ITargetLocker
{
    Transform CurrentTarget();
    void SetLockedTarget(IDamagable target);
    void ResetLockedTarget();
    //bool IsLockedOnTarget { get; }
}