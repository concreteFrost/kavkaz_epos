using UnityEngine;

[CreateAssetMenu(fileName = "Spiral Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Spiral Move")]
public class SpiralMoveSO : ProjectileMoveSO
{
    [SerializeField] float amplitude = 3f;
    [SerializeField] float freq = 5f;

    public override Vector3 Move(Transform self, IDamagable target, Vector3 baseDir, float speed)
    {
        float t = Time.time * freq;

        float x = Mathf.Cos(t);
        float y = Mathf.Sin(t);

        Vector3 spiralOffset =
            self.right * x * amplitude +
            self.up * y * amplitude;

        return baseDir * speed + spiralOffset;
    }
}
