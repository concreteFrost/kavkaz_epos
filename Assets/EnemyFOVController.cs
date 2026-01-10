
using UnityEngine;

public class EnemyFOVController : MonoBehaviour, ITargetLocker
{
    [SerializeField] FovDataSO fovDataSO;
    [SerializeField] Transform eyes;

    HumanoidAIMotor motor;
    AIFov fov;
    
    private bool isLockedOnTarget = false;  
    public bool IsLockedOnTarget { get => isLockedOnTarget; set=>isLockedOnTarget = value; }

    public IDamagable currentTarget;


    private void Update()
    {
        if(currentTarget != null)
        {
            if (currentTarget.IsDead())
            {
                ResetTarget();
            }
        }
    }

    public void Init(HumanoidAIMotor motor)
    {
        this.motor = motor;
        fov = new AIFov(eyes, fovDataSO.objectsToScan, fovDataSO.obstacleMask);
    }

    public void CheckTargets()
    {

        var potentialTarget = fov.PotentialTarget(fovDataSO.viewRadius, fovDataSO.viewAngle);

        if (potentialTarget == null)
        {
            ResetTarget();
            return;
        }

        SetTarget(potentialTarget);
    }

    public void SetTarget(IDamagable target)
    {
        currentTarget = target;
        //motor.SetLockTarget(target.GetAimTransform());
        //motor.SetLockTarget(currentTarget.GetAimTransform());
        //isLockedOnTarget = true;
    }

    public void AssignTargetToMotor()
    {
        if (currentTarget == null) return;
        motor.SetLockTarget(currentTarget.GetAimTransform());
    }

    public void ResetTarget()
    {
        currentTarget = null;
        motor.ResetLockTarget();    

    }

    public bool IsTargetVisible(Transform target)
    {
        return fov.IsTargetVisible(target, fovDataSO.viewRadius, fovDataSO.viewAngle);
    }

    private void OnDrawGizmosSelected()
    {
        if (eyes == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyes.position, fovDataSO.viewRadius);

        Vector3 forward = eyes.forward;
        float halfAngle = fovDataSO.viewAngle / 2;
        Quaternion leftRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfAngle, 0);

        Vector3 leftBoundary = leftRotation * forward * fovDataSO.viewRadius;
        Vector3 rightBoundary = rightRotation * forward * fovDataSO.viewRadius;

        Gizmos.DrawRay(eyes.position, leftBoundary);
        Gizmos.DrawRay(eyes.position, rightBoundary);

        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(eyes.position, currentTarget.GetAimTransform().position);
        }
    }
}
