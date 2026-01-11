using System.Collections;
using UnityEngine;

public class EnemyAttackState : AIState<EnemyBrainContext>
{
    // локальные параметры состояния
    [SerializeField] private float comboDistanceOffset = 0.2f;
    [SerializeField] private float dodgeCounterResetTimer = 5f;

    // runtime
    private Coroutine comboCoroutine;
    private Transform target;
    private float distance;

    EnemyStateTracker tracker;
    CharacterBehaviourStatsSO stats;



    public override void Enter()
    {
        tracker = context.stateTracker;
        tracker.ResetAttackState();

        stats = context.stateTracker.stats;

        context.damageController.DamageTaken += OnDamageTaken;

        comboCoroutine = null;

        if (context.fov.currentTarget == null)
            return;

        context.fov.AssignTargetToMotor();
        target = context.fov.currentTarget.GetOrigin();
    }

    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        var self = context.self;
        var tracker = context.stateTracker;

        // 1. цель потеряна
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        // 2. во время доджа ничего не делаем
        if (motor.IsDodging)
            return AIStateResult.None;

        // 3. обновление восстановления доджа
        tracker.UpdateDodgeCooldown(dodgeCounterResetTimer);

        // 4. дистанция
        distance = Vector3.Distance(self.position, target.position);

        motor.IsSprinting = distance > stats.distanceToRun;

        bool canReach = NavAgentUtils.HasCompletePath(
            self.position,
            target.position
        );

        // 5. цель недостижима
        if (!canReach && distance > stats.attackDistance)
            return AIStateResult.Wait;

        // 6. цель вышла из боевой дистанции
        if (distance > stats.maxCombatDistance)
            return AIStateResult.Chase;

        // 7. если идёт комбо — не вмешиваемся
        if (tracker.isComboRunning)
            return AIStateResult.None;

        // 8. подходим к цели
        if (distance > stats.attackDistance)
        {
            motor.MoveCharacter(target.position);
            return AIStateResult.None;
        }

        // 9. боевое решение
        tracker.UpdateCombatCooldown();

        if (comboCoroutine == null)
        {
            bool willAttack = Random.value > tracker.currentDodgeChance;
            comboCoroutine = StartCoroutine(
                CombatDecision(target, willAttack)
            );
        }
        else
        {
            motor.StopMovement();
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }

        context.damageController.DamageTaken -= OnDamageTaken;
    }

    // ===== Combat logic =====

    private IEnumerator CombatDecision(Transform target, bool willAttack)
    {
        if (willAttack)
        {
            int punches = Random.Range(1, 5);
            yield return ComboCoroutine(punches);
        }
        else
        {
            yield return DodgeCoroutine(target);
        }

        comboCoroutine = null;

        context.stateTracker.ResetCombatCooldown(0.5f, 1f);
    }

    private IEnumerator DodgeCoroutine(Transform target)
    {
        var motor = context.motor;
        var tracker = context.stateTracker;

        motor.IsDodging = true;

        tracker.damageCounter = 0;
        tracker.currentDodgeChance = 0f;

        Vector3 fromTarget =
            (context.self.position - target.position).normalized;

        motor.Dodge(fromTarget);

        while (motor.IsDodging)
            yield return null;
    }

    private IEnumerator ComboCoroutine(int punchesCount)
    {
        var tracker = context.stateTracker;
        var combat = context.combat;

        tracker.isComboRunning = true;

        int executedAttacks = 0;

        void OnAttackEnd() => executedAttacks++;

        combat.OnAttackEnd += OnAttackEnd;

        combat.PerformAttack();

        while (executedAttacks < punchesCount - 1 &&
               distance <= stats.attackDistance + comboDistanceOffset)
        {
            yield return new WaitForSeconds(
                combat.attackBufferTime * 0.9f
            );
            combat.PerformAttack();
        }

        combat.OnAttackEnd -= OnAttackEnd;

        tracker.isComboRunning = false;
    }

    // ===== Damage reaction =====

    private void OnDamageTaken(Transform source)
    {
        context.stateTracker.RegisterDamage(
            stats.dodgeChanceMultiplier
        );
    }
}
