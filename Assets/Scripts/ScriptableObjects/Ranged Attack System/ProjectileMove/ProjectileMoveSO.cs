using UnityEngine;

public abstract class ProjectileMoveSO : ScriptableObject, IProjectileMove
{

    public abstract Vector3 Move(Transform self,Transform target,Vector3 baseDir,float speed);
}
