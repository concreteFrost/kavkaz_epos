using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{

    IAttackSource inv;
    IHumanoidCombat combatAnimData;
    IHumanoidMovement motor;
    ICharacterStatsController statsModifier;

    bool hitActive = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<IAttackSource>();
        combatAnimData = animator.GetComponentInChildren<IHumanoidCombat>();
        motor = animator.GetComponent<IHumanoidMovement>();
        statsModifier = animator.GetComponentInChildren<ICharacterStatsController>();

        if(inv.CurrentWeapon != null)
        {
            statsModifier.ReduceStamina(inv.CurrentWeapon.CurrentAttack().staminaPenalty);
        }
        

        animator.applyRootMotion = true;
        hitActive = false;

        // блокируем вращение персонажа во время атаки
        motor.BlockRotation = true;
        motor.StopMove = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!animator.applyRootMotion) animator.applyRootMotion = true;

        var weapon = inv.CurrentWeapon;
        var attack = weapon.CurrentAttack();

        if (attack == null) return;

        float t = stateInfo.normalizedTime % 1f;

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
        motor.StopMove = false;

        inv.CurrentWeapon.CancelAttack();

        animator.applyRootMotion = false;
        motor.BlockRotation = false;

        // уведомляем контроллер, что атака завершена
        combatAnimData.EndAttack();

        // проверяем очередь нажатий
        //combatAnimData.TryStartNextAttackFromQueue();
    }
}
