using UnityEngine;

[CreateAssetMenu(fileName = "TripleAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH  + "/TripleAttack")]
public class TripleAttackSO : ProjectileAttackSO
{
    [SerializeField] private float spreadAngle = 2f;
    [SerializeField] ProjectileMoveSO moveSO;

public override void Execute(IEmitter emitter)
{
    float spread = spreadAngle > 0 ? spreadAngle : 1;

    Transform origin = emitter.Origin();

    for (int i = -1; i <= 1; i++)
    {
        float angle = spread * i;

        Vector3 direction = Quaternion.AngleAxis(angle, origin.up) 
                            * origin.forward;

            ProjectileDirection moveData = new ProjectileDirection()
            {
                MoveBehaviour = moveSO,
                baseDir = direction,
            };

        emitter.NewProjectile(moveData);
    }
}

}
