using System.Collections.Generic;
using UnityEngine;

public class HumanoidAITargetLock : MonoBehaviour, ITargetLocker
{
    public Transform currentTarget;

    private bool isLockedOnTarget = false;
    public bool IsLockedOnTarget { get => isLockedOnTarget; set => isLockedOnTarget = value; }

    private IDamagable damageController;
    private HumanoidAIController controller;
    private CharacterStats stats;

    public CharacterType targetType;

    public void Init(CharacterTargetLockService service)
    {
        this.damageController = service.damageController;
        this.stats = service.stats;
        this.controller = service.controller;

    }

    private void Update()
    {

        Debug.Log(transform.position);
        if (currentTarget == null) return;
        if (damageController.IsDead())
        {
            ResetLockTarget();
            return;
        }

    }


    public virtual void ResetLockTarget()
    {
        currentTarget = null;
    }

    public Transform CheckNearestTarget()
    {

        var targets = Physics.OverlapSphere(transform.position, stats.GetTargetCheckDistance());

        if (targets.Length > 0)
        {
            currentTarget = GetNearestTarget(targets);

            return currentTarget;
        }

        return null;    


    }

    protected Transform GetNearestTarget(Collider[] targets)
    {

        foreach (var target in targets)
        {
            if (target.TryGetComponent<IDamagable>(out var lockable))
            {
                Debug.Log(target.name);
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < stats.GetTargetCheckDistance())
                    if (lockable.CharacterType == targetType)
                        return target.transform;
            }
        }


        return null;

    }


}
