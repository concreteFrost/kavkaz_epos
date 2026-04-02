using UnityEngine;
using System.Collections;



public class EnemyMixedCombatState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    
    public CombatMode currentMode;

    public override float AttackRangeDistance() => currentMode == CombatMode.Melee ? 1.3f : 10f;


    public override void Enter()
    {
        base.Enter();

        currentMode = combatHandler.DecideCombatMode(distance);
    }

    public override void Init()
    {

        weaponSetter = context.weaponSetter;
        emitter = context.emitter;
        spellInventory = context.spellInventory;

    }

    //protected override AIStateResult HandleCombatBehavior()
    //{
    //    // если режим еще не выбран
    //    return HandleCombatBehavior();
    //}

    public override void HandleAttack(Transform target)
    {
        
        if (currentMode == CombatMode.Melee)
        {

            if (combatHandler.WillPowerAttack())
                combatCoroutine = StartCoroutine(PowerAttackCoroutine());
            else
            {
                int punchesCount = Random.Range(1, 5);
                combatCoroutine = StartCoroutine(MeleeCoroutine(punchesCount));
            }
            return;

        }

        if (currentMode == CombatMode.Magic)
        {
         
            combatCoroutine = StartCoroutine(CastSpellCoroutine());
        }

       
    }

    public override void HandleCooldown()
    {
        motor.StopMovement();
    }

    public override void HandleDefense(bool willDefend)
    {
        combatHandler.ToggleShield(willDefend, weaponSetter, combatController);
    }

    public override bool ShouldExitCooldown()
    {
        float dist = Vector3.Distance(self.position, target.position);

        if (target == null)
            return false;

        if (combatHandler.CanAttack())
            return true;

        return false;
    }

    protected override void FinishCombatAction()
    {
        base.FinishCombatAction();
        currentMode = combatHandler.DecideCombatMode(distance);   
    }


    private IEnumerator PowerAttackCoroutine()
    {
        combatController.PerformPowerAttack();
        while (combatController.IsAttacking)
            yield return null;

        combatHandler.ResetPowerAttackChance();
        FinishCombatAction();
    }

    public override AIStateResult GetNextDecision()
    {

        switch (combatHandler.GetNextDecision())
        {
            case CombatTransition.Attack:
                HandleAttack(target);
                break;

            case CombatTransition.Dodge:
                combatCoroutine = StartCoroutine(DodgeCoroutine(target));
                break;

            case CombatTransition.Strafe:
                return AIStateResult.Strafe;
        }

        return AIStateResult.None;
    }

    IEnumerator CastSpellCoroutine()
    {
        spellInventory.TopUpCurrentItem(1);

        emitter.StartEmit();

        while (emitter.IsEmitting)
            yield return null;

        FinishCombatAction();
    }

   


}
