using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerTargetLock : MonoBehaviour, ITargetLocker
{
    LockOnTargetUI lockOnTargetUI;

    protected Transform targetSeeker;
    public IDamagable currentTarget;

    protected bool wasTargetSearched = false;
    public bool IsLockedOnTarget { get => currentTarget != null; }

    /// <summary>
    /// Ввод мыши по оси Х при котором цель сбрасывается
    /// </summary>
    private float targetSwitchThreshold = 45f;
    private float targetCheckDistance = 5f;
    private float targetResetDistance = 7f;

    [SerializeField] private Image img;

    PlayerController controller;
    IDamagable damageController;

    CharacterType self;
    public void Init(PlayerTargetLockService provider)
    {   
        this.lockOnTargetUI = provider.lockOnTargetUI;
        this.controller = provider.controller;
        this.targetSeeker = controller.transform;
        this.damageController = provider.damageController;
  

        self = CharacterType.Player;    
    }

    private void Update()
    {
        if (currentTarget == null) return;
        if (damageController.IsDead())
        {
            ResetLockTarget();
            return;
        }
        TrackTargetDistance();
    }

    public void TrackTargetDistance()
    {
        lockOnTargetUI.CalculateImagePosition();
        CalculateDistanceToTarget();
    }

    public void SetLockedTarget()
    {
        var t = TryGetLockedTarget();

        if (t != null)
        {   
            lockOnTargetUI.SetTarget(t.GetAimTransform());
            controller.SetLockTarget(t.GetAimTransform());
        }
    }

    protected virtual void CalculateDistanceToTarget()
    {
        if (currentTarget.IsDead())
        {
            ResetLockTarget();
            return;
        }
        var dist = Vector3.Distance(targetSeeker.position, currentTarget.GetOrigin().position);

        if (dist > targetResetDistance)
        {
            ResetLockTarget();
        }
    }

    public IDamagable TryGetLockedTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockTarget();
            lockOnTargetUI.ResetTarget();
            return null;
        }

        var nearest = CheckNearestTarget();

        if(nearest != null)
        {
            currentTarget = CheckNearestTarget();
            lockOnTargetUI.SetTarget(currentTarget.GetAimTransform());

            return nearest;
        }

        return null;
    }

    protected IDamagable GetNearestTarget(Collider[] targets)
    {
        Dictionary<IDamagable, float> objectsDistances = new Dictionary<IDamagable, float>();

        foreach (var target in targets)
        {

            var lockable = target.GetComponent<IDamagable>() ?? target.GetComponentInChildren<IDamagable>();

            if(lockable == null) continue;

            float distance = Vector3.Distance(targetSeeker.position, lockable.GetOrigin().position);
 
            if (distance < targetCheckDistance && lockable.CharacterType != self)
            {
                
                objectsDistances.Add(lockable, distance);

            }
        }

        if (objectsDistances.Count == 0) return null;

        var min = objectsDistances.OrderBy((x) => x.Value).FirstOrDefault().Key;
        return min;
    }


    protected IDamagable CheckNearestTarget()
    {

        var targets = Physics.OverlapSphere(targetSeeker.position, targetCheckDistance);

        if (targets.Length > 0)
        {
            return GetNearestTarget(targets);
        }

        return null;
    }

    public  void ResetLockTarget()
    {
        currentTarget = null;
        wasTargetSearched = false;
        lockOnTargetUI.ResetTarget();
        controller.ResetLockTarget();
       
    }


    public IDamagable SwitchTarget(float mouseX)
    {
        if (currentTarget == null) return null;
        if (Mathf.Abs(mouseX) < targetSwitchThreshold) return null;

        Camera cam = Camera.main;

        Vector3 currentScreen =
            cam.WorldToScreenPoint(currentTarget.GetAimTransform().position);

        var colliders = Physics.OverlapSphere(targetSeeker.position, targetCheckDistance);

        IDamagable bestTarget = null;
        float bestDeltaX = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!col.TryGetComponent<IDamagable>(out var lockable))
                continue;

            IDamagable target = lockable;
            if (target == currentTarget) continue;

            Vector3 screenPos = cam.WorldToScreenPoint(target.GetAimTransform().position);

            float deltaX = screenPos.x - currentScreen.x;

            // вправо
            if (mouseX > 0 && deltaX <= 0) continue;
            // влево
            if (mouseX < 0 && deltaX >= 0) continue;

            float absDelta = Mathf.Abs(deltaX);
            if (absDelta < bestDeltaX)
            {
                bestDeltaX = absDelta;
                bestTarget = target;
            }
        }

        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            //state.SetLockTarget(currentTarget);
            lockOnTargetUI.SetTarget(currentTarget.GetAimTransform());
            controller.SetLockTarget(currentTarget.GetAimTransform());
        }

        return currentTarget;
    }


}
