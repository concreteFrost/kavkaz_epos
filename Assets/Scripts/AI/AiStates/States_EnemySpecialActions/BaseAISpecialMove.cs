using System;
public class BaseAISpecialMove : AIState<EnemyBrainContext>
{
    public bool isFinished = false; 


    public override void Enter()
    {
        isFinished = false;
  
    }

    public override void Exit()
    {
        isFinished = true;
 
    }

    public override AIStateResult Run()
    {
      
        return AIStateResult.None;
    }

    public virtual void OnFightEnded()
    {

    }
  

}
