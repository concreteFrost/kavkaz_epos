using UnityEngine;

public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    HumanoidAIMotor motor;
    EnemyFOVController fov;

    EnemyPatrolHandler patrolStateTracker;
    EnemyPassiveInterruptionHandler passiveInterruptionTracker;

    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;

        fov.ResetTarget();
        motor.ResetLockTarget();    
        motor.ResetSprint();

        patrolStateTracker = context.stateTracker.patrolHandler;
        passiveInterruptionTracker = context.stateTracker.interruptionTracker;

        if (patrolStateTracker.HasReachedMaxWalks())
        {
            destination = context.permamentPosition;
            motor.MoveCharacter(destination);

            patrolStateTracker.ResetPatrol();
            return;
        }

        if (NavAgentUtils.TryGetRandomReachablePoint(
                context.self.position,
                patrolStateTracker.GetMaxPatrolRadius(),
                10,
                out destination))
        {
            motor.MoveCharacter(destination);
            patrolStateTracker.IncrementWalks();
        }

    }

    public override AIStateResult Run()
    {

        fov.CheckTargets();

        if (fov.currentTarget != null)
            return AIStateResult.Chase;

        if (passiveInterruptionTracker.IsInterrupted())
        {
            passiveInterruptionTracker.UpdateInterruption();
            motor.StopMovement();
            motor.RotateToTarget(passiveInterruptionTracker.GetInterruptionDirection());
            return AIStateResult.None;
        }


        if (!NavAgentUtils.HasCompletePath(context.self.position, destination))
            return AIStateResult.Idle;

        if (motor.agentController.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
