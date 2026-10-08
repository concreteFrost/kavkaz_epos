using UnityEngine;

[CreateAssetMenu(
    fileName = "Ballista Move",
    menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Ballista Move"
)]
public class BallistaMoveSO : ProjectileMoveSO
{
    [SerializeField, Min(0f)] private float gravity = 9.81f;

    public override Vector3 Move(
        Transform emitSource,
        Transform self,
        IDamagable target,
        Vector3 baseDir,
        float speed,
        float aliveTime
    )
    {
        // Projectile retains direction only, so speed is restored each frame.
        // This approximates a falling trajectory rather than conserving ballistic velocity.
        return baseDir * speed + Vector3.down * gravity * Time.deltaTime;
    }
}
