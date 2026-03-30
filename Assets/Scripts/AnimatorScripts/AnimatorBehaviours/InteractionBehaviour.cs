using UnityEngine;

public class InteractionBehaviour : StateMachineBehaviour
{
    IInteractor collector;
    IHumanoidMovement motor;
    IDamagable damagable;
    [SerializeField] private AnimationInfoSO clip;
    [SerializeField] private bool hasInteracted;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collector = animator.GetComponentInChildren<IInteractor>();
        motor = animator.GetComponent<IHumanoidMovement>();
        damagable = animator.GetComponentInChildren<IDamagable>();  

        animator.applyRootMotion = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!motor.StopMove) 
            motor.StopMove = true;

        if(!motor.BlockRotation)
            motor.BlockRotation = true; 

        if (animator.applyRootMotion == false)
            animator.applyRootMotion = true;

        

        float t = stateInfo.normalizedTime;

        if (hasInteracted || collector.PickableItem == null || damagable.IsDamaged) return;

        if(t > clip.hitStartFrame && t<clip.hitEndFrame)
        {
            collector.FinishInteraction();  
            hasInteracted = true;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        motor.StopMove = false;
        motor.BlockRotation = false;
        animator.applyRootMotion = false;
        hasInteracted = false;
       
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
