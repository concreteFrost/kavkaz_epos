using UnityEngine;

[CreateAssetMenu(fileName = "Multiple Attack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Multiple Attack")]
public class MultipleAttackSO : ProjectileAttackSO
{
    [SerializeField] private float spreadAngle = 2f;
    [SerializeField] ProjectileMoveSO moveSO;

    public override void Execute(IEmitter emitter)
    {
        float spread = spreadAngle > 0 ? spreadAngle : 1;

        Transform origin = emitter.Origin();
        int spawnAmount = emitter.Projectile().amountToSpawn;  // берём динамически
        if (spawnAmount < 1) spawnAmount = 1;

        float halfSpread = spreadAngle * 0.5f;

        for (int i = 0; i < spawnAmount; i++)
        {
            float t = (spawnAmount == 1) ? 0f : (float)i / (spawnAmount - 1); // 0..1
            float angle = Mathf.Lerp(-halfSpread, halfSpread, t);

            Vector3 direction = Quaternion.AngleAxis(angle, origin.up) * origin.forward;

            ProjectileDirection moveData = new ProjectileDirection()
            {
                MoveBehaviour = moveSO,
                baseDir = direction,
            };

            emitter.NewProjectile(moveData);
        }
    }

}
