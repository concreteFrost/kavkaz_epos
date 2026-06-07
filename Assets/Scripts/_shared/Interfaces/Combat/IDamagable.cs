using System;
using UnityEngine;

public interface IDamagable
{
    Collider DamageCollider();
    CharacterType CharacterType { get; set; }
    // Тип персонажа (например, игрок, враг) для фильтрации атак и логики целей
    void ToggleDamagableCollider(bool isActive);
    void TakeMaxDamage();
    void TakeDamage(DamageData damageData, Transform source = null);
    // Метод нанесения урона; source указывает, кто атакует

    IShield Protection { get; set; }

    bool IsDead { get; set; }
    // Флаг, показывающий, что персонаж мертв

    bool IsDamaged { get; set; }
    // Флаг, указывающий, что персонаж получил урон

    bool IsKnockedOut { get; set; }
    // Флаг, показывающий, что персонаж находится в нокауте (временно неактивен)

    bool InBlockingWindow { get; set; }
    // Временное окно, когда атаки не наносят урон (например, при анимации блока)

    bool CanPlayDamagedAnimation { get; set; }
    Transform GetAimTransform();
    // Точка прицеливания, используемая для расчёта направления атаки

    Transform GetOrigin();
    // Точка происхождения персонажа (например, центр коллайдера или положение тела)

    event Action<Transform> DamageTaken;
    // Событие, вызываемое при получении урона (можно подписаться на эффекты, UI или звук)
}