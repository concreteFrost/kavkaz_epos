using UnityEngine;

[CreateAssetMenu(fileName = "Spiral Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Spiral Move")]
public class SpiralMoveSO : ProjectileMoveSO
{
    [SerializeField] float amplitude = 3f;
    [SerializeField] float freq = 5f;

    // SpiralMoveSO
    public override Vector3 Move(Transform self, IDamagable target, Vector3 baseDir, float speed, float aliveTime)
    {
        // вращение спирали во времени
        float t = aliveTime * freq;

        // смещение по спирали в локальных осях
        Vector3 spiralOffset =self.forward + self.right * Mathf.Cos(t) * amplitude + self.up * Mathf.Sin(t) * amplitude;

        // движение вперёд с постоянной скоростью
        Vector3 forwardMovement = baseDir.normalized * speed;

        // возвращаем вектор скорости (без deltaTime!) — в Update его умножим на Time.deltaTime
        return forwardMovement + spiralOffset;
    }
}
