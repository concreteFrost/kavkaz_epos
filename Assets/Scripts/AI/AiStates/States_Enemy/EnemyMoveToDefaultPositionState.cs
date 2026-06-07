using UnityEngine;

public class EnemyMoveToDefaultPositionState : AIState<EnemyBrainContext>
{

    private Vector3 destination;
    EnemyFOVController fov;
    HumanoidAIMotor motor;

    public override void Enter()
    {
        fov = context.fov;
        motor = context.motor;
        
        fov.StartCheckCooldown();   
        fov.ResetLockedTarget();
        motor.ResetLockTarget();   
        
        context.motor.ResetSprint();

        motor.StopMovement();

        destination = context.permamentPosition;
        motor.MoveCharacter(destination);

        motor.IsSprinting = true;

        context.fov.ResetLockedTarget();
    }

    public override AIStateResult Run()
    {
        var agentTypeId = context.agentController.agent.agentTypeID;
        bool canReach = NavAgentUtils.HasCompletePath(context.self.position, destination, agentTypeId);

        if (!canReach)
        {
            Debug.Log("i cant reach");
            return AIStateResult.Idle;
        }

        if (context.agentController.HasReachedDestination())
            return AIStateResult.Idle;


        fov.CheckTargets(); 

        if(fov.currentTarget != null)
        {
            return AIStateResult.Chase;
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
