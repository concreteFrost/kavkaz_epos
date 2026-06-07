using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HumanoidAICombatActions 
{

    private readonly MonoBehaviour ctx;

    public HumanoidAICombatActions(MonoBehaviour ctx)
    {
        this.ctx = ctx;
        
    }
    // =========================
    // MELEE
    // =========================
    public Coroutine StartMelee(
        IHumanoidMeleeCombat combat,
        EnemyCombatHandler combatHandler,
        int punchesCount,
        float attackRange,
        Func<float> distanceToTarget,
        Action onFinish)
    {
        return ctx.StartCoroutine(MeleeCoroutine(
            combat,
            combatHandler,
            punchesCount,
            attackRange,
            distanceToTarget,
            onFinish));
    }

    private IEnumerator MeleeCoroutine(
     IHumanoidMeleeCombat combat,
     EnemyCombatHandler combatHandler,
     int punchesCount,
     float attackRange,
     Func<float> getDistance,
     Action onFinish)
    {
        int executed = 0;
        void OnAttackEnd() => executed++;

        combat.OnAttackEnd += OnAttackEnd;

        try
        {
            combat.PerformAttack();

            while (executed < punchesCount - 1 &&
                   getDistance() <= combatHandler.GetMeleeAttackDistanceWithOffset())
            {
                yield return new WaitForSeconds(combat.AttackBufferTime * 0.9f);
                combat.PerformAttack();
            }
        }
        finally
        {
            combat.OnAttackEnd -= OnAttackEnd;
        }

        onFinish?.Invoke();
    }
    // =========================
    // POWER ATTACK
    // =========================
    public Coroutine StartPowerAttack(
        IHumanoidMeleeCombat combat,
        EnemyCombatHandler combatHandler,
        Action onFinish)
    {
        return ctx.StartCoroutine(PowerAttackCoroutine(combat, combatHandler, onFinish));
    }

    private IEnumerator PowerAttackCoroutine(
        IHumanoidMeleeCombat combat,
        EnemyCombatHandler combatHandler,
        Action onFinish)
    {
        combat.PerformPowerAttack();

        while (combat.IsAttacking)
            yield return null;

        combatHandler.ResetPowerAttackChance();

        onFinish?.Invoke();
    }

    // =========================
    // DODGE
    // =========================
    public Coroutine StartDodge(
        HumanoidAIMotor motor,
        Transform self,
        Transform target,
        EnemyCombatHandler combatHandler,
        Action onFinish)
    {
        return ctx.StartCoroutine(DodgeCoroutine(motor, self, target, combatHandler, onFinish));
    }

    private IEnumerator DodgeCoroutine(
        HumanoidAIMotor motor,
        Transform self,
        Transform target,
        EnemyCombatHandler combatHandler,
        Action onFinish)
    {
        motor.IsDodging = true;
        combatHandler.ResetDodgeChance();

        Vector3 dir = (self.position - target.position).normalized;
        motor.Dodge(dir);

        while (motor.IsDodging)
            yield return null;

        onFinish?.Invoke();
    }

    // =========================
    // SPELL
    // =========================
    public Coroutine StartSpell(
        IEmitter emitter,
        CharacterSpellInventory inventory,
        Action onFinish)
    {
        return ctx.StartCoroutine(SpellCoroutine(emitter, inventory, onFinish));
    }

    private IEnumerator SpellCoroutine(
        IEmitter emitter,
        CharacterSpellInventory inventory,
        Action onFinish)
    {
        inventory.TopUpCurrentItem(1);

        emitter.StartEmit();

        while (emitter.IsEmitting)
            yield return null;

        onFinish?.Invoke();
    }
}


