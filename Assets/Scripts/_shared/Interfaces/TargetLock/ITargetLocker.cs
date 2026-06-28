using UnityEngine;

public interface ITargetLocker
{
    IDamagable CurrentTarget();

    void SetLockedTarget(IDamagable target);
    void ResetLockedTarget();
    //bool IsLockedOnTarget { get; }
}