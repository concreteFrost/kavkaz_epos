using UnityEngine;

public class PlayerAnimator : BaseHumanoidAnimatorController
{
    IHumanoidMovement locomotion;
    IHumanoidCombat combatController;
    IDamagable damageController;
    ITargetLocker targetLock;
 
    public void Init(HumanoidAnimatorService provider)
    {
        this.animator = provider.animator;
        this.locomotion = provider.motor;
        this.combatController = provider.combatController;
        this.targetLock = provider.targetLock;
        this.damageController = provider.damageController;
    }

    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        UpdateLocomotionState(locomotion);
        UpdateCombatState(combatController);
        UpdateDamageState(damageController);    
        //UpdateTargetLockState(targetLock);


    }


}