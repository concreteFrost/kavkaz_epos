
using System;
using System.Collections;
using UnityEngine;

public class EnemyFOVController : MonoBehaviour, ITargetLocker
{
    [SerializeField] FovDataSO fovDataSO;
    Transform eyes;
    Transform viewDirection;

    AIFov fov;
   
    public IDamagable currentTarget;

    public float checkCooldown = 0f;
    private float maxCheckCooldown = 2f;


    #region ITargetLocker Contract
    public IDamagable CurrentTarget() => currentTarget != null ? currentTarget : null;
    
    public static Action<CharacterType,string> TargetFound;
    public static Action<CharacterType,string> TargetLost;
    string fovId;
    #endregion
    private void Update()
    {
        if (currentTarget == null) return;

        if (currentTarget.IsDead || !currentTarget.DamageCollider().enabled)
        {
            ResetLockedTarget();
        }
    }

    public void Init(CharacterBoneSocket boneSockets, string fovId, Transform viewDirection)
    {
        this.fovId = fovId;
        eyes = boneSockets.GetEyesSocket;
        this.viewDirection = viewDirection;
        fov = new AIFov(eyes, viewDirection, fovDataSO.objectsToScan, fovDataSO.obstacleMask, fovDataSO.layerToIgnore);
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

    public bool IsTargetVisible()
    {
        if(currentTarget == null) return false;
        return fov.IsTargetVisible(currentTarget.GetAimTransform(), fovDataSO.viewRadius, fovDataSO.viewAngle);
    }

    #region Current Target State Control
    public void SetLockedTarget(IDamagable target)
    {
        TargetFound?.Invoke(target.CharacterType, fovId);
        currentTarget = target;
    }

    public IDamagable TryGetDamagable(Transform src) => src.GetComponentInChildren<IDamagable>();   

    public void ResetLockedTarget()
    {
        if(currentTarget != null)
        {
            TargetLost?.Invoke(currentTarget.CharacterType, fovId);
            currentTarget = null;
        }
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
        if (eyes == null || viewDirection == null || fovDataSO == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(eyes.position, fovDataSO.viewRadius);

        Vector3 forward = viewDirection.forward;
        float halfAngle = fovDataSO.viewAngle / 2;
        Quaternion leftRotation = Quaternion.AngleAxis(-halfAngle, viewDirection.up);
        Quaternion rightRotation = Quaternion.AngleAxis(halfAngle, viewDirection.up);

        Vector3 leftBoundary = leftRotation * forward * fovDataSO.viewRadius;
        Vector3 rightBoundary = rightRotation * forward * fovDataSO.viewRadius;

        Gizmos.DrawRay(eyes.position, leftBoundary);
        Gizmos.DrawRay(eyes.position, rightBoundary);
        Gizmos.DrawRay(eyes.position, forward * fovDataSO.viewRadius);

        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(eyes.position, currentTarget.GetAimTransform().position);
        }
    }

}
