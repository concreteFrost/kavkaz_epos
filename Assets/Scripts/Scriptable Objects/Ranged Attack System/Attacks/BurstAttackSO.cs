using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ProjectileAttack_Burst", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Burst Attack")]
public class BurstAttackSO : ProjectileAttackSO
{

    public override void Execute(IEmitter emitter, int amount, float spawnDelay)
    {
        emitter.EmitWithDelay(BurstRoutine(emitter, amount, spawnDelay));
    }

    IEnumerator BurstRoutine(IEmitter emitter, int amount, float spawnDelay)
    {
        var proj = emitter.Projectile();

        for (int i = 0; i < amount; i++)
        {
            proj.CreateProjectile(
                emitter.StartingPosition(),
                emitter.Target(),
                emitter.AttackSource(),
                emitter.Origin().forward,
                emitter.DamageMultiplier()
            );

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

