using System.Collections;
using UnityEngine;

public interface IEmitter
{
    Vector3 StartingPosition();
    //IHumanoidMeleeCombat MeleeCombat();
    /// <summary>
    /// Transform эмитера
    /// </summary>
    /// <returns></returns>
    Transform Origin();

    /// <summary>
    /// Текущая цель (null поумолчанию)
    /// </summary>
    /// <returns></returns>
    IDamagable Target();

    IAttackSource AttackSource();

    /// <summary>
    /// Данные снаряда
    /// </summary>
    /// <returns></returns>
    ProjectileSO Projectile();

    /// <summary>
    /// Разброс снаряда (точность)
    /// </summary>
    /// <returns></returns>
    //float Spread { get; set; }

    void StartEmit();

    float DamageMultiplier();

    /// <summary>
    /// Запуск снаряда
    /// </summary>
    /// <param name="target">Цель относительно запускающего</param>
    void Emit();

    void EndEmit();

    /// <summary>
    /// Запуск снаряда с задержкой
    /// </summary>
    /// <param name="coroutine">Корутина запуска</param>
    /// <returns></returns>
    Coroutine EmitWithDelay(IEnumerator coroutine);

    bool IsEmitting { get; set; }

}