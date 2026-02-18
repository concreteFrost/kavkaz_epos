using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет функцией захвата цели игроком, включая обнаружение, отслеживание, переключение и сброс захвата цели,
/// а также обновление соответствующих состояний пользовательского интерфейса и контроллера.
/// </summary>
public class PlayerTargetLock : MonoBehaviour, ITargetLocker
{
    LockOnTargetUI lockOnTargetUI;

    protected Transform targetSeeker;
    public IDamagable currentTarget;

    protected bool wasTargetSearched = false;
    //public bool IsLockedOnTarget { get => currentTarget != null; }

    /// <summary>
    /// Ввод мыши по оси Х при котором цель сбрасывается
    /// </summary>
    private float targetSwitchThreshold = 45f;
    private float targetCheckDistance = 10f;
    private float targetResetDistance = 13f;

    [SerializeField] private Image img;

    PlayerLocomotionActionHandler controller;
    IDamagable damageController;

    CharacterType self;

    #region ITargetLocker Contract
    public IDamagable CurrentTarget() => currentTarget != null ? currentTarget : null;
    #endregion

    public void Init(
        LockOnTargetUI lockOnTargetUI,
        PlayerLocomotionActionHandler controller,
        IDamagable damageController
        )
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.targetSeeker = controller.transform;
        this.damageController = damageController;

        self = CharacterType.Player;
    }

    /// <summary>
    /// Обновляет состояние отслеживания цели, сбрасывая Lock, если текущая цель недействительна или неактивна.
    /// </summary>
    private void Update()
    {
        if (currentTarget == null) return;
        //if (damageController.IsDead || currentTarget.IsKnockedOut)
        if (damageController.IsDead)
        {
            ResetLockedTarget();
            return;
        }
        TrackTargetDistance();
    }

    /// <summary>
    /// Обновляет положение заблокированной цели в пользовательском интерфейсе и вычисляет расстояние до цели.
    /// </summary>
    public void TrackTargetDistance()
    {
        lockOnTargetUI.CalculateImagePosition();
        CalculateDistanceToTarget();
    }

    /// <summary>
    /// Фиксирует текущую цель и обновляет пользовательский интерфейс и контроллер, отражая заблокированное состояние.
    /// </summary>
    public void SetLockedTarget(IDamagable t)
    {

        controller.SetLockTarget(t.GetAimTransform());
        controller.SetStrafe(true);
        lockOnTargetUI.SetTarget(t.GetAimTransform());
    }

    public void HandleSetTarget()
    {
        var t = TryGetLockedTarget();

        if(t == null) return;   

        SetLockedTarget(t);
    }

    /// <summary>
    /// Вычисляет расстояние до текущей цели и сбрасывает блокировку, если цель уничтожена или находится за пределами сбросимого расстояния.
    /// </summary>
    protected virtual void CalculateDistanceToTarget()
    {
        if (currentTarget.IsDead || currentTarget.IsKnockedOut)
        {
            ResetLockedTarget();
            return;
        }
        var dist = Vector3.Distance(targetSeeker.position, currentTarget.GetOrigin().position);

        if (dist > targetResetDistance)
        {
            ResetLockedTarget();

        }
    }

    /// <summary>
    /// Сбрасывает текущую цель захвата и связанные с ней состояния пользовательского интерфейса и контроллера.
    /// </summary>
    public void ResetLockedTarget()
    {
        currentTarget = null;
        wasTargetSearched = false;
        lockOnTargetUI.ResetTarget();
        controller.ResetLockTarget();
        controller.SetStrafe(false);

    }


    /// <summary>
    /// Пытается получить и зафиксировать ближайшую допустимую цель, переключая состояние блокировки при каждом вызове.
    /// </summary>
    /// <returns>Заблокированная цель, если найдена; в противном случае — null.</returns>
    public IDamagable TryGetLockedTarget()
    {
        wasTargetSearched = !wasTargetSearched;

        if (!wasTargetSearched)
        {
            ResetLockedTarget();
            lockOnTargetUI.ResetTarget();
            return null;
        }

        var nearest = CheckNearestTarget();

        if (nearest != null)
        {
            currentTarget = CheckNearestTarget();
            lockOnTargetUI.SetTarget(currentTarget.GetAimTransform());

            return nearest;
        }

        return null;
    }

    /// <summary>
    /// Находит и возвращает ближайшую допустимую цель IDamagable из предоставленных коллайдеров в пределах указанного расстояния.
    /// </summary>
    /// <param name="targets">Массив объектов Collider для поиска потенциальных целей.</param>
    /// <returns>Ближайшая цель IDamagable, если найдена; в противном случае — null.</returns>
    protected IDamagable GetNearestTarget(Collider[] colliders)
    {
        IDamagable bestTarget = null;
        float bestDistance = float.MaxValue;

        HashSet<IDamagable> checkedTargets = new HashSet<IDamagable>();

        foreach (var col in colliders)
        {
            if (!TryGetDamagable(col, out var target))
                continue;

            if (target.CharacterType == self)
                continue;

            // защита от дубликатов (несколько коллайдеров у одного врага)
            if (!checkedTargets.Add(target))
                continue;

            float distance = Vector3.Distance(
                targetSeeker.position,
                target.GetOrigin().position
            );

            if (distance > targetCheckDistance)
                continue;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTarget = target;
            }
        }

        return bestTarget;
    }



    /// <summary>
    /// Находит и возвращает ближайшую цель в пределах указанного расстояния от ищущего цели.
    /// </summary>
    /// <returns>Ближайшая цель, поддающаяся идентификации (IDamagable), если таковая найдена; в противном случае — null.</returns>
    protected IDamagable CheckNearestTarget()
    {
        var colliders = Physics.OverlapSphere(
            targetSeeker.position,
            targetCheckDistance
        );

        return colliders.Length > 0
            ? GetNearestTarget(colliders)
            : null;
    }

    /// <summary>
    /// Переключает текущую цель на ближайшую допустимую цель в направлении, указанном курсором mouseX, обновляя связанные
    /// ссылки на пользовательский интерфейс и контроллер.
    /// </summary>
    /// <param name="mouseX">Горизонтальное движение мыши, используемое для определения направления переключения целей.</param>
    /// <returns>Вновь выбранная цель, если найдена допустимая; в противном случае — null.</returns>
    public IDamagable SwitchTarget(float mouseX)
    {
        if (currentTarget == null) return null;
        if (Mathf.Abs(mouseX) < targetSwitchThreshold) return null;

        Camera cam = Camera.main;

        Vector3 currentScreen =
            cam.WorldToScreenPoint(currentTarget.GetAimTransform().position);

        var colliders = Physics.OverlapSphere(targetSeeker.position, targetCheckDistance);

        IDamagable bestTarget = null;
        float bestDeltaX = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!TryGetDamagable(col, out var target))
                continue;

            if (target == currentTarget || target.CharacterType == self)
                continue;

            Vector3 screenPos =
                cam.WorldToScreenPoint(target.GetAimTransform().position);

            float deltaX = screenPos.x - currentScreen.x;

            if (mouseX > 0 && deltaX <= 0) continue;
            if (mouseX < 0 && deltaX >= 0) continue;

            float absDelta = Mathf.Abs(deltaX);

            if (absDelta < bestDeltaX)
            {
                bestDeltaX = absDelta;
                bestTarget = target;
            }
        }


        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            //state.SetLockTarget(currentTarget);
            lockOnTargetUI.SetTarget(currentTarget.GetAimTransform());
            controller.SetLockTarget(currentTarget.GetAimTransform());
        }

        return currentTarget;
    }

    /// <summary>
    /// Возвращает IDamagable или null
    /// </summary>
    /// <param name="col"></param>
    /// <param name="damagable"></param>
    /// <returns></returns>
    private bool TryGetDamagable(Collider col, out IDamagable damagable)
    {
        damagable =
            col.GetComponent<IDamagable>()
            ?? col.GetComponentInChildren<IDamagable>()
            ?? col.GetComponentInParent<IDamagable>();

        return damagable != null;
    }



}
