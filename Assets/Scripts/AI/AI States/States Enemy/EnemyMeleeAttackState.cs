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
        if (usePowerAttack)
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
            int punchesCount = combatHandler.GetPunchesCount();

            combatCoroutine = combatActions.StartMelee(
                combatController,
                punchesCount,
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
        // Удаление цели и переходы состояний не отменяют оставшуюся перезарядку.
        return combatHandler.CanAttack();
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
