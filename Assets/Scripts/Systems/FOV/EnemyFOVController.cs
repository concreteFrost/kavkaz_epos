
using System.Collections;
using UnityEngine;

public class EnemyFOVController : MonoBehaviour, ITargetLocker
{
    [SerializeField] FovDataSO fovDataSO;
    [SerializeField] Transform eyes;

    AIFov fov;
    

    public IDamagable currentTarget;

    public float checkCooldown = 0f;
    private float maxCheckCooldown = 2f;


    private void Update()
    {
       
        if(currentTarget != null)
        {
            if (currentTarget.IsDead)
            {
                ResetLockedTarget();
            }
        }
    }

    public void Init()
    {
        fov = new AIFov(eyes, fovDataSO.objectsToScan, fovDataSO.obstacleMask);
        checkCooldown = 0;
    }

    public void CheckTargets()
    {
        if (checkCooldown > 0) return;

        var potentialTarget = fov.PotentialTarget(fovDataSO.viewRadius, fovDataSO.viewAngle);

        if (potentialTarget == null)
        {
            ResetLockedTarget();
            return;
        }

        SetLockedTarget(potentialTarget);
    }

    public bool IsTargetVisible(Transform target)
    {
        return fov.IsTargetVisible(target, fovDataSO.viewRadius, fovDataSO.viewAngle);
    }

    #region Current Target State Control
    public void SetLockedTarget(IDamagable target)
    {
        currentTarget = target;
    }

    public void ResetLockedTarget()
    {
       
        currentTarget = null;
     

    }

    public void StartCheckCooldown()
    {
        StartCoroutine(CheckCooldownCoroutine());
    }
    #endregion

    IEnumerator CheckCooldownCoroutine()
    {
        float elapsed = 0f;

        while(elapsed < maxCheckCooldown)
        {
           
            elapsed += Time.deltaTime;
            checkCooldown = elapsed;

            yield return null;
        }

        checkCooldown = 0;
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
