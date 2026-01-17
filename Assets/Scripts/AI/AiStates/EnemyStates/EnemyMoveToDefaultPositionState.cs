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
        
        fov.ResetTarget();
        context.motor.ResetSprint();

      

        motor.StopMovement();

        destination = context.permamentPosition;
        motor.MoveCharacter(destination);

        motor.IsSprinting = true;

        context.fov.ResetTarget();
    }

    public override AIStateResult Run()
    {
        bool canReach = NavAgentUtils.HasCompletePath(context.self.position, destination);

        if (!canReach)
        {
            Debug.Log("i cant reach");
            return AIStateResult.Idle;
        }

        if (context.motor.HasReachedDestination())
            return AIStateResult.Idle;


        //fov.CheckTargets(); 

        //if(fov.currentTarget != null)
        //{
        //    return AIStateResult.Chase;
        //}

        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
