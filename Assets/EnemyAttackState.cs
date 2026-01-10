using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyAttackState : AIState<EnemyBrainContext>
{
    //контроль комбо

    private float currCombatCooldown = 0f;
    private float maxCombatCooldown = 0f;
    private bool isComboRunning = false;
    private float comboDistanceOffset = 0.2f;
    private Coroutine comboCoroutine;

    //контроль доджа
    public float lastDamageTime = -10f;
    private int damageCounter = 0;
    private float dodgeCounterResetTimer = 5f;
    [SerializeField] private float currentDodgeChance = 0f;

    //цель
    private Transform target;
    float distance;

    HumanoidCharacterStatsSO stats;


    public override void Enter()
    {

        currCombatCooldown = 0f;
        maxCombatCooldown = 0;
        currentDodgeChance = 0f;

        stats = context.stats.statsSO as HumanoidCharacterStatsSO;

        context.damageController.DamageTaken += AdjustDodgeChance;
       
        isComboRunning = false;
        comboCoroutine = null;

        if (context.fov.currentTarget == null)
            return;

        var aimTarget = context.fov.currentTarget.GetAimTransform();

        context.fov.AssignTargetToMotor();

        target = context.fov.currentTarget.GetOrigin();

        
    }

    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        var self = context.self;

        //если нет цели то вернуться в первоначальное состояние
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        //если додж то игнорируем последующий код
        if (motor.IsDodging) return AIStateResult.None;

        //обновляем счётчик перерыва доджа
        UpdateDodgeCooldown();

        distance = Vector3.Distance(self.position, target.position);

        //бежать если дистанция далека
        motor.IsSprinting = distance > stats.distanceToRun;

        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);

        //если цель далека и недостежима то вернуться в первоначальное состояние
        if (!canReach && distance > stats.attackDistance) return AIStateResult.Wait;

        //если дистанция больше дистанции боя то преследовать цель
        if (distance > stats.maxCombatDistance) return AIStateResult.Chase;

        //игнорировать последующий код если идёт комбо
        if (isComboRunning) return AIStateResult.None;

        //если дистанция больше атаки то идём на цель
        //и игнорируем нижний блок кода
        if (distance > stats.attackDistance)
        {
            motor.MoveCharacter(target.position);
            return AIStateResult.None;
        }

        //отчитываем промежуток между атаками
        currCombatCooldown += Time.deltaTime;

        //принять решение в бою по достижению времени
        //if (currCombatCooldown >= maxCombatCooldown && comboCoroutine == null)
        if (comboCoroutine == null)
        {
            bool willAttack = Random.value > currentDodgeChance;
            comboCoroutine = StartCoroutine(CombatDecision(target,willAttack));
        }

        //иначе останавливаемся скорость
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

        context.damageController.DamageTaken -= AdjustDodgeChance;
    }

    private void Dodge(Transform targetPoint)
    {
        Vector3 fromTarget = (transform.position - targetPoint.position).normalized;
        context.motor.Dodge(fromTarget);
       
    }

    IEnumerator CombatDecision(Transform target, bool willAttack)
    {
        
        if (willAttack)
        {
            int punches = Random.Range(1, 5);
            yield return ComboSampleCoroutine(punches);
            
        }
        else
        {
            yield return DodgeCoroutine(target);
        }

        comboCoroutine = null;
        currCombatCooldown = 0f;
        maxCombatCooldown = Random.Range(0.5f, 1f);
        
    }

    IEnumerator DodgeCoroutine(Transform target)
    {
        context.motor.IsDodging = true;
        currentDodgeChance = 0f;
        damageCounter = 0;
       

        Dodge(target);

        while (context.motor.IsDodging)
            yield return null;
    }

    IEnumerator ComboSampleCoroutine(int punchesCount)
    {
        isComboRunning = true;

        int executedAttacks = 0;
        var combat = context.combat;

        void OnAttackEnd() => executedAttacks++;

        combat.OnAttackEnd += OnAttackEnd;

        combat.PerformAttack();

        while (executedAttacks < punchesCount - 1 && distance <= stats.attackDistance + comboDistanceOffset)
        {
            yield return new WaitForSeconds(combat.attackBufferTime * 0.9f);
            combat.PerformAttack();
        }

        combat.OnAttackEnd -= OnAttackEnd;
        isComboRunning = false;
    }

    public void AdjustDodgeChance(IAttackSource source)
    {
        lastDamageTime = Time.time;
        damageCounter++;

        // можно уменьшать шанс атаки/увеличивать шанс уклонения
        currentDodgeChance = damageCounter * stats.dodgeChanceMultiplier;
    }

    private void UpdateDodgeCooldown()
    {
        if (Time.time - lastDamageTime > dodgeCounterResetTimer) // сек без урона
        {
            damageCounter = 0;
            currentDodgeChance = 0f; // восстановление нормального поведения
        }
    }
}
