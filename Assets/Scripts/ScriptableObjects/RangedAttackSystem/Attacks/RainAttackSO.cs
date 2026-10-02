using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ProjectileAttack_Rain",
    menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Rain Attack"
)]
public class RainAttackSO : ProjectileAttackSO
{
    [SerializeField, Range(1, 10)] private int maxProjectiles = 10;
    [SerializeField, Min(0f)] private float minSpawnDelay = 0.08f;
    [SerializeField, Min(0f)] private float maxSpawnDelay = 0.4f;

    public override void Execute(IEmitter emitter, int amount, float spawnDelay)
    {
        emitter.EmitWithDelay(RainRoutine(emitter, Mathf.Clamp(amount, 1, maxProjectiles)));
    }

    private IEnumerator RainRoutine(IEmitter emitter, int amount)
    {
        var projectile = emitter.Projectile();
        for (int i = 0; i < amount; i++)
        {
            projectile.CreateProjectile(
                emitter.StartingPosition(),
                emitter.Target(),
                emitter.AttackSource(),
                Vector3.down,
                emitter.DamageMultiplier()
            );

            if (i < amount - 1)
                yield return new WaitForSeconds(Random.Range(minSpawnDelay, Mathf.Max(minSpawnDelay, maxSpawnDelay)));
        }
    }
}
