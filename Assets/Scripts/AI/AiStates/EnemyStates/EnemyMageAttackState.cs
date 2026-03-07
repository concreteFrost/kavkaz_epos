using System.Collections;
using UnityEngine;

public class EnemyMageAttackState : BaseEnemyAttackState
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    float meleeDistance = 1.3f;

    protected override void Init()
    {
        emitter = context.emitter;
        spellInventory = context.spellInventory;
    }

    protected override AIStateResult GetNextDecision()
    {
        if (distance > 10f)
        {
            HandleAttack(target);
            return AIStateResult.None;
        }

        return base.GetNextDecision();
    }


    protected override void HandleAttack(Transform target)
    {
       
        if (distance < meleeDistance)
        {
            combatCoroutine = StartCoroutine(MeleeCoroutine(punchesCount:1));
            return;
        }

        if(spellInventory.CurrentItem == null)
        {
            Debug.Log("no spell assigned");
            return;
        }

        combatCoroutine = StartCoroutine(AttackCoroutine());
    }

  
    IEnumerator AttackCoroutine()
    {
        if(spellInventory.CurrentItem.quantity <= 1)
        {
            spellInventory.TopUpCurrentItem(20);
        }

        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();
    }

    protected override bool ShouldExitCooldown()
    {

        if (distance < meleeDistance)
            return true;

        if (!fov.IsTargetVisible())
            return true;
        if (combatHandler.CanAttack())
            return true;

        return false;
    }

    protected override void HandleCooldown()
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

    protected override void HandleDefense(bool willDefend)
    {
        //
    }
}

