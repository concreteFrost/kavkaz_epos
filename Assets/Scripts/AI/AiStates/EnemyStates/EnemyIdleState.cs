public class EnemyIdleState : AIState<EnemyBrainContext>
{
    private EnemyStateTracker stateTracker;

    public override void Enter()
    {
        stateTracker = context.stateTracker;

        // в idle всегда гарантированно гасим любое предыдущее движение
        context.motor.StopMovement();
        context.motor.ResetSprint();

        // сбрасываем цель Ч idle не удерживает агрессию
        context.fov.ResetTarget();

        // инициализаци€ таймеров и флагов состо€ни€ поко€
        stateTracker.SetMaxIdleTime();
        stateTracker.ResetIdleState();
        stateTracker.ResetInterruption();
    }

    public override AIStateResult Run()
    {
        // ищем потенциальные цели
        context.fov.CheckTargets();

        // переходим в погоню если цель найдена
        if (context.fov.currentTarget != null)
            return AIStateResult.Chase;

        // реакци€ на полученный урон без смены состо€ни€
        if (stateTracker.IsInterrupted())
        {
            context.motor.RotateToTarget(stateTracker.GetInterruptionDirection());
            stateTracker.UpdateInterruption();
            return AIStateResult.None;
        }

        
        stateTracker.UpdateCurrentIdleTime();

        if (stateTracker.HasIdleTimeFinished())
            return AIStateResult.Patrol;

        return AIStateResult.None;
    }

    public override void Exit()
    {
        stateTracker.ResetIdleState();
    }
}
