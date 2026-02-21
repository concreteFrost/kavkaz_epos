using UnityEngine;
using System.Collections;

public class EnemyMageWaitState : AIState<EnemyBrainContext>
{
    EnemyWaitForTargetHandler handler;
    CharacterSpellInventory spellInventory;
    IEmitter emitter;   
    EnemyFOVController fov;
    HumanoidAIMotor motor;

    Coroutine castCoroutine = null;

    public override void Enter()
    {
        handler = context.stateTracker.waitForTargetHandler;
        fov = context.fov;
        motor = context.motor;
        spellInventory = context.spellInventory;    
        emitter = context.emitter;  

        handler.ResetWaitState();

        motor.StopMovement();
        motor.SetStrafe(true);
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());
    }

    public override AIStateResult Run()
    {

        Transform self = context.self;

        // 1. нет цели
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        if (castCoroutine != null)
            return AIStateResult.None;


        Transform target = fov.currentTarget.GetOrigin();

        bool canReach = NavAgentUtils.HasCompletePath(self.position, target.position);

        if (fov.IsTargetVisible())
        {

            if (canReach)
            {
                return AIStateResult.Chase;
            }

            if (handler.CanAttack())
            {
                castCoroutine = StartCoroutine(CastCroutine());
                handler.ResetWaitState();
            }
          
            return AIStateResult.None;
        }

        handler.UpdateWaitTimer(canReach);

        if (handler.HasWaitTimerExceeded())
        {
            return AIStateResult.MoveToStartPosition;
        }

       

        return AIStateResult.None;
    }



    public override void Exit()
    {
        //fov.ResetTarget();
        motor.ResetLockTarget();
        motor.SetStrafe(false);
        if(castCoroutine != null)
        {
            castCoroutine = StartCoroutine(CastCroutine());
        }


    }

    IEnumerator CastCroutine()
    {
        if (spellInventory.CurrentSpell.quantity <= 1)
        {
            spellInventory.TopUpCurrentSpell(20);
        }

        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

       emitter.EndEmit();
       handler.ResetDistanceAttackTimer();
       
       castCoroutine = null;

    }
}
