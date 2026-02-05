using UnityEngine;

[CreateAssetMenu(fileName = "TargetAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH  + "/TargetAttack")]
public class TargetAttackSO : ProjectileAttackSO
{
    [SerializeField] private ProjectileAttackSO backupAttackSO;
    [SerializeField] private ProjectileMoveSO moveSO;
    public override void Execute(IEmitter gun)
    {
        if (gun.Target() == null)
        {            
            backupAttackSO.Execute(gun);
            return;
        }

        Quaternion spread = SpreadRotation(gun.Spread);
        
        var dir = (gun.Target().position - gun.Origin().position).normalized;
        dir = spread * dir;

        var data = gun.Projectile().CreateData(moveSO, dir, gun.Target());

        var bullet = gun.NewProjectile(data);
           
    }
}
