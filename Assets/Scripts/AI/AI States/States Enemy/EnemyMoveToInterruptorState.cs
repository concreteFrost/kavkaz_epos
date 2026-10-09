using UnityEngine;

public class EnemyMoveToInterruptorState : AIState<EnemyBrainContext>
{
    private Vector3 destination;

    HumanoidAIMotor motor;
    HumanoidAgentController agentController;
    EnemyFOVController fov;
    EnemyNotifierManager notifierManager;

    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;
        agentController = context.agentController;
        notifierManager = context.notifierManager;  
       
        fov.ResetLockedTarget();
        motor.ResetLockTarget();
        motor.ResetSprint();

       

    }

    public override AIStateResult Run()
    {
        if (fov.currentTarget != null)
        {

            
            notifierManager.Notify(fov.currentTarget);
            return AIStateResult.Chase;
        }

        fov.CheckTargets();

        if (fov.currentTarget != null)
            return AIStateResult.Chase;


        if (!NavAgentUtils.HasCompletePath(context.self.position, destination,context.agentController.agent.agentTypeID))
            return AIStateResult.Idle;

        if (agentController.HasReachedDestination())
            return AIStateResult.Idle;



        return AIStateResult.None;
    }

    public override void Exit()
    {
     
    }

}