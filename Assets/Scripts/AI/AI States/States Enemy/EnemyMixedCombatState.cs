using UnityEngine;


public class EnemyMixedCombatState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;
    IEmitter emitter;
    CharacterSpellInventory spellInventory;

    public override void Enter()
    {
        base.Enter();

        combatMode = combatHandler.DecideCombatMode(distanceToTarget);
    }

    public override void Init()
    {

        weaponSetter = context.weaponSetter;
        emitter = context.emitter;
        spellInventory = context.spellInventory;

    }

    public override void HandleAttack(Transform target)
    {
        
        if (combatMode == CombatMode.Melee)
        {

            if (combatHandler.WillPowerAttack())
                combatCoroutine = combatActions.StartPowerAttack(combatController, combatHandler, FinishCombatAction);
            else
            {
                int punchesCount = Random.Range(1, 5);
                combatCoroutine = combatActions.StartMelee(combatController, combatHandler, punchesCount, combatHandler.GetAttackDistance(combatMode), ()=>distanceToTarget, FinishCombatAction);
            }
            return;

        }

        if (combatMode == CombatMode.Magic)
        {

            combatCoroutine = combatActions.StartSpell(emitter, spellInventory, FinishCombatAction);
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
        combatMode = combatHandler.DecideCombatMode(distanceToTarget);   
    }





}
