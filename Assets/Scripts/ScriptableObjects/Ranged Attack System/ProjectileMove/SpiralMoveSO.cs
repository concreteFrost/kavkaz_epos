using UnityEngine;

[CreateAssetMenu(fileName = "Spiral Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Spiral Move")]
public class SpiralMoveSO : ProjectileMoveSO
{
    [SerializeField] float amplitude = 3f;
    [SerializeField] float freq = 5f;

    public override Vector3 Move(Transform self, Transform target, Vector3 baseDir, float speed)
    {
        float t = Time.time * freq;

        float x = Mathf.Sin(t);
        float y = Mathf.Sin(t) * Mathf.Cos(t);

        return baseDir * speed + (self.right * x * amplitude + self.up * y * amplitude);
    }

}
