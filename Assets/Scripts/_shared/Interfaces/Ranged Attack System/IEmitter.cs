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
    IDamagable Target();

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

    /// <summary>
    /// Создает новый снаряд с заполнеными данными
    /// </summary>
    /// <returns></returns>
    IProjectile NewProjectile(ProjectileDirection dir);

    bool IsEmitting { get; set; }

}