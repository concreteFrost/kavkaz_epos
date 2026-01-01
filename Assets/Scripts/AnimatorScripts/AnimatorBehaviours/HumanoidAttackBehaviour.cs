using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{
    public float hitStart = 0.3f;
    public float hitEnd = 0.6f;

    IAttackSource inv;
    ICharacterStatsModifier stats;
    HumanoidCombatController combatAnimData;
    IHumanoidMovement motor;

    bool hitActive = false;
    bool recoveryTriggered = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<IAttackSource>();
        //stats = animator.GetComponentInChildren<ICharacterStatsModifier>();
        combatAnimData = animator.GetComponentInChildren<HumanoidCombatController>();
        motor = animator.GetComponent<IHumanoidMovement>();

        //stats.ReduceStamina(inv.CurrentWeapon.GetCurrentAttack().staminaPenalty);

        animator.applyRootMotion = true;
        
        hitActive = false;
        recoveryTriggered = false;

        combatAnimData.StartAttack();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!motor.BlockRotation) motor.BlockRotation = true;
        if (!animator.applyRootMotion) animator.applyRootMotion = true;

        float t = stateInfo.normalizedTime;

        // включаем hitbox
        if (!hitActive && t >= hitStart)
        {
            inv.CurrentWeapon.PerformAttack();
            hitActive = true;
        }

        // отключаем hitbox
        if (hitActive && t >= hitEnd)
        {
            inv.CurrentWeapon.CancelAttack();
            hitActive = false;
        }

        // открываем recovery window
        if (!recoveryTriggered && t >= 0.7f)
        {
            combatAnimData.EndAttack();
            recoveryTriggered = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv.CurrentWeapon.CancelAttack();
        motor.BlockRotation = false;
        animator.applyRootMotion = false;
        hitActive = false;
        recoveryTriggered = false;
    }
}
