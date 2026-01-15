
public class EnemyIdleState : AIState<EnemyBrainContext>
{

    EnemyStateTracker stateTracker;

    public override void Enter()
    {

        stateTracker = context.stateTracker;

        context.motor.StopMovement();
        context.motor.ResetSprint();
        context.fov.ResetTarget();

        stateTracker.SetMaxIdleTime();
        stateTracker.ResetIdleState();
        stateTracker.ResetInterruption();

    }

    public override AIStateResult Run()
    {

        //проверям цели в любом случае
        context.fov.CheckTargets();

        if (context.fov.currentTarget != null)
        {
            return AIStateResult.Chase;
        }

        //поворачиваемся в сторону цели если получили урон
        if (stateTracker.IsInterrupted())
        {
            context.motor.RotateToTarget(context.stateTracker.interruptionDir);
            stateTracker.UpdateInterruption();
            return AIStateResult.None;
        }

        stateTracker.UpdateCurrentIdleTime();

        if (stateTracker.currIdleTime > stateTracker.maxIdleTime)
        {

            return AIStateResult.Patrol;
        }

        return AIStateResult.None;
    }


    public override void Exit()
    {
        stateTracker.ResetIdleState();

    }



}
