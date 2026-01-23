using UnityEngine;

public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    EnemyPatrolHandler patrolStateTracker;
    EnemyPassiveInterruptionHandler passiveInterruptionTracker;

    public override void Enter()
    {
        context.fov.ResetTarget();
        context.motor.ResetSprint();

        patrolStateTracker = context.stateTracker.patrolHandler;
        passiveInterruptionTracker = context.stateTracker.interruptionTracker;

        if (patrolStateTracker.HasReachedMaxWalks())
        {
            destination = context.permamentPosition;
            context.motor.MoveCharacter(destination);

            patrolStateTracker.ResetPatrol();
            return;
        }

        if (NavAgentUtils.TryGetRandomReachablePoint(
                context.self.position,
                patrolStateTracker.GetMaxPatrolRadius(),
                10,
                out destination))
        {
            context.motor.MoveCharacter(destination);
            patrolStateTracker.IncrementWalks();
        }

    }

    public override AIStateResult Run()
    {

        context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
            return AIStateResult.Chase;

        if (passiveInterruptionTracker.IsInterrupted())
        {
            passiveInterruptionTracker.UpdateInterruption();
            context.motor.StopMovement();
            context.motor.RotateToTarget(passiveInterruptionTracker.GetInterruptionDirection());
            return AIStateResult.None;
        }


        if (!NavAgentUtils.HasCompletePath(context.self.position, destination))
            return AIStateResult.Idle;

        if (context.motor.agentController.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
