using System.Collections.Generic;
using UnityEngine;

public class AIFov
{

    Transform eyes;
    Transform viewDirection;
    List<CharacterType> objectsToScan;
    LayerMask obstacleMask;
    LayerMask layerToIgnore;


    public AIFov(Transform eyes, Transform viewDirection, List<CharacterType> objectsToScan, LayerMask obstacleMask, LayerMask layerToIgnore)
    {
        this.eyes = eyes;
        this.viewDirection = viewDirection;
        this.objectsToScan = objectsToScan;
        this.obstacleMask = obstacleMask;
        this.layerToIgnore = layerToIgnore;

    }
    public IDamagable PotentialTarget(float viewRadius, float viewAngle)
    {

        if (eyes == null || viewDirection == null)
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
    /// Проверяет видна ли цель и нет ли припятствий
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    public bool IsTargetVisible(Transform targetTransform, float viewRadius, float viewAngle)
    {

       
        if (eyes == null || viewDirection == null || targetTransform == null) return false;

        Vector3 directionToTarget = (targetTransform.position - eyes.position).normalized;
        float distanceToTarget = Vector3.Distance(eyes.position, targetTransform.position);

        if (distanceToTarget > viewRadius) return false;
        // Р Р°Р·РЅРёС†Р° РІ РІС‹СЃРѕС‚Рµ РЅРµ РґРѕР»Р¶РЅР° РІС‹РІРѕРґРёС‚СЊ Р±Р»РёР·РєСѓСЋ С†РµР»СЊ РёР· РіРѕСЂРёР·РѕРЅС‚Р°Р»СЊРЅРѕРіРѕ СЃРµРєС‚РѕСЂР° РѕР±Р·РѕСЂР°.
        Vector3 planarDirectionToTarget = Vector3.ProjectOnPlane(directionToTarget, viewDirection.up);
        Vector3 planarForward = Vector3.ProjectOnPlane(viewDirection.forward, viewDirection.up);
        if (Vector3.Angle(planarForward, planarDirectionToTarget) > viewAngle / 2) return false;

        return !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, obstacleMask);
    }





}
