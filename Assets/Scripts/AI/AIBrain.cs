using UnityEngine;

public abstract class AIBrain : MonoBehaviour, IBrain
{
    public abstract void ForceStop();
    public abstract void SetPaused(bool paused);
    public abstract void SetInitialState();

    public abstract void ForceChangeState(IAIState state);

}