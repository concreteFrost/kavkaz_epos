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
    Strafe = 7,
    MoveToInterruptor = 8,
}

public class EnemyBrain : AIBrain
{
    EnemyBrainContext context;

    private AIStateMachine stateMachine = new AIStateMachine();

    public AIState<EnemyBrainContext> currentState;
    

    [Header("Behaviours")]
    [SerializeField] private AIState<EnemyBrainContext> idle;
    [SerializeField] private AIState<EnemyBrainContext> patrol;
    [SerializeField] private AIState<EnemyBrainContext> chase;
    [SerializeField] private AIState<EnemyBrainContext> attack;
    [SerializeField] private AIState<EnemyBrainContext> strafe;
    [SerializeField] private AIState<EnemyBrainContext> wait;
    [SerializeField] private AIState<EnemyBrainContext> moveToStart;
    [SerializeField] private AIState<EnemyBrainContext> moveToInterruptor;

    public bool isActivated = false;

    
    public void Init(EnemyBrainContext context)
    {
        this.context = context;

        idle.Init(context);
        patrol.Init(context);
        chase.Init(context);
      
        strafe.Init(context);
        wait.Init(context);
        moveToStart.Init(context);
        moveToInterruptor.Init(context);

        attack.Init(context);

        SetActivated(true);

    }

    void Update()
    {
        if (context.damageController.IsDead || stateMachine.CurrentState == null )
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
            case AIStateResult.Strafe:
                stateMachine.ChangeState(strafe);
                break;
            case AIStateResult.Wait:
                stateMachine.ChangeState(wait);
                break;
            case AIStateResult.MoveToStartPosition:
                stateMachine.ChangeState(moveToStart);
                break;
            case AIStateResult.MoveToInterruptor:
                stateMachine.ChangeState(moveToInterruptor);
                break;


        }

        currentState = stateMachine.CurrentState as AIState<EnemyBrainContext>;
    }

    public void ForceChangeState(AIState<EnemyBrainContext> state)
    {
        
        state.Init(context);
        stateMachine.ChangeState(state);
    }

    public override void ForceStop()
    {
        stateMachine.ForceExit();
    }

    public override void SetInitialState()
    {
        stateMachine.ChangeState(idle);
    }

    //public void ForceChase()
    //{
    //    stateMachine.ChangeState(chase);
    //}

    public void SetActivated(bool activated)
    {
      
        isActivated = activated;

        if(isActivated) SetInitialState();
    }
    


}
