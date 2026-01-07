using UnityEngine;

public class AIStateMachine<TContext>
{
    IAIState<TContext> CurrentState;

    public void ChangeState(IAIState<TContext> nextState, TContext ctx)
    {
        if (CurrentState == nextState) return;

        CurrentState?.Exit(ctx);
        CurrentState = nextState;
        CurrentState.Enter(ctx);
    }

    public void Run(TContext ctx)
    {
        CurrentState?.Run(ctx);
    }

    public void ForceExit(TContext ctx)
    {
        CurrentState?.Exit(ctx);
        CurrentState = null;
    }
}
