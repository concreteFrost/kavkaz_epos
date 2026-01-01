using UnityEngine;

[CreateAssetMenu(fileName = "TargetMove", menuName = ProjectileConsts.PROJECTILE_MOVE_PATH + "TargetMove")]
public class TargetMoveSO : ProjectileMoveSO
{
    [SerializeField] float turnSpeed = 100f;

    public override Vector3 Move(
        Transform self,
        Transform target,
        Vector3 baseDir,
        float speed
    )
    {
        Vector3 desiredDir = self.forward;
        //запустить снаряд вперёд если цель отсутствует
        if (target == null)
            return self.position.normalized * speed;

        //направление к цели
        desiredDir =
            (target.position - self.position).normalized; 

        //поворот в сторону цели
        Vector3 finalDir = Vector3.RotateTowards(
            baseDir,
            desiredDir,
            turnSpeed * Time.deltaTime,
            0f
        );

        return finalDir * speed;
    }
}
