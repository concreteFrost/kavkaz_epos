using UnityEngine;

public class EnemyWaitForTargetState : AIState<EnemyBrainContext>
{
    [SerializeField] private float lostTargetTimer = 0f;
    [SerializeField] private float cantReachTimer = 0f;
    [SerializeField] private float maxWaitTimer = 3f;

    Transform chaseTarget;

    public override void Enter()
    {

        cantReachTimer = 0f;
        chaseTarget = context.fov.currentTarget.GetOrigin();

        context.motor.StopMovement();
    }

    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        bool canReach = NavAgentUtils.HasCompletePath(self.position, chaseTarget.position);

        if (!canReach)
        {
            cantReachTimer += Time.deltaTime;
        }
        else
        {
            return AIStateResult.Chase;
        }

        if(cantReachTimer >= maxWaitTimer)
        {
            Debug.Log("cant reach, return to idle");
            return AIStateResult.MoveToStartPosition;
        }

        return AIStateResult.None;
    }



    public override void Exit()
    {
        chaseTarget = null;
    }

}