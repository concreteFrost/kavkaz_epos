using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IAIState<HumanoidAIContext>
{
    public void Enter(HumanoidAIContext ctx)
    {
        ctx.motor.StopMovement();
        ctx.controller.ResetLockTarget();   
    }

    public void Exit(HumanoidAIContext ctx)
    {
        
    }   

    public void Run(HumanoidAIContext ctx)
    {
        ctx.fov.CheckTargets();

        if (ctx.fov.currentTarget != null)
        {
            ctx.controller.SetLockTarget(ctx.fov.currentTarget.GetAimTransform());
            Debug.Log("current target found");
        }
    }

    
}
