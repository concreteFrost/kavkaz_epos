using UnityEngine;


public class EnemyMixedCombatState : BaseEnemyAttackState
{
    protected HumanoidWeaponSetter weaponSetter;
    IEmitter emitter;
    CharacterSpellInventory spellInventory;

    public override void Enter()
    {
        base.Enter();

        var currentTarget = context.fov.currentTarget;
        if (currentTarget != null)
            distanceToTarget = Vector3.Distance(context.self.position, currentTarget.GetOrigin().position);

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

            if (usePowerAttack)
            {       
                combatCoroutine = combatActions.StartPowerAttack(combatController, combatHandler, FinishCombatAction);
            }            
                
            else
            {
                int punchesCount = combatHandler.GetPunchesCount();
                combatCoroutine = combatActions.StartMelee(combatController, punchesCount, CanStartComboAttack, FinishCombatAction);
            }

            audioManager.PlayAttack();
            return;

        }

        if (combatMode == CombatMode.Magic)
        {
            if (!spellInventory.TrySelectRandomSpell())
            {
                // Без доступных заклинаний сближаемся для ближнего боя со следующего обновления.
                combatMode = CombatMode.Melee;
                return;
            }


            combatCoroutine = combatActions.StartSpell(emitter, spellInventory, FinishCombatAction);
        }

       
    }

    public override void HandleCooldown()
    {
        if (target == null || context.damageController.IsDamaged || motor.StopMove)
        {
            motor.StopStrafe();
            return;
        }

        // Во время паузы обходим цель; повторный запуск нужен только после завершения текущего отрезка.
        motor.SetStrafe(true);
        if (motor.strafeCoroutine == null)
            motor.StartStrafe(self, target);
    }

    protected override void FinishCooldown()
    {
        // Корутина движения принадлежит мотору, поэтому завершение паузы останавливает её явно.
        motor.StopStrafe();
        motor.StopMovement();
        base.FinishCooldown();
    }

    public override void Exit()
    {
        motor.StopStrafe();
        base.Exit();
    }

    public override void HandleDefense(bool willDefend)
    {
        combatHandler.ToggleShield(willDefend, weaponSetter, combatController);
    }

    public override bool ShouldExitCooldown()
    {
        return combatHandler.CanAttack();
    }

    protected override void FinishCombatAction()
    {
        base.FinishCombatAction();
        combatMode = combatHandler.DecideCombatMode(distanceToTarget);   
    }





}
