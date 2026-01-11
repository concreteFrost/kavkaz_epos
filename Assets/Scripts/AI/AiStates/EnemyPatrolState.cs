using UnityEngine;

public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    public override void Enter()
    {
        context.fov.ResetTarget();
        context.motor.ResetSprint();

        var tracker = context.stateTracker;

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
        if (!NavAgentUtils.HasCompletePath(context.self.position, destination))
            return AIStateResult.Idle;

        if (context.motor.HasReachedDestination())
            return AIStateResult.Idle;

        if (context.fov.currentTarget == null)
            context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
            return AIStateResult.Chase;

        return AIStateResult.None;
    }

    public override void Exit() { }
}
