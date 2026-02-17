using System.Collections.Generic;
using UnityEngine;

public class AIFov
{

    Transform eyes;
    List<CharacterType> objectsToScan;
    LayerMask obstacleMask;
    LayerMask layerToIgnore;


    public AIFov(Transform eyes, List<CharacterType> objectsToScan, LayerMask obstacleMask, LayerMask layerToIgnore)
    {
        this.eyes = eyes;
        this.objectsToScan = objectsToScan;
        this.obstacleMask = obstacleMask;
        this.layerToIgnore = layerToIgnore;

    }
    public IDamagable PotentialTarget(float viewRadius, float viewAngle)
    {

        if (eyes == null)
        {
            Debug.Log("no eyes assigned. field of view is not working");
            return null;
        }
        Collider[] colliders = Physics.OverlapSphere(eyes.position, viewRadius,~layerToIgnore);

        foreach (Collider collider in colliders)
        {
            Transform targetTransform = collider.transform;

            IDamagable lockable = targetTransform.GetComponentInChildren<IDamagable>() ??
                                   targetTransform.GetComponent<IDamagable>();

            if (lockable != null)
            {
                bool isTarget = IsTarget(lockable) && IsTargetVisible(lockable.GetAimTransform(), viewRadius, viewAngle);

                if (isTarget) return lockable;
            }

           
        }

        return null;
    }

    private bool IsTarget(IDamagable lockable)
    {
        if (!lockable.IsDead && objectsToScan.Contains(lockable.CharacterType))
        {
            return true;

        }

        return false;
    }
    /// <summary>
    /// ѕровер€ет видна ли цель и нет ли прип€тствий
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    public bool IsTargetVisible(Transform targetTransform, float viewRadius, float viewAngle)
    {

       
        Vector3 directionToTarget = (targetTransform.position - eyes.position).normalized;
        float distanceToTarget = Vector3.Distance(eyes.position, targetTransform.position);

        if (distanceToTarget > viewRadius) return false;
        if (Vector3.Angle(eyes.forward, directionToTarget) > viewAngle / 2) return false;

        return !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, obstacleMask);
    }





}
