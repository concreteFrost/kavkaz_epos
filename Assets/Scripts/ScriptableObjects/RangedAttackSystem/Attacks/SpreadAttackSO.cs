using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttack_Spread", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Spread Attack")]
public class SpreadAttackSO : ProjectileAttackSO
{

    public float angle = 30f;

    public override void Execute(IEmitter emitter, int amount, float spawnDelay)
    {
        var proj = emitter.Projectile();

        for (int i = 0; i < amount; i++)
        {
            float currentAngle = 0f;

            if (amount > 1)
            {
                float t = (float)i / (amount - 1);
                currentAngle = Mathf.Lerp(-angle, angle, t);
            }

            Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * emitter.Origin().forward;

            proj.CreateProjectile(
                emitter.StartingPosition(),
                emitter.Target(),
                emitter.AttackSource(),
                dir,
                emitter.DamageMultiplier()
            );
        }
    }

}
