using UnityEngine;

public class EnemyIdleState : AIState<EnemyBrainContext>
{

    private EnemyIdleHandler idleHandler;
    private EnemyCombatHandler combatHandler;
    private HumanoidAIMotor motor;
    private EnemyFOVController fov;
    private EnemyNotifierManager notifierManager;   

    public override void Enter()
    {

       
        motor = context.motor;
        fov = context.fov;  
        notifierManager = context.notifierManager;  
        idleHandler = context.stateTracker.idleHandler;
      
        combatHandler = context.stateTracker.combatHandler;

        // в idle всегда гарантированно гасим любое предыдущее движение
        motor.StopMovement();
        motor.ResetSprint();

        // сбрасываем цель Ч idle не удерживает агрессию
        fov.ResetLockedTarget();
        motor.ResetLockTarget();

        // инициализаци€ таймеров и флагов состо€ни€ поко€
        idleHandler.SetMaxIdleTime();
        idleHandler.ResetIdleState();

        //сбрасываем данные комбата
        combatHandler.ResetCombatState();

    }

    public override AIStateResult Run()
    {

        if (fov.currentTarget != null)
        {

            notifierManager.Notify(fov.currentTarget);
            return AIStateResult.Chase;
        }

        // ищем потенциальные цели
        fov.CheckTargets();


        idleHandler.UpdateCurrentIdleTime();

        if (idleHandler.HasIdleTimeFinished())
            return AIStateResult.Patrol;

        return AIStateResult.None;
    }

    public override void Exit()
    {
        idleHandler.ResetIdleState();
       
    }
}
