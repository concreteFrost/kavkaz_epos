using System.Data;
using UnityEngine;

public  class AINotifierListener<T>
{
    private float listenDistance = 120f;
    AIStateMachine stateMachine;
    AIState<T> reactionState;
    Transform self;

    public AINotifierListener(Transform self,AIStateMachine stateMachine, AIState<T> reactionState)
    {

        this.self = self;
        this.stateMachine = stateMachine;
        this.reactionState = reactionState;
    }

    public void OnNotify(Transform target)
    {

        if (stateMachine.CurrentState == null) return;

        if(target == self)
        {
            Debug.Log("i am the notifier");
        }

        float distance = Vector3.Distance(self.position,target.position);

        if (distance > listenDistance) return;

        Debug.Log("listening");

        stateMachine.ChangeState(reactionState);


    }


}
