using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargetLock : MonoBehaviour
{
    PlayerController playerMotor;
    Transform playerTransform;
    LockOnTargetUI lockOnTargetUI;  
    Transform currentTarget;

    [SerializeField] float targetSwitchThreshold = 45f;

    [SerializeField] float checkTargetRadius = 10f;
    [SerializeField] float targetResetDistance = 15f;

    bool wasTargetSearched = false;

    [SerializeField] private Image img;

    private void Update()
    {
        
        if (currentTarget == null) return;

        lockOnTargetUI.CalculateImagePosition();

        CalculateDistanceToTarget();

    }

    public void Init(PlayerController _motor, LockOnTargetUI _lockOnTargetUI)
    {
        playerMotor = _motor;
        playerTransform = playerMotor.transform;    
        lockOnTargetUI = _lockOnTargetUI;   
    }


    private void CalculateDistanceToTarget()
    {
        var dist = Vector3.Distance(playerTransform.position, currentTarget.position);

        if (dist > targetResetDistance)
        {
            ResetLockTarget();
        }
    }
    public void SetLockTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockTarget();
            lockOnTargetUI.ResetTarget();   
            return;
        }

        currentTarget = CheckNearestTarget();

        if (currentTarget != null)
        {
            playerMotor.SetLockTarget(currentTarget);
            lockOnTargetUI.SetTarget(currentTarget);           
        }
    }

    private void ResetLockTarget()
    {
        currentTarget = null;
        playerMotor.ResetLockTarget();
        lockOnTargetUI.ResetTarget();
        wasTargetSearched = false;
    }

    private Transform CheckNearestTarget()
    {
        var targets = Physics.OverlapSphere(playerTransform.position, checkTargetRadius);

        if (targets.Length > 0)
        {    
            return GetNearestTarget(targets);
        }

        return null;
    }

    private Transform GetNearestTarget(Collider[] targets)
    {
        Dictionary<ITargetLockable, float> objectsDistances = new Dictionary<ITargetLockable, float>();

        foreach (var target in targets)
        {
            if (target.TryGetComponent<ITargetLockable>(out var lockable))
            {
                float distance = Vector3.Distance(playerTransform.position, lockable.GetTargetTransform().position);

                if (distance < checkTargetRadius)
                    objectsDistances.Add(lockable, distance);
            }
        }

        if (objectsDistances.Count == 0) return null;

        var min = objectsDistances.OrderBy((x) => x.Value).FirstOrDefault().Key;
        return min.GetTargetTransform();
    }

    public void SwitchTarget(float mouseX)
    {
        if (currentTarget == null) return;
        if (Mathf.Abs(mouseX) < targetSwitchThreshold) return;

        Camera cam = Camera.main;

        Vector3 currentScreen =
            cam.WorldToScreenPoint(currentTarget.position);

        var colliders = Physics.OverlapSphere(playerTransform.position, checkTargetRadius);

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
            playerMotor.SetLockTarget(currentTarget);
            lockOnTargetUI.SetTarget(currentTarget);
        }
    }


}
