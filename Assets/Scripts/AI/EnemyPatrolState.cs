using UnityEngine;


public class EnemyPatrolState : AIState<EnemyBrainContext>
{
    [SerializeField] private float maxDestinationRadius = 10f;
    [SerializeField] private int maxWalks = 3;
    [SerializeField] private int currWalks = 0;

    private Vector3 destination;

    public override void Enter()
    {
        if(currWalks >= maxWalks)
        {
            destination = context.permamentPosition;
            context.motor.MoveCharacter(destination);
            
            currWalks = 0;
            return;
        }

        if (NavAgentUtils.TryGetRandomReachablePoint(context.self.position, maxDestinationRadius,10, out destination))
        {
            context.motor.MoveCharacter(destination);   
            currWalks++;
        }
    }

    public override AIStateResult Run()
    {
        bool canReach = NavAgentUtils.HasCompletePath(context.self.position, destination);

        if (!canReach)
        {
            Debug.Log("i cant reach");
            return AIStateResult.Idle;
        }

        if (context.motor.HasReachedDestination())
            return AIStateResult.Idle;

        //if (context.fov.currentTarget != null)
        //    return AIStateResult.Chase;

        return AIStateResult.None;
    }

    public override void Exit() {
        
    }

   

}
