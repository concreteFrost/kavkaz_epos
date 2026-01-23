public class EnemyIdleState : AIState<EnemyBrainContext>
{

    private EnemyIdleHandler idleHandler;
    private EnemyCombatHandler combatHandler;
    private EnemyPassiveInterruptionHandler interruptionTracker;

    public override void Enter()
    {
      
        idleHandler = context.stateTracker.idleHandler;
        interruptionTracker = context.stateTracker.interruptionTracker;

        combatHandler = context.stateTracker.combatHandler;

        // в idle всегда гарантированно гасим любое предыдущее движение
        context.motor.StopMovement();
        context.motor.ResetSprint();

        // сбрасываем цель Ч idle не удерживает агрессию
        context.fov.ResetTarget();
        context.motor.ResetLockTarget();

        // инициализаци€ таймеров и флагов состо€ни€ поко€
        idleHandler.SetMaxIdleTime();
        idleHandler.ResetIdleState();
        interruptionTracker.ResetInterruption();

        //сбрасываем данные комбата
        combatHandler.ResetCombatState();   
    }

    public override AIStateResult Run()
    {
        // ищем потенциальные цели
        context.fov.CheckTargets();

        // переходим в погоню если цель найдена
        if (context.fov.currentTarget != null)
            return AIStateResult.Chase;

        // реакци€ на полученный урон без смены состо€ни€
        if (interruptionTracker.IsInterrupted())
        {
            context.motor.RotateToTarget(interruptionTracker.GetInterruptionDirection());
            interruptionTracker.UpdateInterruption();
            return AIStateResult.None;
        }


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
