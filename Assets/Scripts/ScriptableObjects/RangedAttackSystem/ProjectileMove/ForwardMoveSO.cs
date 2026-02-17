using UnityEngine;

[CreateAssetMenu(fileName = "Forward Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Forward Move")]
public class ForwardMoveSO : ProjectileMoveSO
{
    public override Vector3 Move(Transform self, IDamagable target, Vector3 baseDir, float speed)
    {
        return baseDir * speed;
    }
}
    

