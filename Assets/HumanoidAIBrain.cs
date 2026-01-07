using UnityEngine;

public class HumanoidAIBrain : MonoBehaviour
{
    HumanoidAIContext context;
    AIStateMachine<HumanoidAIContext> stateMachine = new AIStateMachine<HumanoidAIContext>();

    public IAIState<HumanoidAIContext> idleBehavour;

    public void InitContext(HumanoidAIContext context)
    {
        this.context = context;
    }

    public void InitBehaviours(IAIState<HumanoidAIContext> idleBehaviour)
    {
        this.idleBehavour = idleBehaviour; 
        stateMachine.ChangeState(idleBehaviour,context);

    }


    // Update is called once per frame
    void Update()
    {
        stateMachine.Run(context);
    }
}
