using UnityEngine;

[CreateAssetMenu(fileName = "TripleAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH  + "/TripleAttack")]
public class TripleAttackSO : ProjectileAttackSO
{
    [SerializeField] private float spreadAngle = 2f;

    [SerializeField] ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {

        float spread = spreadAngle > 0 ? spreadAngle : 1;

        Vector3 offsetDirection = new Vector3(-spread, 0, spread);
        
        for (int i = 0; i < 3; i++)
        {
            var data = emitter.Projectile().CreateData(
                moveSO,
                offsetDirection
                );

            var newBullet = emitter.NewProjectile(data);

           

            offsetDirection.x += spread;
        }
    }
}
