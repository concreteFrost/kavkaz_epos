using UnityEngine;

public class HumanoidAIAnimatorController : BaseHumanoidAnimatorController
{
    IHumanoidMovement movement;
    ITargetLocker targetLocker;
    IDamagable damagable;
    IHumanoidCombat attackSource;

    public void Init(HumanoidAnimatorService service)
    {
        this.movement = service.motor;   
        this.animator = service.animator;
        this.targetLocker = service.targetLock;
        this.damagable = service.damageController;
        this.attackSource = service.combatController;
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
        //UpdateTargetLockState(targetLocker);
        UpdateCombatState(attackSource);    
    }
}
