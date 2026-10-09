using UnityEngine;

[CreateAssetMenu(
    fileName = "Ground Trace Move",
    menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Ground Trace"
)]
public class GroundTraceMoveSO : ProjectileMoveSO
{
    public override Vector3 Move(
        Transform emitSource,
        Transform self,
        IDamagable target,
        Vector3 baseDir,
        float speed,
        float aliveTime
    )
    {
        return Vector3.up * speed;
    }
}
