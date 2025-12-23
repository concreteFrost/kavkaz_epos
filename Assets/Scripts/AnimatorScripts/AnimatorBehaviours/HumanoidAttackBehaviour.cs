using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{
    public float enableTime = 0.3f;
    public float disableTime = 0.6f;

    public bool attackEnabled = false;
    public bool attackDisabled = false;

    IAttackSource inv;
    ICharacterStatsModifier stats;
    ICharacterCombatAnimData combatAnimData;
    IHumanoidMovementAnimData motor;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<IAttackSource>();
        stats = animator.GetComponentInChildren<ICharacterStatsModifier>();
        combatAnimData = animator.GetComponentInChildren<ICharacterCombatAnimData>();
        motor = animator.GetComponent<IHumanoidMovementAnimData>();    
 
        stats.ReduceStamina(inv.CurrentWeapon.GetCurrentAttack().staminaPenalty);
        animator.applyRootMotion = true;
        //combatAnimData.BlockRotation = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!animator.applyRootMotion)
        {
            animator.applyRootMotion = true;
        }

        if (!motor.BlockRotation)
        {
           motor.BlockRotation = true;
        }

        float t = stateInfo.normalizedTime;

        if (!attackEnabled & t >= enableTime)
        {
           
            inv.CurrentWeapon.PerformAttack();
           
            attackEnabled = true;
        }

        if (attackEnabled & t >= disableTime)
        {
            inv.CurrentWeapon.CancelAttack();  
            attackEnabled = false;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackEnabled = false;
        attackDisabled = false;
        motor.BlockRotation = false;
        inv.CurrentWeapon.CancelAttack();
        animator.applyRootMotion = false;
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
