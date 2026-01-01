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
        float t = stateInfo.normalizedTime % 1f;

        // включаем/выключаем хитбокс
        if (!hitActive && t >= combatAnimData.CurrentAttack().hitStartFrame)
        {
            inv.CurrentWeapon.PerformAttack(); // активируем хитбокс
            hitActive = true;
        }

        if (hitActive && t >= combatAnimData.CurrentAttack().hitEndFrame)
        {
            inv.CurrentWeapon.CancelAttack(); // деактивируем хитбокс
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
