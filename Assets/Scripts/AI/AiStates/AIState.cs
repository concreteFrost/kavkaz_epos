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

    public abstract AIStateResult Run();
   
}
