using UnityEngine;

public class HumanoidAIAnimator : BaseHumanoidAnimator
{
    IHumanoidMovement movement;
    ITargetLocker targetLocker;
    IDamagable damagable;

    public void Init(Animator anim, IHumanoidMovement movement, ITargetLocker targetLocker, IDamagable damagable)
    {
        this.movement = movement;   
        this.animator = anim;
        this.targetLocker = targetLocker;
        this.damagable = damagable;
    }

    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        UpdateLocomotionState(movement);
        UpdateDamageState(damagable);
        UpdateTargetLockState(targetLocker);
    }
}
