public interface IBrain
{
    void ForceStop();
    void SetInitialState();

    void ForceChangeState(IAIState state);


}