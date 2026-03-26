using System.Collections;
using UnityEngine;

public class EnemyMeleeAttackState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;

    protected override void Init()
    {
        combatController = context.combat;
        weaponSetter = context.weaponSetter;
    }


    protected override void HandleAttack(Transform target)
    {
        if (combatHandler.WillPowerAttack())
            combatCoroutine = StartCoroutine(PowerAttackCoroutine());
        else
        {
            int punchesCount = Random.Range(1, 5);
            combatCoroutine = StartCoroutine(MeleeCoroutine(punchesCount));
        }
            
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
        combatHandler?.ToggleShield(willDefend, weaponSetter, combatController);
    }

 
    protected override bool ShouldExitCooldown()
    {
        float dist = Vector3.Distance(self.position, target.position);

        if (target == null)
            return false;

        if (combatHandler.CanAttack() || dist >= 5f)
            return true;

        return false;
    }

    protected override void HandleCooldown()
    {
        float dist = Vector3.Distance(self.position, target.position);

        if (dist > 5f)
        {
            motor.StopMovement();
            return;
        }

        Vector3 dir = (self.position - target.position).normalized;
        motor.MoveLocal(dir);
    }
}
