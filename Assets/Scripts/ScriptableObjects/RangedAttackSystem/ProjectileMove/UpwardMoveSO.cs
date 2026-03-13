using UnityEngine;

[CreateAssetMenu(fileName = "Upward Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Upward Move")]
public class UpwardMoveSO : ProjectileMoveSO
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
        return self.up * speed;
    }
}
