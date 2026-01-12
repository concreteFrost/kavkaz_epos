using UnityEngine;

public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    private Vector3 destination;
    EnemyStateTracker tracker;

    public override void Enter()
    {
        context.fov.ResetTarget();
        context.motor.ResetSprint();

        tracker = context.stateTracker;

        if (tracker.HasReachedMaxWalks())
        {
            destination = context.permamentPosition;
            context.motor.MoveCharacter(destination);

            tracker.ResetPatrol();
            return;
        }

        if (NavAgentUtils.TryGetRandomReachablePoint(
                context.self.position,
                tracker.maxDestinationRadius,
                10,
                out destination))
        {
            context.motor.MoveCharacter(destination);
            tracker.IncrementWalks();
        }

    }

    public override AIStateResult Run()
    {

        context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
            return AIStateResult.Chase;

        if (tracker.IsInterrupted())
        {
            tracker.UpdateInterruption();
            context.motor.StopMovement();
            context.motor.RotateToTarget(context.stateTracker.interruptionDir);
            return AIStateResult.None;
        }


        if (!NavAgentUtils.HasCompletePath(context.self.position, destination))
            return AIStateResult.Idle;

        if (context.motor.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
