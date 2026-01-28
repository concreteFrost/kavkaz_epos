using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : AIState<EnemyBrainContext>
{

    EnemyFOVController fov;
    HumanoidAIMotor motor;

    EnemyChaseHandler chaseHandler;
    EnemyCombatHandler combatHandler;   

    public override void Enter()
    {
        fov = context.fov;
        motor = context.motor;
        chaseHandler = context.stateTracker.chaseHandler;
        combatHandler = context.stateTracker.combatHandler;

        chaseHandler.ResetChaseState();
        context.agentController.StopAgent();
         
        motor.IsSprinting = true;
    }

    public override AIStateResult Run()
    {
        if (context.ragdollController.IsRecovering)
        {
            chaseHandler.ResetChaseState();
            return AIStateResult.None;
        }
           

        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
        {
            return AIStateResult.Idle;
        }
            
        Transform target = context.fov.currentTarget.GetOrigin();
       
        // 2. цель недостижима
        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);

        if (!canReach)
        {
            return AIStateResult.Wait;
        }
        
      
        bool isTargetVisible = fov.IsTargetVisible(
            fov.currentTarget.GetAimTransform()
        );

        
        chaseHandler.UpdateLostTargetTimer(isTargetVisible);

        if (chaseHandler.HasLostTargetTimerExceeded())
            return AIStateResult.Patrol;

        // 4. дистанция
        float distanceToTarget =
            Vector3.Distance(self.position, target.position);

        if (chaseHandler.IsTargetFar(distanceToTarget))
            return AIStateResult.MoveToStartPosition;

        if (chaseHandler.IsCloseToAttack(distanceToTarget))
        {
            return AIStateResult.Attack;
          
        }
        else
        {
            motor.MoveCharacter(target.position);
           
        }

        //motor.IsSprinting = distanceToTarget > stats.distanceToRun;

        return AIStateResult.None;
    }

    public override void Exit() { }
}
