using UnityEngine;

public class EnemyInterruptionState : AIState<EnemyBrainContext>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EnemyIdleHandler idleHandler;
    private HumanoidAIMotor motor;
    private EnemyFOVController fov;
    private EnemyNotifierManager notifierManager;
    private Animator anim;

    float currInterruptionTimer = 0;
    float interruptionMaxTimer = 5f;
    public override void Enter()
    {
        motor = context.motor;
        fov = context.fov;
        notifierManager = context.notifierManager;
        idleHandler = context.stateTracker.idleHandler;

        // в idle всегда гарантированно гасим любое предыдущее движение
        motor.StopMovement();
        motor.ResetSprint();

        // сбрасываем цель — idle не удерживает агрессию
        fov.ResetLockedTarget();
        motor.ResetLockTarget();

        anim = context.animator;


       motor.IsTurning = true;
       anim.CrossFade(AnimatorParameters.lookAroundState, 0, 0);

    }

    public override AIStateResult Run()
    {
        currInterruptionTimer+= Time.deltaTime; 

        if (fov.currentTarget != null)
        {

            notifierManager.Notify(fov.currentTarget);
            return AIStateResult.Chase;
        }

        // ищем потенциальные цели
        fov.CheckTargets();


        if(currInterruptionTimer >= interruptionMaxTimer)
        {
            anim.SetBool(AnimatorParameters.IsTurning, false);
            return AIStateResult.Idle;
        }

        return AIStateResult.None;
    }

    public override void Exit()
    {
        idleHandler.ResetIdleState();
        motor.IsTurning = false;
        currInterruptionTimer = 0f;
    }
}
