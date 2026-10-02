using UnityEngine;

[CreateAssetMenu(
    fileName = "Chaotic Rain Move",
    menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Chaotic Rain"
)]
public class ChaoticRainMoveSO : ProjectileMoveSO
{
    [SerializeField, Min(0f)] private float directionSpread = 1f;
    [SerializeField, Min(0f)] private float waveStrength = 0.55f;
    [SerializeField, Min(0f)] private float waveFrequency = 2.5f;

    public override Vector3 Move(
        Transform emitSource,
        Transform self,
        IDamagable target,
        Vector3 baseDir,
        float speed,
        float aliveTime
    )
    {
        // Derive stable but distinct movement parameters per projectile. ScriptableObjects
        // are shared by every projectile, so random state must not be stored on this asset.
        float seed = Mathf.Abs(self.GetInstanceID());
        float angle = Hash01(seed, 12.9898f) * Mathf.PI * 2f;
        float drift = directionSpread * Mathf.Lerp(0.45f, 1.4f, Hash01(seed, 78.233f));
        float frequency = waveFrequency * Mathf.Lerp(0.65f, 1.6f, Hash01(seed, 39.346f));
        float phaseX = Hash01(seed, 11.135f) * Mathf.PI * 2f;
        float phaseZ = Hash01(seed, 57.583f) * Mathf.PI * 2f;

        Vector3 personalDrift = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * drift;

        Vector3 sideways = new Vector3(
            Mathf.Sin(aliveTime * frequency + phaseX),
            0f,
            Mathf.Cos(aliveTime * frequency * 0.73f + phaseZ)
        ) * waveStrength;

        return (Vector3.down + personalDrift + sideways).normalized * speed;
    }

    private static float Hash01(float value, float salt)
    {
        float hashed = Mathf.Sin(value * salt) * 43758.5453f;
        return Mathf.Repeat(hashed, 1f);
    }
}
