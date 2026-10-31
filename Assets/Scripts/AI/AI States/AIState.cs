using UnityEngine;

public abstract class AIState<Context> : MonoBehaviour, IAIState
{

    protected Context context;

    public void Init(Context context)
    {
        this.context = context;

    }

    public abstract void Enter();

    public abstract void Exit();

    /// <summary>Прерывает состояние и все принадлежащие ему корутины.</summary>
    public void ForceExit()
    {
        Exit();
        StopAllCoroutines();
    }

    public abstract AIStateResult Run();
   
}
