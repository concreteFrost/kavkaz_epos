using UnityEngine;

public class EnemyIdleState : AIState<EnemyBrainContext>
{

    [SerializeField] private float currIdleTime;
    public float maxIdleTime;
    public override void Enter()
    {
        var stats = context.stats.statsSO as HumanoidCharacterStatsSO;
        context.motor.StopMovement();
        context.motor.ResetSprint();
        context.fov.ResetTarget();  

        maxIdleTime = Random.Range((float)stats.minIdleStationary, (float)stats.maxIdleStationary);
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
            
            return AIStateResult.Chase;
        }

        return AIStateResult.None;
    }


    public override void Exit()
    {

    }



}
