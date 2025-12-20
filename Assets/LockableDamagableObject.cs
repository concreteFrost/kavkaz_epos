using UnityEngine;

public class LockableDamagableObject : DamagableObject, ITargetLockable
{
    bool isActive = true;
    public override void Die()
    {
        SetTargetActive(false);
        base.Die();
    }

    public Transform GetTargetTransform() => transform;

    public bool IsActive() => isActive;
   
    public void SetTargetActive(bool active)
    {
        isActive = active;
    }
}
