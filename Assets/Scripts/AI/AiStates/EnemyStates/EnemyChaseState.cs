using UnityEngine;

public class EnemyChaseState : AIState<EnemyBrainContext>
{
    Transform chaseTarget;
    EnemyStateTracker tracker;
    CharacterBehaviourStatsSO stats;


    public override void Enter()
    {
        tracker = context.stateTracker;
        stats = tracker.stats;
        tracker.ResetChaseState();
        chaseTarget = context.fov.currentTarget.GetOrigin();

        context.motor.IsSprinting = true;
    }

    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        var tracker = context.stateTracker;
        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        Transform target = chaseTarget;
       

        // 2. цель недостижима
        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);
        tracker.UpdateCantReachTimer(canReach);

        if (tracker.cantReachTimer > stats.maxCantReachTimer)
            return AIStateResult.Wait;

        // 3. цель потеряна
        bool isTargetVisible = fov.IsTargetVisible(
            fov.currentTarget.GetAimTransform()
        );
        tracker.UpdateLostTargetTimer(isTargetVisible);

        if (tracker.lostTargetTimer > stats.maxLostTargetTimer)
            return AIStateResult.Patrol;

        // 4. дистанция
        float distanceToTarget =
            Vector3.Distance(self.position, target.position);

        if (distanceToTarget > stats.maxChaseDistance)
            return AIStateResult.MoveToStartPosition;

        if (distanceToTarget > stats.distanceToStop)
        {
            motor.MoveCharacter(target.position);
        }
        else
        {
            return AIStateResult.Attack;
        }

        //motor.IsSprinting = distanceToTarget > stats.distanceToRun;

        return AIStateResult.None;
    }

    public override void Exit() { }
}
