using UnityEngine;

[CreateAssetMenu(
    fileName = "Orbit Move",
    menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Orbit Move"
)]
public class OrbitMoveSO : ProjectileMoveSO
{
    [SerializeField] float radius = 2f;
    [SerializeField] float angularSpeed = 180f; // degrees per second

    public override Vector3 Move(
        Transform emitSource,
        Transform self,
        IDamagable target,
        Vector3 baseDir,
        float speed,
        float aliveTime
    )
    {

        Vector3 center = emitSource.position;
        center.y = emitSource.transform.position.y  + 1.5f;

        float angle = angularSpeed * aliveTime;

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * radius;

        Vector3 desiredPos = center + offset;

        Vector3 dir = (desiredPos - self.position).normalized;
        return dir * speed;
    }
}
