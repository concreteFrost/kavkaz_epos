using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{

    ICombatInventory inv;
    IHumanoidCombat combatAnimData;
    IHumanoidMovement motor;
    ICharacterStatsController statsModifier;
    IDamagable damageController;
    WeaponAttack attack;
    IWeapon weapon;
    IPushable pushable;
   
    bool hitActive = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<ICombatInventory>();
        combatAnimData = animator.GetComponentInChildren<IHumanoidCombat>();
        motor = animator.GetComponent<IHumanoidMovement>();
        statsModifier = animator.GetComponentInChildren<ICharacterStatsController>();
        damageController = animator.GetComponentInChildren<IDamagable>();
        pushable = animator.GetComponentInChildren<IPushable>();

        if(inv.CurrentWeapon != null)
        {
            statsModifier.ReduceStamina(inv.CurrentWeapon.CurrentAttack().staminaPenalty);
        }
        
        animator.applyRootMotion = true;
        hitActive = false;

        // блокируем вращение персонажа во время атаки
        motor.BlockRotation = true;
        motor.StopMove = true;

        weapon = inv.CurrentWeapon;
        attack = weapon.CurrentAttack();
       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!animator.applyRootMotion) animator.applyRootMotion = true;
     
        if (damageController.IsDamaged || pushable.IsPushed) return;

        if (attack == null) return;

        //animator.speed = attack.animationInfo.animationSpeed;

        float t = stateInfo.normalizedTime;

        if (!hitActive && t >= attack.animationInfo.hitStartFrame)
        {
            weapon.PerformAttack();
            hitActive = true;
        }

        if (hitActive && t >= attack.animationInfo.hitEndFrame)
        {
            weapon.CancelAttack();
            hitActive = false;
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;

        hitActive = false;
        motor.StopMove = false;

        inv.CurrentWeapon.CancelAttack();

        animator.applyRootMotion = false;
        motor.BlockRotation = false;

        // уведомляем контроллер, что атака завершена
        combatAnimData.EndAttack();

    }
}
