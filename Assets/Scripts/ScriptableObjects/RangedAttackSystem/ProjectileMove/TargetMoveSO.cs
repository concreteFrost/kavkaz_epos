using UnityEngine;

[CreateAssetMenu(fileName = "Target Move", menuName = ScriptablePaths.PROJECTILE_MOVE_PATH + "/Target Move")]
public class TargetMoveSO : ProjectileMoveSO
{
    [SerializeField] float turnSpeed = 100f;

    public override Vector3 Move(
        Transform self,
        IDamagable target,
        Vector3 baseDir,
        float speed
    )
    {
        Vector3 desiredDir = self.forward;
        //запустить снаряд вперёд если цель отсутствует
        if (target == null)
        {
            Debug.Log("target is null");
            return self.position.normalized * speed;
        }
           

        //направление к цели
        desiredDir =
            (target.GetAimTransform().position - self.position).normalized; 

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
