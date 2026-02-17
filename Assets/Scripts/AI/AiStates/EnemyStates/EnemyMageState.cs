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

    protected override void HandleAttack(Transform target)
    {
        if (!fov.IsTargetVisible(fov.currentTarget.GetAimTransform()))
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

    protected override void HandleDefense(bool willDefend)
    {
        //
    }

    IEnumerator AttackCoroutine()
    {
        emitter.StartEmit();

        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();

    }

    protected override AIStateResult CantAttackResult()
    {
        
        return AIStateResult.Strafe;
    }

  
}
