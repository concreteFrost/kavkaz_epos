using UnityEngine;

public class EnemyIdleState : AIState<EnemyBrainContext>
{

    [SerializeField] private float currIdleTime;
    public float maxIdleTime;
    public override void Enter()
    {
        var stats = context.stats.statsSO as HumanoidCharacterStatsSO;
        context.motor.StopMovement();
        context.fov.ResetCurrentTarget();   
        context.motor.ResetLockTarget();

        maxIdleTime = Random.Range((float)stats.minIdleStationary, (float)stats.maxIdleStationary);
        currIdleTime = 0;
       
    }

    public override void Exit()
    {
        currIdleTime = 0;
    }   

    public override AIStateResult Run()
    {
        currIdleTime += Time.deltaTime;

        if (currIdleTime > maxIdleTime) {
     
            return AIStateResult.Patrol;

        }

        context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
        {
            context.motor.SetLockTarget(context.fov.currentTarget.GetAimTransform());
            return AIStateResult.Patrol;
            //return AIStateResult.Chase;
        }

        return AIStateResult.None;
    }

    
}
