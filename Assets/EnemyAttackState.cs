using System.Collections;
using UnityEngine;

public class EnemyAttackState : AIState<EnemyBrainContext>
{

    [SerializeField] private float maxCombatDistance = 8f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float currCombatCooldown = 0f;
    [SerializeField] private float maxCombatCooldown = 0f;
    [SerializeField] private bool isComboRunning = false;
 
    Transform target;
    private Coroutine comboCoroutine;

    public override void Enter()
    {

        maxCombatCooldown = 0;
        currCombatCooldown = 0;

        isComboRunning = false;

        if (context.fov.currentTarget == null) return;

        Transform aimTarget = context.fov.currentTarget.GetAimTransform();
       
        context.motor.SetLockTarget(aimTarget);

        Transform targetToChase = context.fov.currentTarget.GetOrigin();
        target = targetToChase;
    }



    public override AIStateResult Run()
    {
        var fov = context.fov;
        var motor = context.motor;
        Transform self = context.self;

        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        target = fov.currentTarget.GetOrigin();

        float distance = Vector3.Distance(self.position, target.position);

        if (distance > maxCombatDistance)
            return AIStateResult.Chase;

        if (isComboRunning)
            return AIStateResult.None;

        if (distance > attackDistance)
        {
            motor.MoveCharacter(target.position);
            return AIStateResult.None;
        }

        currCombatCooldown += Time.deltaTime;

        if (currCombatCooldown >= maxCombatCooldown && comboCoroutine == null)
        {
            comboCoroutine = StartCoroutine(CombatDecision(target));
        }
        else if (comboCoroutine == null && !motor.isDodging)
        {
            motor.ResetSpeed();
        }


        motor.ResetSpeed();
        return AIStateResult.None;
    }



    public override void Exit()
    {
        if(comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);  

            comboCoroutine = null;  
        }
    }

    private void Dodge(Transform targetPoint)
    {
        Vector3 fromTarget = (transform.position - targetPoint.position).normalized;
        context.motor.Dodge(fromTarget);

    }

    IEnumerator CombatDecision(Transform target)
    {
        bool willDodge = Random.value > 0.4f;

        if (willDodge)
        {
            yield return StartCoroutine(DodgeCoroutine(target));
        }
        else
        {
            int punches = Random.Range(1, 4);
            yield return StartCoroutine(ComboSampleCoroutine(punches));
        }
    }

    IEnumerator DodgeCoroutine(Transform target)
    {
        context.motor.IsDodging = true;

        Dodge(target);
        while (context.motor.isDodging)
        {
            yield return null;
        }
    }

    IEnumerator ComboSampleCoroutine(int punchesCount)
    {
        isComboRunning = true;
        
        int executedAttacks = 0;

        var combat = context.combat;

        void OnAttackEnd()
        {
            executedAttacks++;
        }

        combat.OnAttackEnd += OnAttackEnd;

        // первый инпут
        combat.PerformAttack();

        while (executedAttacks < punchesCount - 1)
        {
            // ждём окно буфера
            yield return new WaitForSeconds(combat.attackBufferTime * 0.9f);
            combat.PerformAttack();
        }

        combat.OnAttackEnd -= OnAttackEnd;
        isComboRunning = false;
        comboCoroutine = null;

        currCombatCooldown = 0;
    
        maxCombatCooldown = Random.Range(2, 4);
    }


}
