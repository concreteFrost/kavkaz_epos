using UnityEngine;

public class EnemyMoveToInterruptorState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    HumanoidAIMotor motor;
    HumanoidAgentController agentController;
    EnemyFOVController fov;
    EnemyPassiveInterruptionHandler passiveInterruptionTracker;
    EnemyNotifierManager notifierManager;

    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;
        agentController = context.agentController;
        passiveInterruptionTracker = context.interruptionManager.passiveInterruptionHandler;
        notifierManager = context.notifierManager;  
       
        fov.ResetLockedTarget();
        motor.ResetLockTarget();
        motor.ResetSprint();

       
        destination = passiveInterruptionTracker.InterruptorPosition();

    }

    public override AIStateResult Run()
    {
        if (fov.currentTarget != null)
        {

            Debug.Log("notified");
            notifierManager.Notify(fov.currentTarget);
            return AIStateResult.Chase;
        }

        fov.CheckTargets();

        motor.MoveCharacter(passiveInterruptionTracker.InterruptorPosition());

        if (fov.currentTarget != null)
            return AIStateResult.Chase;

        if (passiveInterruptionTracker.IsInterrupted())
        {
            
            return passiveInterruptionTracker.ReactOnDamage(context.self.position, context.animator);
        }

        if (!NavAgentUtils.HasCompletePath(context.self.position, destination))
            return AIStateResult.Idle;

        if (agentController.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {
     
    }

}