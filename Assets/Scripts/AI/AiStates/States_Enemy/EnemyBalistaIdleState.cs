using System.Collections;
using UnityEngine;

public class EnemyBalistaIdleState : AIState<EnemyBrainContext>
{
    private EnemyIdleHandler idleHandler;
    private EnemyCombatHandler combatHandler;
    private HumanoidAIMotor motor;
    private EnemyFOVController fov;

    public CatapultController catapult;

    Coroutine rotateCoroutine;

    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;
        idleHandler = context.stateTracker.idleHandler;

        combatHandler = context.stateTracker.combatHandler;


        // в idle всегда гарантированно гасим любое предыдущее движение
        motor.StopMovement();
        motor.ResetSprint();

        // сбрасываем цель — idle не удерживает агрессию
        fov.ResetLockedTarget();
        motor.ResetLockTarget();

        //сбрасываем данные комбата
        combatHandler.ResetCombatState();

        if (rotateCoroutine == null) 
        rotateCoroutine = StartCoroutine(RotateToDefaultDirection());
    }

    public override AIStateResult Run()
    {

        if (fov.currentTarget != null)
        {

            if (catapult == null || !catapult.IsTargetInLaunchRange(fov.currentTarget))
                return AIStateResult.Chase;
            

            if(catapult.CanLaunch())
                catapult.TryLaunch(fov.currentTarget);
            
            return AIStateResult.None;
        }

        // ищем потенциальные цели
        fov.CheckTargets();

      
        return AIStateResult.None;
    }

    public override void Exit()
    {
        idleHandler.ResetIdleState();
        rotateCoroutine = null;
    }

    IEnumerator RotateToDefaultDirection()
    {
        float elapsed = 0;

        while(elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            motor.RotateToDirection(context.permamentForward);
            yield return null;
        }

        rotateCoroutine = null;
    }

  
}
