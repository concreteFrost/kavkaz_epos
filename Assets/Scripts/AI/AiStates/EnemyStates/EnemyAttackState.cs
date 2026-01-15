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
    EnemyFOVController fov;
    CharacterBehaviourStatsSO stats;
    HumanoidAIMotor motor;


    public override void Enter()
    {

        fov = context.fov;
        tracker = context.stateTracker;
        stats = context.stateTracker.stats;
        motor = context.motor;

        tracker.ResetAttackState();
        comboCoroutine = null;

        if (context.fov.currentTarget == null)
            return;

        target = context.fov.currentTarget.GetOrigin();
    }

    public override AIStateResult Run()
    {
       
        var self = context.self;

        //цель потеряна
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        //во время доджа ничего не делаем
        if (motor.IsDodging)
            return AIStateResult.None;

        //обновление восстановления доджа
        tracker.UpdateDodgeCooldown(dodgeCounterResetTimer);

        //дистанция
        distance = Vector3.Distance(self.position, target.position);

        //изменения поведения вращения к цели
     

        motor.IsSprinting = distance > stats.distanceToRun;

        bool canReach = NavAgentUtils.HasCompletePath(
            self.position,
            target.position
        );

        //цель недостижима
        if (!canReach && distance > stats.attackDistance)
            return AIStateResult.Wait;

        //цель вышла из боевой дистанции
        if (distance > stats.maxCombatDistance)
            return AIStateResult.Chase;

        //если идёт комбо — не вмешиваемся
        if (tracker.isComboRunning)
            return AIStateResult.None;

        //подходим к цели
        if (distance > stats.attackDistance)
        {
            motor.MoveCharacter(target.position);
            return AIStateResult.None;
        }

        // 9. боевое решение
        tracker.UpdateCombatCooldown();

        if (comboCoroutine == null)
        {
            motor.SetLockTarget(fov.currentTarget.GetAimTransform());
            bool willAttack = Random.value > tracker.currentDodgeChance;
            comboCoroutine = StartCoroutine(
                CombatDecision(target, willAttack)
            );
        }
        else
        {
            motor.ResetLockTarget();
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

        motor.ResetLockTarget();

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

   
}
