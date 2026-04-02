using System.Collections;
using UnityEngine;

public class EnemyMeleeAttackState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;

    public override float AttackRangeDistance() => 1.3f;
    public override void Init()
    {
        combatController = context.combat;
        weaponSetter = context.weaponSetter;
    }


    public override void HandleAttack(Transform target)
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

    public override void HandleDefense(bool willDefend)
    {
        combatHandler?.ToggleShield(willDefend, weaponSetter, combatController);
    }

 
    public override bool ShouldExitCooldown()
    {
        float dist = Vector3.Distance(self.position, target.position);

        if (target == null)
            return false;

        if (combatHandler.CanAttack() || dist >= 5f)
            return true;

        return false;
    }

    public override void HandleCooldown()
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
