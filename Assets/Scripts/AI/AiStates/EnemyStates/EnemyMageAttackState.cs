using System.Collections;
using Unity.VisualScripting;
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
       
        if (!fov.IsTargetVisible(fov.currentTarget))
        {
            motor.MoveCharacter(fov.currentTarget.GetOrigin().position);
            return;
        }

        if (distance < meleeDistance)
        {
            combatCoroutine = StartCoroutine(MeleeCoroutine(punchesCount:1));
            return;
        }

        if (spellInventory.CurrentSpell == null)
        {
            combatCoroutine = null;
            return;
        }

        combatCoroutine = StartCoroutine(AttackCoroutine());
    }

  
    IEnumerator AttackCoroutine()
    {
        if(spellInventory.CurrentSpell.quantity <= 1)
        {
            spellInventory.TopUpCurrentSpell(20);
        }

        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();
    }

    protected override bool ShouldStopCooldown()
    {
        if (target == null)
            return true;

        if (distance < meleeDistance)
            return true;

        if (!fov.IsTargetVisible(fov.currentTarget))
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

