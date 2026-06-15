using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    // Источник атаки (обычно Transform игрока или врага)
    protected IAttackSource attackSource;

    // Коллайдер, используемый для нанесения урона
    protected Collider damageCollider;

    // Список уже поражённых коллайдеров, чтобы не наносить урон дважды
    protected readonly HashSet<Collider> hitColliders = new();

    // Данные о уроне для текущей атаки
    DamageData damageData;

    // Список типов персонажей, которым не наносится урон
    List<CharacterType> objectsToIgnore;

    [HideInInspector] public bool attackInterrupted; // флаг прерывания атаки (например, блок щитом)
    public bool isAttackRegistered = false;         // флаг того, что атака уже зарегистрирована

    private Vector3 lastPosition; // позиция коллайдера в предыдущем кадре для расчёта движения

    // Инициализация коллайдера
    public void Init()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.isTrigger = true; // триггер, чтобы не было физического столкновения
        damageCollider.enabled = false;

        lastPosition = transform.position;

        DisableCollider(); // выключаем коллайдер по умолчанию
    }

    // FixedUpdate используется для физики
    private void FixedUpdate()
    {
        if (!damageCollider.enabled || attackInterrupted) return;

        // Вычисляем направление и дистанцию движения коллайдера
        Vector3 moveDir = transform.position - lastPosition;
        float moveDist = moveDir.magnitude;

        if (moveDist > 0f)
        {
            // "Протаскиваем" коллайдер по траектории и получаем все пересечения
            Collider[] hits = SweepColliders(lastPosition, moveDir.normalized, moveDist);

            foreach (var col in hits)
            {
                HandleCollision(col);
                if (attackInterrupted) break; // остановка при блоке или успешном ударе
            }
        }

        lastPosition = transform.position;
    }

    // Включение коллайдера для атаки
    public virtual void EnableCollider(DamageData damageData ,List<CharacterType> targetsToIgnore, IAttackSource attackSource)
    {
        this.damageData = damageData;
        this.attackSource = attackSource;
        objectsToIgnore = targetsToIgnore;

        attackInterrupted = false;
        hitColliders.Clear();

        damageCollider.enabled = true;
        lastPosition = transform.position;
    }

    // Выключение коллайдера после атаки
    public virtual void DisableCollider()
    {
        attackSource = null;
        damageCollider.enabled = false;
        attackInterrupted = false;
        hitColliders.Clear();
        objectsToIgnore = null;
    }

    // Метод "протаскивания" коллайдера и получение пересечений
    protected virtual Collider[] SweepColliders(Vector3 origin, Vector3 direction, float distance)
    {
        if (damageCollider == null) return System.Array.Empty<Collider>();

        switch (damageCollider)
        {
            case BoxCollider box:
                Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);
                return Physics.BoxCastAll(origin + box.center, halfExtents, direction, transform.rotation, distance).ConvertHitsToColliders();

            case SphereCollider sphere:
                float radius = sphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                return Physics.SphereCastAll(origin + sphere.center, radius, direction, distance).ConvertHitsToColliders();

            case CapsuleCollider capsule:
                Vector3 up = Vector3.up;
                switch (capsule.direction)
                {
                    case 0: up = transform.right; break;
                    case 1: up = transform.up; break;
                    case 2: up = transform.forward; break;
                }
                Vector3 point1 = origin + capsule.center + up * (capsule.height / 2 - capsule.radius);
                Vector3 point2 = origin + capsule.center - up * (capsule.height / 2 - capsule.radius);
                float capRadius = capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                return Physics.CapsuleCastAll(point1, point2, capRadius, direction, distance).ConvertHitsToColliders();

            default:
                Debug.LogWarning("Unsupported collider type: " + damageCollider.GetType());
                return System.Array.Empty<Collider>();
        }
    }

    // Обработка коллизий с другими объектами
    protected virtual void HandleCollision(Collider other)
    {
        if (attackInterrupted) return;

        // Проверка, можно ли нанести урон этому объекту
        if (!TryGetDamagable(other, out var damagable))
        {
            isAttackRegistered = true;
            
            return;
        }

        if (NotInTargetList(damagable)) return;
        if (!hitColliders.Add(other)) return;

        // Проверка блока щитом с пересчётом урона
        if (damagable.Protection != null )
        {
            if(damagable.Protection.IsProtectionActive && IsFacingTarget(damagable))
            {
                DamageData recalculatedDamage = new DamageData(); // создаём отдельный объект
              
                recalculatedDamage.damageMultiplier = damageData.damageMultiplier;
                recalculatedDamage.balanceDamageType = BalanceDamageType.Blocked;
                recalculatedDamage.statusEffectData = damageData.statusEffectData;
                recalculatedDamage.impactForce = damageData.impactForce;
                recalculatedDamage.finalDamage = damageData.finalDamage *(1f - damagable.Protection.ShieldData().GetDefenceBonus());

                damagable.Protection.ReduceDurability(damagable.Protection.ShieldData().GetBreakdownPenalty());
                ApplyDamage(damagable, recalculatedDamage);
                return; // атака прервана блоком
            }
          
        }

       
        // Наносим обычный урон
        ApplyDamage(damagable, damageData);
    }

    // Метод применения урона
    protected virtual void ApplyDamage(IDamagable target, DamageData data)
    {
        target.TakeDamage(data, attackSource);

        isAttackRegistered = true;
    }

    // Проверка, есть ли компонент IDamagable
    protected bool TryGetDamagable(Collider other, out IDamagable damagable)
    {
        damagable = other.GetComponent<IDamagable>();
        return damagable != null;
    }

    // Проверка, можно ли атаковать цель по типу персонажа
    protected bool NotInTargetList(IDamagable damagable)
    {
        if (objectsToIgnore == null || objectsToIgnore.Count == 0) return true;
        return objectsToIgnore.Contains(damagable.CharacterType);
    }

    // Проверка, смотрит ли цель на атакующего (для блока щитом)
    private bool IsFacingTarget(IDamagable target)
    {
        Vector3 toTarget = (target.GetOrigin().position - attackSource.Source().position).normalized;
        Vector3 targetForward = target.GetOrigin().forward;
        float angle = Vector3.Angle(-toTarget, targetForward);

        return angle < 45f; // угол, в пределах которого блок считается действительным
    }
}

// Расширение для конвертации RaycastHit[] в Collider[]
public static class ColliderExtensions
{
    public static Collider[] ConvertHitsToColliders(this RaycastHit[] hits)
    {
        var cols = new Collider[hits.Length];
        for (int i = 0; i < hits.Length; i++)
            cols[i] = hits[i].collider;
        return cols;
    }
}