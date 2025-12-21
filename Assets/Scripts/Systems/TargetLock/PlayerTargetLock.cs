using UnityEngine;
using UnityEngine.UI;

public class PlayerTargetLock : TargetLock
{

    LockOnTargetUI lockOnTargetUI;    
   
    /// <summary>
    /// Ввод мыши по оси Х при котором цель сбрасывается
    /// </summary>
    [SerializeField] float targetSwitchThreshold = 45f;

    [SerializeField] private Image img;

    PlayerController controller;
    PlayerStatsModifier statsModifier;

    public void Init(PlayerTargetLockServices provider)
    {   
        this.lockOnTargetUI = provider.lockOnTargetUI;
        this.controller = provider.controller;
        this.targetSeeker = controller.transform;
        this.statsModifier = provider.statsModifier; 
    }

    private void Update()
    {
        if (currentTarget == null) return;
        if (statsModifier.IsDead())
        {
            ResetLockTarget();
            return;
        }
        TrackTargetDistance();
    }

    public void TrackTargetDistance()
    {
        lockOnTargetUI.CalculateImagePosition();
        CalculateDistanceToTarget();
    }

    public override void SetLockedTarget()
    {
        var t = TryGetLockedTarget();

        if (t != null)
        {
            lockOnTargetUI.SetTarget(t.GetTargetTransform());
            controller.SetLockTarget(t.GetTargetTransform());
        }
    }

    protected override void CalculateDistanceToTarget()
    {
        base.CalculateDistanceToTarget();
    }
    public override ITargetLockable TryGetLockedTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockTarget();
            lockOnTargetUI.ResetTarget();
            return null;
        }

        var nearest = CheckNearestTarget();

        if(nearest != null)
        {
            currentTarget = CheckNearestTarget();
            lockOnTargetUI.SetTarget(currentTarget.GetTargetTransform());

            return nearest;
        }

        return null;
    }

    public override void ResetLockTarget()
    {
       base.ResetLockTarget();
       lockOnTargetUI.ResetTarget();
       controller.ResetLockTarget();
    }


    public ITargetLockable SwitchTarget(float mouseX)
    {
        if (currentTarget == null) return null;
        if (Mathf.Abs(mouseX) < targetSwitchThreshold) return null;

        Camera cam = Camera.main;

        Vector3 currentScreen =
            cam.WorldToScreenPoint(currentTarget.GetTargetTransform().position);

        var colliders = Physics.OverlapSphere(targetSeeker.position, checkTargetRadius);

        ITargetLockable bestTarget = null;
        float bestDeltaX = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!col.TryGetComponent<ITargetLockable>(out var lockable))
                continue;

            ITargetLockable target = lockable;
            if (target == currentTarget) continue;

            Vector3 screenPos = cam.WorldToScreenPoint(target.GetTargetTransform().position);

            float deltaX = screenPos.x - currentScreen.x;

            // вправо
            if (mouseX > 0 && deltaX <= 0) continue;
            // влево
            if (mouseX < 0 && deltaX >= 0) continue;

            float absDelta = Mathf.Abs(deltaX);
            if (absDelta < bestDeltaX)
            {
                bestDeltaX = absDelta;
                bestTarget = target;
            }
        }

        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            //state.SetLockTarget(currentTarget);
            lockOnTargetUI.SetTarget(currentTarget.GetTargetTransform());
            controller.SetLockTarget(currentTarget.GetTargetTransform());
        }

        return currentTarget;
    }


}
