using System.Collections.Generic;
using UnityEngine;

public class PushCollider : MonoBehaviour
{
    protected Collider col;
    List<CharacterType> objectsToIgnore;
    private bool pushRegistered = false;


    public void Init(List<CharacterType> objectsToIgnore)
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;

        this.objectsToIgnore = objectsToIgnore;

        DisableCollider();
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
        pushRegistered = false; 
    }

    protected bool TryGetDamagable(Collider other, out IPushable damagable)
    {
        damagable = other.GetComponentInChildren<IPushable>() ?? other.GetComponent<IPushable>();
        return damagable != null;
    }

    protected bool NotInTargetList(IPushable damagable)
    {

        if (objectsToIgnore == null || objectsToIgnore.Count == 0) return true;
        return objectsToIgnore.Contains(damagable.CharacterType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(pushRegistered) return;

        if (!TryGetDamagable(other, out var damagable)) return;

        if (NotInTargetList(damagable)) return;


        var dir = GetPushDir(other);

        damagable.GetPushed(dir);

        pushRegistered = true;  
        
    }

    private PushDirection GetPushDir(Collider other)
    {
        // Вектор от игрока к объекту, который толкает
        Vector3 pushDirection = other.transform.position - transform.position;

        // Определяем, спереди или сзади
        float dot = Vector3.Dot(transform.forward, pushDirection.normalized);

        if (dot > 0)
        {
            return PushDirection.Forward;
        }
        else
        {

            return PushDirection.Back;
        }
    }
}
