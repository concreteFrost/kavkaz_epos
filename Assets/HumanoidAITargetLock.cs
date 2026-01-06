using System.Collections.Generic;
using UnityEngine;

public class HumanoidAITargetLock : MonoBehaviour, ITargetLocker
{
    public IDamagable currentTarget;

    [SerializeField] Transform eyes;
    [SerializeField] private float viewRadius = 20f;
    [SerializeField] private float viewAngle = 70f;

    //float cullingDistance = 100f; // задает радиус отключения поиска

    public LayerMask obstacleMask;

    #region ILockableContract
    private bool isLockedOnTarget = false;
    public bool IsLockedOnTarget { get => isLockedOnTarget; set => isLockedOnTarget = value; }
    #endregion

    public CharacterType targetType;
    public List<CharacterType> objectsToScan = new List<CharacterType>(); // обьекты интереса

    bool isLookingForTargets = true;

    public void Init()
    {
        currentTarget = null;
        isLookingForTargets = true;
    }

    private void Update()
    {
        if (!isLookingForTargets) return;

        CheckTargets(); //убрать и перенести в состояния

    }

    #region Search

    /// <summary>
    /// Проверяет наличие целей в заданом радиусе
    /// </summary>
    private void CheckTargets()
    {

        if(eyes == null)
        {
            Debug.Log("no eyes assigned. field of view is not working");
            return; 
        }
        Collider[] colliders = Physics.OverlapSphere(eyes.position, viewRadius);

        foreach (Collider collider in colliders)
        {
            Transform targetTransform = collider.transform;

            IDamagable lockable = targetTransform.GetComponentInChildren<IDamagable>() ??
                                   GetComponent<IDamagable>();  

            if (lockable != null)
            {
               
                if (IsTargetVisible(lockable) && !lockable.IsDead() && objectsToScan.Contains(lockable.CharacterType))
                {
                    SetCurrentTarget(lockable);
                    break;
                }

            }

        }
    }

    /// <summary>
    /// Проверяет видна ли цель и нет ли припятствий
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    private bool IsTargetVisible(IDamagable targetTransform)
    {

        IDamagable targetLockable = targetTransform;

        Vector3 directionToTarget = (targetLockable.GetAimTransform().position - eyes.position).normalized;
        float distanceToTarget = Vector3.Distance(eyes.position, targetLockable.GetAimTransform().position);

        if (distanceToTarget > viewRadius) return false;
        if (Vector3.Angle(eyes.forward, directionToTarget) > viewAngle / 2) return false;

        return !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, obstacleMask);
    }

    #endregion


    #region Target State
    private void SetCurrentTarget(IDamagable target)
    {
        //currentTarget = target.TargetToAim;
        currentTarget = target;
        //Debug.Log(currentTarget);

    }

    public void ResetCurrentTarget()
    {
        currentTarget = null;   
    }
    #endregion

    #region Target Search State
    public void SetLookingForTargets(bool lookingForTargets)
    {
        isLookingForTargets = lookingForTargets;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (eyes == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyes.position, viewRadius);

        Vector3 forward = eyes.forward;
        float halfAngle = viewAngle / 2;
        Quaternion leftRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfAngle, 0);

        Vector3 leftBoundary = leftRotation * forward * viewRadius;
        Vector3 rightBoundary = rightRotation * forward * viewRadius;

        Gizmos.DrawRay(eyes.position, leftBoundary);
        Gizmos.DrawRay(eyes.position, rightBoundary);

        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(eyes.position, currentTarget.GetAimTransform().position);
        }
    }






}
