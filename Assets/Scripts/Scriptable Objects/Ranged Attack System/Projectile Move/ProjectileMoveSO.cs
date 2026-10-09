using UnityEngine;

public abstract class ProjectileMoveSO : ScriptableObject, IProjectileMove
{
    public abstract Vector3 Move(Transform emitSource, Transform self,IDamagable target,Vector3 baseDir,float speed, float aliveTime);
}
