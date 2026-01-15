using System.Collections;
using UnityEngine;

public class EnemyStrafeState : AIState<EnemyBrainContext>
{
    Transform chaseTarget;
    EnemyStateTracker tracker;
    CharacterBehaviourStatsSO stats;
    EnemyFOVController fov;
    HumanoidAIMotor motor;

    Coroutine strafeCoroutine;

    public Vector3 strafeDir;


    public override void Enter()
    {
        fov = context.fov;
        tracker = context.stateTracker;
        stats = tracker.stats;
        motor = context.motor;

        fov.ToggleLockState(true);
        motor.StopMovement();
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());
        motor.SetStrafe(true);
    }

    public override AIStateResult Run()
    {


        if (fov.currentTarget == null)
            return AIStateResult.Idle;


        if (strafeCoroutine == null)
        {
            strafeCoroutine = StartCoroutine(StrafeCoroutine());

            return AIStateResult.None;
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();

        if (strafeCoroutine != null)
        {
            StopCoroutine(strafeCoroutine);
            strafeCoroutine = null;
        }

        motor.ResetLockTarget();
        motor.SetStrafe(false);
        fov.ToggleLockState(false);

    }

    IEnumerator StrafeCoroutine()
    {
        bool isRight = Random.value > 0.5f;

        float elapsed = 0f;
        float maxStrafeTime = 3f;

        while (elapsed < maxStrafeTime && fov.currentTarget != null)
        {
            Vector3 selfPos = context.self.position;
            Vector3 targetPos = fov.currentTarget.GetOrigin().position;

            // направление НА цель
            Vector3 toTarget = (targetPos - selfPos).normalized;

            // перпендикуляр в плоскости XZ
            Vector3 strafeDir = Vector3.Cross(Vector3.up, toTarget).normalized;

            // выбор стороны
            Vector3 finalDir = isRight ? strafeDir : -strafeDir;

            // локальное движение без pathfinding
            motor.MoveLocal(finalDir);

            elapsed += Time.deltaTime;
            yield return null;
        }

        strafeCoroutine = null;
    }
}
