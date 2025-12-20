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

    private void Update()
    {
        if (currentTarget == null) return;

        lockOnTargetUI.CalculateImagePosition();

        CalculateDistanceToTarget();

    }

    public void Init(LockOnTargetUI _lockOnTargetUI, Transform transform)
    {
        targetSeeker = transform;
        lockOnTargetUI = _lockOnTargetUI;
    }


    protected override void CalculateDistanceToTarget()
    {
        base.CalculateDistanceToTarget();
    }
    public override Transform GetLockedTarget()
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
            lockOnTargetUI.SetTarget(currentTarget);

            return nearest;
        }

        return null;
    }

    public override void ResetLockTarget()
    {
       base.ResetLockTarget();
       lockOnTargetUI.ResetTarget();
    }


    public Transform SwitchTarget(float mouseX)
    {
        if (currentTarget == null) return null;
        if (Mathf.Abs(mouseX) < targetSwitchThreshold) return null;

        Camera cam = Camera.main;

        Vector3 currentScreen =
            cam.WorldToScreenPoint(currentTarget.position);

        var colliders = Physics.OverlapSphere(targetSeeker.position, checkTargetRadius);

        Transform bestTarget = null;
        float bestDeltaX = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!col.TryGetComponent<ITargetLockable>(out var lockable))
                continue;

            Transform target = lockable.GetTargetTransform();
            if (target == currentTarget) continue;

            Vector3 screenPos = cam.WorldToScreenPoint(target.position);

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
            lockOnTargetUI.SetTarget(currentTarget);
        }

        return currentTarget;
    }


}
