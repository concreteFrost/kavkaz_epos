using System.Collections;
using UnityEngine;

public class EnemyAttackState : BaseEnemyAttackState
{

    protected HumanoidCombatInventory inventory;

    protected override void Init()
    {
        combatController = context.combat;
        inventory = context.inventory;
    }

    protected override void HandleAttack(Transform target)
    {
        if (combatHandler.WillPowerAttack())
            combatCoroutine = StartCoroutine(PowerAttackCoroutine());
        else
            combatCoroutine = StartCoroutine(ComboCoroutine());
    }

    private IEnumerator PowerAttackCoroutine()
    {
        combatController.PerformPowerAttack();

        while (combatController.IsAttacking)
            yield return null;

        combatHandler.ResetPowerAttackChance();
        FinishCombatAction();
    }

    protected override void HandleDefense(bool willDefend)
    {
        combatHandler?.ToggleShield(willDefend, inventory, combatController);
    }

    protected override AIStateResult CantAttackResult()
    {
        combatHandler.UpdateCombatCooldown();
        motor.StopMovement();

        return AIStateResult.None;
    }
}
