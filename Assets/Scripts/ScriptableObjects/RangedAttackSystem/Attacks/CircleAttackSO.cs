using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileAttack_Circle", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Circle Attack")]
public class CircleAttackSO : ProjectileAttackSO
{
   
    public override void Execute(IEmitter emitter,int amount, float spawnDelay)
    {
        var proj = emitter.Projectile();

        for (int i = 0; i < amount; i++)
        {
            float angle = (360f / amount) * i;

            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

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
