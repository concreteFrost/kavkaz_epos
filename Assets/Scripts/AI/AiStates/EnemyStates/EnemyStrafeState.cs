using System.Collections;
using UnityEngine;

public class EnemyStrafeState : AIState<EnemyBrainContext>
{
    // ссылки на контекст
    private EnemyStrafeHandler strafeHandler;
    private EnemyCombatHandler combatHandler;

    private EnemyFOVController fov;
    private HumanoidAIMotor motor;
    private HumanoidCombatController combatController;
    private HumanoidCombatInventory inventory;  

    // корутина стрейфа
    private Coroutine strafeCoroutine;

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

        // сбрасываем и инициализируем таймеры состояния стрейфа
        strafeHandler.ResetStrafeState();
        strafeHandler.SetNewMaxInStrafeTime();

        // полностью останавливаем обычное перемещение
        motor.StopMovement();

        // включаем режим стрейфа (влияет на скорость / анимации)
        motor.SetStrafe(true);

        // фиксируем поворот тела на цель
        motor.SetLockTarget(fov.currentTarget.GetAimTransform());

        // сообщаем FOV, что цель сейчас залочена
        fov.ToggleLockState(true);
    }

    public override AIStateResult Run()
    {
        
        if (fov.currentTarget == null)
            return AIStateResult.Idle;

        combatHandler.ToggleShield(true, inventory, combatController);

        // обновляем время, проведённое в стрейфе
        strafeHandler.UpdateTimeInStrafeState();

        // стрейф длился достаточно — переходим в погоню
        if (strafeHandler.IsStrafeTimeFinished())
            return AIStateResult.Chase;

        Transform self = context.self;

        // проверяем дистанцию до цели
        float distance = Vector3.Distance(
            self.position,
            fov.currentTarget.GetOrigin().position
        );

        // цель ушла слишком далеко — стрейф больше не имеет смысла
        if (strafeHandler.IsStrafeTargetFar(distance))
            return AIStateResult.Attack;

        // запускаем корутину стрейфа один раз
        if (strafeCoroutine == null)
            strafeCoroutine = StartCoroutine(StrafeCoroutine());

        return AIStateResult.None;
    }

    public override void Exit()
    {
        // гарантированно останавливаем корутину
        StopStrafeCoroutine();

        motor.ResetLockTarget();
        motor.SetStrafe(false);
        fov.ToggleLockState(false);

        combatHandler.ToggleShield(false, inventory, combatController);
    }

    private void StopStrafeCoroutine()
    {
       
        if (strafeCoroutine == null)
            return;

        StopCoroutine(strafeCoroutine);
        strafeCoroutine = null;
    }

    private IEnumerator StrafeCoroutine()
    {
        
        bool isRight = Random.value > 0.5f;

        float elapsed = 0f;
        const float maxStrafeTime = 3f;

        
        while (elapsed < maxStrafeTime && fov.currentTarget != null)
        {
            Transform self = context.self;
            Transform target = fov.currentTarget.GetOrigin();

            Vector3 selfPos = self.position;
            Vector3 targetPos = target.position;

            // направление от врага к цели
            Vector3 toTarget = (targetPos - selfPos).normalized;

            // боковое направление в плоскости XZ
            Vector3 strafeDir = Vector3.Cross(Vector3.up, toTarget).normalized;

            // движение в выбранную сторону
            motor.MoveLocal(isRight ? strafeDir : -strafeDir);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // корутина завершена — разрешаем перезапуск
        strafeCoroutine = null;
    }
}
