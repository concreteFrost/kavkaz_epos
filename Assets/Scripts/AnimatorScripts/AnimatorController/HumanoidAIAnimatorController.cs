using UnityEngine;

public class HumanoidAIAnimatorController : BaseHumanoidAnimatorController
{

    public override void Init(HumanoidAnimatorService service)
    {
       base.Init(service);  
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
