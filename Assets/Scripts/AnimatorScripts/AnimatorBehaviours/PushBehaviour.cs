using UnityEngine;

public class PushBehaviour : StateMachineBehaviour
{

  
    IHumanoidMovement motor;
    ICharacterStatsController statsModifier;
    IPushSource pushSource;

    IHumanoidCombat combat;
    HumanoidStats stats;

    bool pushActive = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
       
        motor = animator.GetComponent<IHumanoidMovement>();
        pushSource = animator.GetComponentInChildren<IPushSource>();
        statsModifier = animator.GetComponentInChildren<ICharacterStatsController>();
        combat = animator.GetComponentInChildren<IHumanoidCombat>();
        stats = animator.GetComponentInChildren<HumanoidStats>();


        statsModifier.ReduceStamina(stats.statsSO.staminaPushReducePenalty);

        animator.applyRootMotion = true;
        pushActive = false;

        // блокируем вращение персонажа во время атаки
        motor.BlockRotation = true;
        motor.StopMove = true;


        combat.ResetCombo();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.applyRootMotion) animator.applyRootMotion = true;

        //if (damagable.IsDamaged) return;

        float t = stateInfo.normalizedTime % 1f;

        var animationData = pushSource.AnimationData();

        if (!pushActive && t >= animationData.hitStartFrame)
        {
            pushSource.PerformPush();
            pushActive = true;
        }

        if (pushActive && t >= animationData.hitEndFrame)
        {
            pushSource.CancelPush();
            pushActive = false;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
        motor.StopMove = false;

        pushSource.CancelPush();

        animator.applyRootMotion = false;
        motor.BlockRotation = false;
        

       

        // уведомляем контроллер, что атака завершена
        
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
