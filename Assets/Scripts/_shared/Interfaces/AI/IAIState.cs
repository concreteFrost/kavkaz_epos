using UnityEngine;

public interface IAIState
{
    void Enter();
    AIStateResult Run();
    void Exit();
}
