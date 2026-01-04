using UnityEngine;

public class CharacterTargetLock : TargetLock
{

    IDamagable damageController;
    HumanoidAIController controller;

    public void Init(CharacterTargetLockService service)
    {
        this.damageController = service.damageController;
        this.controller = service.controller;
        this.targetSeeker = controller.transform;

       
    }

    private void Update()
    {
        if (currentTarget == null) return;
        if (damageController.IsDead())
        {
            ResetLockTarget();
            return;
        }
        TrackTargetDistance();
    }

    public void TrackTargetDistance()
    {
       
        CalculateDistanceToTarget();
    }

    public override void SetLockedTarget()
    {
        var t = TryGetLockedTarget();

        if (t != null)
        {
            Debug.Log(t.GetTargetTransform().name);
            controller.SetLockTarget(t.GetTargetTransform());
        }
    }

    public override ITargetLockable TryGetLockedTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockTarget();
            
            return null;
        }

        var nearest = CheckNearestTarget();

        if (nearest != null)
        {
            currentTarget = CheckNearestTarget();
            return nearest;
        }

        return null;
    }

    public override void ResetLockTarget()
    {
        base.ResetLockTarget();
        controller.ResetLockTarget();
    }


}
