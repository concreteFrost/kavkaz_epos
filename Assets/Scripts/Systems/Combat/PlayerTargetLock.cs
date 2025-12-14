using UnityEngine;
using UnityEngine.UI;

public class PlayerTargetLock : MonoBehaviour
{
    PlayerMotor playerMotor;
    LockOnTargetUI lockOnTargetUI;  
    Transform currentTarget;

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

    public void Init(PlayerMotor _motor, LockOnTargetUI _lockOnTargetUI)
    {
        playerMotor = _motor;
        lockOnTargetUI = _lockOnTargetUI;   
    }


    private void CalculateDistanceToTarget()
    {
        var dist = Vector3.Distance(transform.position, currentTarget.position);


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
        var targets = Physics.OverlapSphere(transform.position, checkTargetRadius);

        if (targets.Length > 0)
        {
            float nearestDistance = Mathf.Infinity;

            foreach (var target in targets)
            {
                if (target.TryGetComponent<ITargetLockable>(out var lockable))
                {
                    float distance = Vector3.Distance(transform.position, lockable.GetTargetTransform().position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        currentTarget = lockable.GetTargetTransform();
                        return currentTarget;
                    }
                }
            }

        }

        return null;
    }
}
