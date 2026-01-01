using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{


    IAttackSource inv;
    ICharacterCombatAnimData combatAnimData;
    IHumanoidMovement motor;

    bool hitActive = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<IAttackSource>();
        combatAnimData = animator.GetComponentInChildren<HumanoidCombatController>();
        motor = animator.GetComponent<IHumanoidMovement>();

        animator.applyRootMotion = true;
        hitActive = false;

        // блокируем вращение персонажа во время атаки
        motor.BlockRotation = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

      
        var weapon = inv.CurrentWeapon;
        var attack = weapon.CurrentAttack();

        if (attack == null) return;

        float t = stateInfo.normalizedTime % 1f;

        if (!hitActive && t >= attack.hitStartFrame)
        {
            weapon.PerformAttack();
            hitActive = true;
        }

        if (hitActive && t >= attack.hitEndFrame)
        {
            weapon.CancelAttack();
            hitActive = false;
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.speed = 1f;

        inv.CurrentWeapon.CancelAttack();

        animator.applyRootMotion = false;
        motor.BlockRotation = false;

        // уведомляем контроллер, что атака завершена
        combatAnimData.EndAttack();

        // проверяем очередь нажатий
        ((HumanoidCombatController)combatAnimData).TryStartNextAttackFromQueue();
    }
}
