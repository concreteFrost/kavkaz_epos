using UnityEngine;

public class PlayerAnimatorController : BaseHumanoidAnimatorController
{

 
    public override void Init(HumanoidAnimatorService provider)
    {
        base.Init(provider);
       
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