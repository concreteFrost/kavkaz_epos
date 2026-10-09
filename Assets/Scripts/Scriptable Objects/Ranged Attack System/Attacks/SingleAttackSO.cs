using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttack_Single", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Single Attack")]
public class SingleAttackSO : ProjectileAttackSO
{

    public override void Execute(IEmitter emitter, int amount, float spawnDelay)
    {
        amount = 1;

        var proj = emitter.Projectile();

        proj.CreateProjectile(
            emitter.StartingPosition(),
            emitter.Target(),
            emitter.AttackSource(),
            emitter.Origin().forward,
            emitter.DamageMultiplier()
        );

    }
}

