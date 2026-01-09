using UnityEngine;

public class EnemyChaseBehaviour : AIState<EnemyBrainContext>
{
    [SerializeField] private float lostTargetTimer = 0f;
    [SerializeField] private float cantReachTimer = 0f;

    Transform chaseTarget;

    public override void Enter()
    {

        lostTargetTimer = 0f;
        cantReachTimer = 0f;

        chaseTarget = context.fov.currentTarget.GetOrigin();
    }

    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        Transform target = chaseTarget;

        // 2. цель недостижима
        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);
        cantReachTimer = canReach ? 0f : cantReachTimer + Time.deltaTime;

        var stats = context.stats.statsSO as HumanoidCharacterStatsSO;

        if (cantReachTimer > stats.maxCantReachTimer)
            return AIStateResult.Patrol;

        // 3. цель потеряна из виду
        bool isTargetVisible = fov.IsTargetVisible(fov.currentTarget.GetAimTransform());
        lostTargetTimer = isTargetVisible ? 0f : lostTargetTimer + Time.deltaTime;

        if (lostTargetTimer > stats.maxLostTargetTimer)
            return AIStateResult.Patrol;

        // 4. дистанция
        float distanceToTarget = Vector3.Distance(self.position, target.position);

        // 4.1 цель слишком далеко
        if (distanceToTarget > stats.maxChaseDistance)
        {
            Debug.Log("target is too far");
            return AIStateResult.Patrol;
        }

        // 4.2 движение
        if (distanceToTarget > stats.distanceToStop)
        {
            motor.MoveCharacter(target.position);
        }
        else
        {
            return AIStateResult.Attack;
        }

        motor.isSprinting = distanceToTarget > stats.distanceToRun;

        return AIStateResult.None;
    }





    public override void Exit()
    {
        chaseTarget = null;
        

    }


}
