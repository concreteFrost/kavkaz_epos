using UnityEngine;

public class AIStateMachine
{
    public IAIState CurrentState;

    public void ChangeState(IAIState nextState)
    {
        if (CurrentState == nextState) return;

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Run()
    {
        CurrentState?.Run();
    }

    public void ForceExit()
    {
        if(CurrentState == null) return;    

        CurrentState.ForceExit();
        CurrentState = null;
    }
}
