using NUnit;
using UnityEngine;

public class SpellCastBehaviour : StateMachineBehaviour
{
    IEmitter emitter;
    IHumanoidMovement motor;
    CharacterStatsController stats;
    IDamagable damageController;
    IPushable pushable;

    bool hitActive = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        emitter = animator.GetComponentInChildren<IEmitter>();

     
        motor = animator.GetComponent<IHumanoidMovement>();
        stats = animator.GetComponentInChildren<CharacterStatsController>();
        damageController = animator.GetComponentInChildren<IDamagable>();
        pushable = animator.GetComponentInChildren<IPushable>();

        hitActive = false;

        motor.BlockRotation = true;
        motor.StopMove = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.applyRootMotion) animator.applyRootMotion = true;

        if (damageController.IsDamaged || pushable.IsPushed) return;

        var spell = emitter.Projectile() as SpellProjectileSO;
        var animation = spell.animation;


        //if (attack == null) return;

        //animator.speed = attack.animationInfo.animationSpeed;

        float t = stateInfo.normalizedTime;

        if (!hitActive && t >= animation.hitStartFrame)
        {
            emitter.Emit();
            stats.Stamina.ReduceCurrent(spell.staminaPenalty);
            hitActive = true;
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitActive = false;

        animator.speed = 1f;

        hitActive = false;
        motor.StopMove = false;

        animator.applyRootMotion = false;
        motor.BlockRotation = false;

        emitter.EndEmit();  


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
