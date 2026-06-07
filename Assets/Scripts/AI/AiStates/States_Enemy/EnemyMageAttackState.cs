using System.Collections;
using UnityEngine;

public class EnemyMageAttackState : BaseEnemyAttackState
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    float meleeDistance = 1.3f;
   

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
       
        if (distanceToTarget < meleeDistance)
        {
            int punchesSount = 1;
            combatCoroutine = combatActions.StartMelee(combatController, combatHandler,punchesSount, combatHandler.GetAttackDistance(combatMode),()=> distanceToTarget, FinishCombatAction);
            return;
        }

        if(spellInventory.CurrentItem == null)
        {
            Debug.Log("no spell assigned");
            return;
        }

        combatCoroutine = combatActions.StartSpell(emitter, spellInventory, FinishCombatAction);
    }

  

    public override bool ShouldExitCooldown()
    {

        if (distanceToTarget < meleeDistance)
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

        if (dist > 7f)
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

