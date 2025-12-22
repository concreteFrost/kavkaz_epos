using UnityEngine;

public class WeaponThrowBehavior : StateMachineBehaviour
{

    IAttackSource inv;
    ICharacterCombatAnimData combat;
    ICharacterMovementAnimData motor;
    ICharacterDamageAnimData damage;    
   
  
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        motor = animator.GetComponent<ICharacterMovementAnimData>();

        inv = animator.GetComponentInChildren<IAttackSource>();
        combat = animator.GetComponentInChildren<ICharacterCombatAnimData>();
        damage = animator.GetComponentInChildren<ICharacterDamageAnimData>();   

        animator.applyRootMotion = true;
        motor.BlockRotation = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (!animator.applyRootMotion) animator.applyRootMotion = true;
        if(!motor.StopMove) motor.StopMove = true;

        float t = stateInfo.normalizedTime;

        if (t >= 0.4f && t<=0.5f && !damage.IsDamaged )
        {

            inv.CurrentWeapon.ThrowWeapon(animator.transform, 20f);
            inv.ResetWeapon();
            combat.IsThrowingWeapon = false;
            
        }

       

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        motor.StopMove = false;
        
        combat.IsThrowingWeapon = false;
        motor.BlockRotation = false;
        animator.applyRootMotion = false;
    }


}
