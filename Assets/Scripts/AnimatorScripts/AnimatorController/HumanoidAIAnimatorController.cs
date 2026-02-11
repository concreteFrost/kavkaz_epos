using UnityEngine;

public class HumanoidAIAnimatorController : BaseHumanoidAnimatorController
{

    public override void Init(Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver)
    {
        base.Init(animator, overrideController, motor, combatController, targetLock, damageController, pushReceiver); 
    }

    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            return;
        }

        UpdateLocomotionState(movement);
        UpdateDamageState(damagable);
        //UpdateTargetLockState(targetLocker);
        UpdateCombatState(attackSource);    
    }
}
