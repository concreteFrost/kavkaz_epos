using UnityEngine;

[CreateAssetMenu(fileName = "projectile_move_spit", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Плевок")]
public class SpitMoveSO : ProjectileMoveSO
{
    [Tooltip("Начальная скорость подъёма в метрах в секунду.")]
    [SerializeField, Min(0f)] private float initialUpwardSpeed;

    [Tooltip("Ускорение падения в метрах в секунду за секунду.")]
    [SerializeField, Min(0f)] private float gravity;

    public override Vector3 Move(Transform emitSource, Transform self, IDamagable target,
        Vector3 baseDir, float speed, float aliveTime)
    {
        // Средняя скорость за кадр даёт равномерную дугу при разной частоте кадров.
        float verticalSpeed = initialUpwardSpeed - gravity * (aliveTime + Time.deltaTime * 0.5f);
        return baseDir * speed + Vector3.up * verticalSpeed;
    }
}
