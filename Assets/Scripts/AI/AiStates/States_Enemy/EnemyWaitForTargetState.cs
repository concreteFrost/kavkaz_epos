using UnityEngine;

public class EnemyWaitForTargetState : AIState<EnemyBrainContext>
{

    EnemyWaitForTargetHandler handler;

    EnemyFOVController fov;
    HumanoidAIMotor motor;

    public override void Enter()
    {
        handler = context.stateTracker.waitForTargetHandler;
        fov = context.fov;
        motor = context.motor;

        handler.ResetWaitState();    

        motor.StopMovement();
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());
    }

    public override AIStateResult Run()
    {
       
        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        Transform target = fov.currentTarget.GetOrigin();

        var agentTypeId = context.agentController.agent.agentTypeID;
        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position, agentTypeId);

        if (!canReach)
        {
            handler.UpdateWaitTimer(canReach);
        }
        else
        {
            return AIStateResult.Chase;
        }

        if(handler.HasWaitTimerExceeded())
        {
            //Debug.Log("cant reach, return to idle");
           
            return AIStateResult.MoveToStartPosition;
        }

        return AIStateResult.None;
    }



    public override void Exit()
    {
        //fov.ResetTarget();
        motor.ResetLockTarget();
        
    }

}