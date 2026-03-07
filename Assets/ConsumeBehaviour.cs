using UnityEngine;

public class ConsumeBehaviour : StateMachineBehaviour
{
    CharacterConsumeController consumeController;

    IHumanoidMovement motor;
    IDamagable damagable;
    [SerializeField] private bool hasInteracted;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        consumeController = animator.GetComponentInChildren<CharacterConsumeController>();
        motor = animator.GetComponent<IHumanoidMovement>();
        damagable = animator.GetComponentInChildren<IDamagable>();

        animator.applyRootMotion = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!motor.StopMove)
            motor.StopMove = true;

        if (animator.applyRootMotion == false)
            animator.applyRootMotion = true;

        AnimationInfoSO clip = consumeController.GetAnimtionClip();

        if (clip == null) return;

        float t = stateInfo.normalizedTime;

        if (hasInteracted || damagable.IsDamaged) return;

        if (!hasInteracted && t>clip.hitStartFrame)
        {
            consumeController.Consume();
            hasInteracted = true;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        motor.StopMove = false;
        animator.applyRootMotion = false;
        hasInteracted = false;

        animator.speed = 1;

        consumeController.EndConsume();
    }


}
