using UnityEngine;

public class EnemyAIAnimatorController : BaseHumanoidAnimatorController
{

    public override void Init(Animator animator,
        AnimatorOverrideController overrideController,
        CharacterAudioManager audioManager,
        IHumanoidMovement motor,
        IHumanoidMeleeCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver)
    {
        base.Init(animator, overrideController,audioManager, motor, combatController, targetLock, damageController, pushReceiver); 
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


