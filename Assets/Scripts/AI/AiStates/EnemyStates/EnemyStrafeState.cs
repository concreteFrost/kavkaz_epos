using System.Collections;
using UnityEngine;

public class EnemyStrafeState : AIState<EnemyBrainContext>
{
    // ссылки на контекст
    private EnemyStrafeHandler strafeHandler;
    private EnemyCombatHandler combatHandler;

    private EnemyFOVController fov;
    private HumanoidAIMotor motor;
    private IHumanoidMeleeCombat combatController;
    private HumanoidCombatInventory inventory;

    public override void Enter()
    {

        fov = context.fov;
        strafeHandler = context.stateTracker.strafeHandler;
        combatHandler = context.stateTracker.combatHandler;
        combatController = context.combat;
        inventory = context.inventory;

        motor = context.motor;

        // без цели стрейф не имеет смысла
        if (fov.currentTarget == null)
            return;

        // сбрасываем и инициализируем таймеры состо€ни€ стрейфа

        motor.IsSprinting = false;
        strafeHandler.ResetStrafeState();
        strafeHandler.SetNewMaxInStrafeTime();

        // полностью останавливаем обычное перемещение
        motor.StopMovement();

        // включаем режим стрейфа (вли€ет на скорость / анимации)
        motor.SetStrafe(true);

        // фиксируем поворот тела на цель
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());

        motor.IsSprinting = false;

        // сообщаем FOV, что цель сейчас залочена
        //fov.ToggleLockState(true);
    }

    public override AIStateResult Run()
    {

        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        combatHandler.ToggleShield(true, inventory, combatController);

        Transform target = context.fov.currentTarget.GetOrigin();

        // обновл€ем врем€, проведЄнное в стрейфе
        strafeHandler.UpdateTimeInStrafeState();

        // стрейф длилс€ достаточно Ч переходим в погоню
        if (strafeHandler.IsStrafeTimeFinished())
            return AIStateResult.Chase;

        Transform self = context.self;

        // провер€ем дистанцию до цели
        float distance = Vector3.Distance(
            self.position,
            fov.currentTarget.GetOrigin().position
        );

        // цель ушла слишком далеко Ч стрейф больше не имеет смысла
        if (strafeHandler.IsStrafeTargetFar(distance))
            return AIStateResult.Chase;

        // запускаем корутину стрейфа один раз
        if (motor.strafeCoroutine == null)
            motor.StartStrafe(self, target);

        return AIStateResult.None;
    }

    public override void Exit()
    {

        motor.StopStrafe();
        motor.ResetLockTarget();
        motor.SetStrafe(false);
        //fov.ToggleLockState(false);

        combatHandler.ToggleShield(false, inventory, combatController);
    }

   

  
}
