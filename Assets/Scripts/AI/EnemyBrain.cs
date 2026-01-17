using UnityEngine;

public enum AIStateResult
{
    None = 0,
    Idle = 1,
    Patrol = 2,
    Chase = 3,
    Attack = 4,
    Wait = 5,
    MoveToStartPosition = 6,
}

public class EnemyBrain : MonoBehaviour
{
    EnemyBrainContext context;
    EnemyDamageHandler damageHandler;

    internal AIStateMachine stateMachine = new AIStateMachine();

    public AIState<EnemyBrainContext> currentState;

    [Header("Behaviours")]
    [SerializeField] private AIState<EnemyBrainContext> idle;
    [SerializeField] private AIState<EnemyBrainContext> patrol;
    [SerializeField] private AIState<EnemyBrainContext> chase;
    [SerializeField] private AIState<EnemyBrainContext> attack;
    [SerializeField] private AIState<EnemyBrainContext> wait;
    [SerializeField] private AIState<EnemyBrainContext> moveToStart;

    public void Init(EnemyBrainContext context)
    {
        this.context = context; 

        idle.Init(context);
        patrol.Init(context);
        chase.Init(context);    
        attack.Init(context);
        wait.Init(context);
        moveToStart.Init(context);

        stateMachine.ChangeState(idle);

        damageHandler = new EnemyDamageHandler(context);            
    }

    private void OnDisable()
    {
        damageHandler.Dispose();
    }


    void Update()
    {
        if (context.damageController.IsDead())
        {
            stateMachine.ForceExit();
            return;
        }
        stateMachine.Run();

        switch (stateMachine.CurrentState.Run())
        {
            case AIStateResult.Idle:
                stateMachine.ChangeState(idle);
                break;
            case AIStateResult.Patrol:
                stateMachine.ChangeState(patrol);
                break;
            case AIStateResult.Chase:
                stateMachine.ChangeState(chase);
                break;
            case AIStateResult.Attack:
                stateMachine.ChangeState(attack);
                break;
            case AIStateResult.Wait:
                stateMachine.ChangeState(wait);
                break;
            case AIStateResult.MoveToStartPosition:
                stateMachine.ChangeState(moveToStart);
                break;
        }

        currentState = stateMachine.CurrentState as AIState<EnemyBrainContext>;
    }

}
