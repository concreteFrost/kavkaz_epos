using UnityEngine;

[CreateAssetMenu(fileName = "SingleAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/SingleAttack")]
public class SingleAttackSO : ProjectileAttackSO
{
    [SerializeField] ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {
       
        Vector3 dir = emitter.Origin().forward;

        ProjectileDirection directionData = new ProjectileDirection()
        {
            MoveBehaviour = moveSO,
            baseDir = dir,
        };

      
        var b = emitter.NewProjectile(directionData);

    }
}
