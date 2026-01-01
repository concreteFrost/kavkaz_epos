using UnityEngine;

[CreateAssetMenu(fileName = "SingleAttack", menuName = ProjectileConsts.PROJECTILE_ATTACK_PATH + "SingleAttack")]
public class SingleAttackSO : ProjectileAttackSO
{
    [SerializeField] ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {
        Quaternion spread = SpreadRotation(emitter.Spread);
        Vector3 dir = emitter.Origin().forward;
        dir = spread * dir;

        var data = emitter.Projectile().CreateData(
            moveSO,
            dir);

        var b = emitter.NewProjectile(data);

    }
}
