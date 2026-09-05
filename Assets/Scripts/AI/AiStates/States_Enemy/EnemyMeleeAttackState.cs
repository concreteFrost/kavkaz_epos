using System.Collections;
using UnityEngine;

public class EnemyMeleeAttackState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;

    public override void Init()
    {
        combatController = context.combat;
        weaponSetter = context.weaponSetter;
        combatMode = CombatMode.Melee;

        
    }


    public override void HandleAttack(Transform target)
    {
        if (combatHandler.WillPowerAttack())
        {
            combatCoroutine = combatActions.StartPowerAttack(
                combatController,
                combatHandler,
                FinishCombatAction
            );

            //чтобы не спамить голос
            TryPlayVoice(audioManager.PlayPowerAttack); 
        }
        else
        {
            int punchesCount = Random.Range(1, 5);

            combatCoroutine = combatActions.StartMelee(
                combatController,
                combatHandler,
                punchesCount,
                combatHandler.GetAttackDistance(combatMode),
                () => distanceToTarget,
                FinishCombatAction
            );

            audioManager.PlayPowerAttack();
        }
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
