using UnityEngine;

public struct ProjectileDirection
{
    public IProjectileMove MoveBehaviour;
    public Vector3 baseDir;
}
public abstract class ProjectileSO : ItemSO
{
    public GameObject prefab;

    [Header("Base stats")]
    public float speed = 3f;
    public float lifetime = 10f;

    public DamageData damageData;
 
    public ProjectileAttackSO attackSO;

    //public virtual void SetDirection(
    //    IProjectileMove move,
    //    Vector3 direction
       
    //)
    //{
    //    Move = move,
    //        baseDir = direction,
    //}

}
