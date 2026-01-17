public class EnemyIdleState : AIState<EnemyBrainContext>
{

    private EnemyIdleHandler idleTracker;
    private EnemyPassiveInterruptionHandler interruptionTracker;

    public override void Enter()
    {
      
        idleTracker = context.stateTracker.idleHandler;
        interruptionTracker = context.stateTracker.interruptionTracker;

        // в idle всегда гарантированно гасим любое предыдущее движение
        context.motor.StopMovement();
        context.motor.ResetSprint();

        // сбрасываем цель Ч idle не удерживает агрессию
        context.fov.ResetTarget();

        // инициализаци€ таймеров и флагов состо€ни€ поко€
        idleTracker.SetMaxIdleTime();
        idleTracker.ResetIdleState();
        interruptionTracker.ResetInterruption();
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


        idleTracker.UpdateCurrentIdleTime();

        if (idleTracker.HasIdleTimeFinished())
            return AIStateResult.Patrol;

        return AIStateResult.None;
    }

    public override void Exit()
    {
        idleTracker.ResetIdleState();
    }
}
