using UnityEngine;

[CreateAssetMenu(fileName = "ForwardMove", menuName = ProjectileConsts.PROJECTILE_MOVE_PATH + "ForwardMove")]
public class ForwardMoveSO : ProjectileMoveSO
{
    public override Vector3 Move(Transform self, Transform target, Vector3 baseDir, float speed)
    {
        return baseDir * speed;
    }
}
    

