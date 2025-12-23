using UnityEngine;

public class ClimbingBehaviour : StateMachineBehaviour
{

    IClimber climber;

    PlayerController contr;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        climber = animator.GetComponentInChildren<IClimber>();  
        contr = animator.GetComponentInChildren<PlayerController>();  
        animator.applyRootMotion = true;    
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.applyRootMotion)
            animator.applyRootMotion = true;

        //if (stateInfo.normalizedTime > 0.7f)
        //{
        //    float pushSpeed = 2f;

        //    Vector3 pos = animator.transform.position;
            
        //    pos.z += pushSpeed * Time.deltaTime; // ❗ ТОЛЬКО Z
        //    animator.transform.position = pos;
        //}
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        animator.applyRootMotion = false;

        contr.ExitClimb();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
