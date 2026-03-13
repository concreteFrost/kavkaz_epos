using UnityEngine;

public interface IProjectileMove
{
    /// <summary>
    /// Определяет поведение снаряда за время его жизненного цикла
    /// </summary>
    /// <param name="self">Указатель на Transform снаряда</param>
    /// <param name="target">Цель снаряда (возможна null)</param>
    /// <param name="baseDir">Изначальное направление</param>
    /// <param name="speed">Скорость передвижения</param>
    /// <param name="aliveTime">Время жизни снаряда</param>
    /// <returns></returns>
    Vector3 Move(Transform emitSource, Transform self, IDamagable target, Vector3 baseDir, float speed, float aliveTime);
}
