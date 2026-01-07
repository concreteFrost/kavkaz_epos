using UnityEngine;

public interface IAIState<T>
{
    void Enter(T ctx);
    void Run(T ctx);
    void Exit(T ctx);
}
