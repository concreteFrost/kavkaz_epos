using System.Collections;
using UnityEngine;

public class EnemyMageAttackState : BaseEnemyAttackState
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;


    public override void Init()
    {
        emitter = context.emitter;
        spellInventory = context.spellInventory;
        combatMode = CombatMode.Magic;
    }


    public override AIStateResult GetNextDecision()
    {
        if (distanceToTarget > 10f)
        {
            HandleAttack(target);
            return AIStateResult.None;
        }

        return base.GetNextDecision();
    }


    public override void HandleAttack(Transform target)
    {
       
        if (IsFirstMeleeAttackInRange())
        {
          
            int punchesCount = combatHandler.GetPunchesCount();
            combatCoroutine = combatActions.StartMelee(combatController, punchesCount, CanStartComboAttack, FinishCombatAction);
            TryPlayVoice(audioManager.PlayAttack);
            return;
        }

        if (!spellInventory.TrySelectRandomSpell())
        {
            Debug.Log("no spell assigned");
            return;
        }

        //audioManager.PlayAttack();
        combatCoroutine = combatActions.StartSpell(emitter, spellInventory, FinishCombatAction);
    }

  

    public override bool ShouldExitCooldown()
    {

        if (IsFirstMeleeAttackInRange())
            return true;

        if (!fov.IsTargetVisible())
            return true;
        if (combatHandler.CanAttack())
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

    public override void HandleDefense(bool willDefend)
    {
        //
    }
}

