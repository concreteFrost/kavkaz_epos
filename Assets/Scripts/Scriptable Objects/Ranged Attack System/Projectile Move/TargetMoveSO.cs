using UnityEngine;

[CreateAssetMenu(fileName = "Target Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Target Move")]
public class TargetMoveSO : ProjectileMoveSO
{
    [SerializeField] float turnSpeed = 3f;
    [SerializeField] float homingDuration = 1.5f;

    public override Vector3 Move(Transform emitSource,
     Transform self,
     IDamagable target,
     Vector3 baseDir,
     float speed,
     float aliveTime
 )
    {
        if (target == null || aliveTime >= homingDuration)
            return baseDir * speed;

        Vector3 desiredDir =
            (target.GetAimTransform().position - self.position).normalized;

        Vector3 finalDir = Vector3.RotateTowards(
            baseDir,
            desiredDir,
            turnSpeed * Time.deltaTime,
            0f
        );

        return finalDir * speed;
    }
}

