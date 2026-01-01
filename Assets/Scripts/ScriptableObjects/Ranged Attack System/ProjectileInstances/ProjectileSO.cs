using UnityEngine;
public abstract class ProjectileSO : ScriptableObject
{
    public GameObject prefab;

    [Header("Base stats")]
    public float speed = 3f;
    public float damage = 1f;
    public float lifetime = 10f;

    public ProjectileAttackSO attackSO;

    public virtual ProjectileData CreateData(
        IProjectileMove move,
        Vector3 direction,
        Transform target = null
    )
    {
        return new ProjectileData
        {
            speed = speed,
            damage = damage,
            lifetime = lifetime,
            Move = move,
            baseDir = direction,
            target = target
        };
    }

}
