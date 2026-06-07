using UnityEngine;

public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    HumanoidAIMotor motor;
    HumanoidAgentController agentController;
    EnemyFOVController fov;

    EnemyPatrolHandler patrolStateTracker;
    EnemyPassiveInterruptionHandler passiveInterruptionTracker;

    EnemyNotifierManager notifierManager;

    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;
        agentController = context.agentController;
        patrolStateTracker = context.stateTracker.patrolHandler;
        passiveInterruptionTracker = context.interruptionManager.passiveInterruptionHandler;
        notifierManager = context.notifierManager;  

        fov.ResetLockedTarget();
        motor.ResetLockTarget();    
        motor.ResetSprint();

        if (patrolStateTracker.HasReachedMaxWalks())
        {
            destination = context.permamentPosition;
            motor.MoveCharacter(destination);

            patrolStateTracker.ResetPatrol();
            return;
        }

        var agentTypeId = context.agentController.agent.agentTypeID;
        if (NavAgentUtils.TryGetRandomReachablePoint(
                context.self.position,
                agentTypeId,
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
        if (fov.currentTarget != null)
        {
           
            notifierManager.Notify(fov.currentTarget);
            return AIStateResult.Chase;
        }

        fov.CheckTargets();
           
        if (passiveInterruptionTracker.IsInterrupted())
        {
           return passiveInterruptionTracker.ReactOnDamage(context.self.position, context.animator);
        }

        var agentTypeId = context.agentController.agent.agentTypeID;
        if (!NavAgentUtils.HasCompletePath(context.self.position, destination, agentTypeId))
            return AIStateResult.Idle;

        if (agentController.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {

    }
}
