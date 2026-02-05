using UnityEngine;

public class WeaponThrowBehavior : StateMachineBehaviour
{

    ICombatInventory inv;
    IHumanoidMovement motor;
    IDamagable damagable;   
    bool weaponThrowed = false;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        motor = animator.GetComponent<IHumanoidMovement>();
        inv = animator.GetComponentInChildren<ICombatInventory>();
        damagable = animator.GetComponentInChildren<IDamagable>();

        animator.applyRootMotion = true;
        motor.BlockRotation = true;
        motor.StopMove = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (!animator.applyRootMotion) animator.applyRootMotion = true;

        if (damagable.IsDamaged) return;

        float t = stateInfo.normalizedTime;

        if (t >= 0.4f && !weaponThrowed)
        {

            inv.CurrentWeapon.ThrowWeapon(animator.transform, 20f);
            inv.ResetWeapon();
          
            weaponThrowed = true;
            
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponThrowed = false;
        motor.StopMove = false;
        motor.BlockRotation = false;
        animator.applyRootMotion = false;
    }


}
