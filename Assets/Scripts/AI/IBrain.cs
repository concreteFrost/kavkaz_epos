public interface IBrain
{
    void ForceStop();
    void SetPaused(bool paused);
    void SetInitialState();

    void ForceChangeState(IAIState state);


}