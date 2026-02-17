using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{

    IDamagable dm;
    IHumanoidMovement motor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm = animator.GetComponentInChildren<IDamagable>();
        motor = animator.GetComponent<IHumanoidMovement>();
        animator.applyRootMotion = true;
        //animator.speed = 1; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



        if (animator.applyRootMotion == false)
            animator.applyRootMotion = true;

        motor.StopMove = true;
        dm.IsDamaged = true;

        //float t = stateInfo.normalizedTime;

        //if(t > 0.95f)
        //{
        //    dm.IsDamaged = false;
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.applyRootMotion = false;

        dm.IsDamaged = false;
        motor.StopMove = false;
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
