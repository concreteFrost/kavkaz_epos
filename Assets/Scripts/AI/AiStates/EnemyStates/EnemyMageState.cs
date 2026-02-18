using System.Collections;
using UnityEngine;

public class EnemyMageState : BaseEnemyAttackState
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;

    protected override void Init()
    {
        emitter = context.emitter;
        spellInventory = context.spellInventory;
    }

    protected override AIStateResult TrackCombatBehaviour(Transform target)
    {
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());

        if (!combatHandler.IsInAttackRange(distance))
        {
            motor.MoveCharacter(target.position);
            motor.SetStrafe(false);
            HandleDefense(true);
            return AIStateResult.None;
        }

       
        motor.SetStrafe(true);
        HandleDefense(false);

        return GetNextDecision();
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
        if(distance < 1f)
        {
            combatCoroutine = StartCoroutine(ComboCoroutine(punchesCount:1));
            return;
        }

        if (!fov.IsTargetVisible(fov.currentTarget))
        {
            motor.MoveCharacter(fov.currentTarget.GetOrigin().position);
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
        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();
    }

    protected override bool ShouldStopCooldown()
    {
        if (target == null)
            return true;

        if (distance < 1f)
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
