using System.Collections;
using UnityEngine;

public interface IEmitter
{
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
    Transform Target();

    /// <summary>
    /// Данные снаряда
    /// </summary>
    /// <returns></returns>
    ProjectileSO Projectile();

    /// <summary>
    /// Разброс снаряда (точность)
    /// </summary>
    /// <returns></returns>
    float Spread { get; set; }

    /// <summary>
    /// Запуск снаряда
    /// </summary>
    /// <param name="target">Цель относительно запускающего</param>
    void Emit();

    void SetEmitTarget(Transform target=null);
    /// <summary>
    /// Запуск снаряда с задержкой
    /// </summary>
    /// <param name="coroutine">Корутина запуска</param>
    /// <returns></returns>
    Coroutine EmitWithDelay(IEnumerator coroutine);

    /// <summary>
    /// Создает новый снаряд с заполнеными данными
    /// </summary>
    /// <returns></returns>
    IProjectile NewProjectile(ProjectileData data);

    bool IsEmitting { get; set; }

}