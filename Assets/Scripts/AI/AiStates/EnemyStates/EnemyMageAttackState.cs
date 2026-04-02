using System.Collections;
using UnityEngine;

public class EnemyMageAttackState : BaseEnemyAttackState
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    float meleeDistance = 1.3f;

    public override float AttackRangeDistance() => 10f;

    public override void Init()
    {
        emitter = context.emitter;
        spellInventory = context.spellInventory;
    }


    public override AIStateResult GetNextDecision()
    {
        if (distance > 10f)
        {
            HandleAttack(target);
            return AIStateResult.None;
        }

        return base.GetNextDecision();
    }


    public override void HandleAttack(Transform target)
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
        //пополняем запасы чтобы магия не истощалась 
        spellInventory.TopUpCurrentItem(1);

        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();
    }

    public override bool ShouldExitCooldown()
    {

        if (distance < meleeDistance)
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

