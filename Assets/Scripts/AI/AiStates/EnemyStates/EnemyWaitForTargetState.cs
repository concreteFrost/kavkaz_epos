using UnityEngine;

public class EnemyWaitForTargetState : AIState<EnemyBrainContext>
{

    EnemyStateTracker tracker;
    Transform chaseTarget;
    EnemyFOVController fov;
    HumanoidAIMotor motor;

    public override void Enter()
    {
        tracker = context.stateTracker;
        fov = context.fov;
        motor = context.motor;

        tracker.ResetWaitState();    
        chaseTarget =fov.currentTarget.GetOrigin();

        motor.StopMovement();
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());
    }

    public override AIStateResult Run()
    {
       
        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        bool canReach = NavAgentUtils.HasCompletePath(self.position, chaseTarget.position);

        if (!canReach)
        {
            tracker.UpdateWaitTimer(canReach);
        }
        else
        {
            return AIStateResult.Chase;
        }

        if(tracker.waitTimer >= tracker.stats.maxWaitTimer)
        {
            Debug.Log("cant reach, return to idle");
            return AIStateResult.MoveToStartPosition;
        }

        return AIStateResult.None;
    }



    public override void Exit()
    {
        chaseTarget = null;
        motor.ResetLockTarget();
        
    }

}