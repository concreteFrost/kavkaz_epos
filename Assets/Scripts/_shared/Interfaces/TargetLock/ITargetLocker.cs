using UnityEngine;

public interface ITargetLocker
{

    void SetLockedTarget(IDamagable target);
    void ResetLockedTarget();
    //bool IsLockedOnTarget { get; }
}