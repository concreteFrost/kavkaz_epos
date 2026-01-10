using UnityEngine;

public class EnemyMoveToDefaultPositionState : AIState<EnemyBrainContext>
{

    private Vector3 destination;

    public override void Enter()
    {

        context.fov.ResetTarget();
        context.motor.ResetSprint();

        destination = context.permamentPosition;
        context.motor.MoveCharacter(destination);
        context.motor.IsSprinting = true;

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



        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
