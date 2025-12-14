using UnityEngine;

public class PlayerTargetLock : MonoBehaviour
{
    PlayerMotor playerMotor;
    Transform currentTarget;

    [SerializeField] float checkTargetRadius = 10f;
    [SerializeField] float targetResetDistance = 15f;

    bool wasTargetSearched = false;

    private void Update()
    {

        if (currentTarget == null) return;

        CalculateDistanceToTarget();

    }

    public void Init(PlayerMotor motor)
    {
        playerMotor = motor;
    }


    private void CalculateDistanceToTarget()
    {
        var dist = Vector3.Distance(transform.position, currentTarget.position);


        if (dist > targetResetDistance)
        {
            ResetLockTarget();
            Debug.Log("Target Lock Reset due to distance");
        }
    }
    public void SetLockTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockTarget();
            return;
        }

        currentTarget = CheckNearestTarget();

        if (currentTarget != null)
        {
            //playerCamera.LockOnTarget(currentTarget);
            playerMotor.SetLockTarget(currentTarget);
        }
    }



    private void ResetLockTarget()
    {

        currentTarget = null;
        //playerCamera.UnlockTarget();
        playerMotor.ResetLockTarget();
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
