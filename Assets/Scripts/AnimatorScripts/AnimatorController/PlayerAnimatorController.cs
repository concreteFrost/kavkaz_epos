using UnityEngine;

public class PlayerAnimatorController : BaseHumanoidAnimatorController
{

    public override void Init(
        Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidMeleeCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver)
    {
       
       
        base.Init(animator,overrideController,motor,combatController,targetLock,damageController,pushReceiver);
       
    }

    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        UpdateLocomotionState(movement);
        UpdateCombatState(attackSource);
        UpdateDamageState(damagable);    
        //UpdateTargetLockState(targetLock);


    }


}