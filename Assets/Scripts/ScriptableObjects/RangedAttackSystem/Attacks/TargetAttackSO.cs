using UnityEngine;

[CreateAssetMenu(fileName = "TargetAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH  + "/TargetAttack")]
public class TargetAttackSO : ProjectileAttackSO
{
    [SerializeField] private ProjectileAttackSO backupAttackSO;
    [SerializeField] private ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {
        if (emitter.Target() == null)
        {            
            backupAttackSO.Execute(emitter);
            return;
        }

        //Quaternion spread = SpreadRotation(emitter.Spread);
        
        var dir = (emitter.Target().GetAimTransform().position - emitter.Origin().position).normalized;
       

        ProjectileDirection moveData = new ProjectileDirection()
        {
            MoveBehaviour = moveSO,
            baseDir = dir,
        };

        var bullet = emitter.NewProjectile(moveData);
           
    }
}
