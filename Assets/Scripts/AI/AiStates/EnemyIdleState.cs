using System.Collections;
using UnityEngine;

public class EnemyIdleState : AIState<EnemyBrainContext>
{

    EnemyStateTracker stateTracker;

    Coroutine interruptedCoroutine;


    public override void Enter()
    {

        stateTracker = context.stateTracker;

        interruptedCoroutine = null;

        context.motor.StopMovement();
        context.motor.ResetSprint();
        context.fov.ResetTarget();

        stateTracker.SetMaxIdleTime();
        stateTracker.ResetIdleState();

        context.damageController.DamageTaken += OnDamageTaken;

    }

    public override AIStateResult Run()
    {

        //проверям цели в любом случае
        context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
        {
            return AIStateResult.Chase;
        }

        if (interruptedCoroutine != null) return AIStateResult.None;


        stateTracker.UpdateCurrentIdleTime();

        if (stateTracker.currIdleTime > stateTracker.maxIdleTime)
        {

            return AIStateResult.Patrol;
        }

        return AIStateResult.None;
    }


    public override void Exit()
    {
        context.damageController.DamageTaken -= OnDamageTaken;
        interruptedCoroutine = null;
    }

    private void OnDamageTaken(Transform attackSource = null)
    {

        if (attackSource == null) return;

        interruptedCoroutine = StartCoroutine(TurnCoroutine(attackSource.position));

    }

    private IEnumerator TurnCoroutine(Vector3 dir)
    {
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            context.motor.RotateToTarget(dir);

            yield return null;
        }

        interruptedCoroutine = null;
    }

}
