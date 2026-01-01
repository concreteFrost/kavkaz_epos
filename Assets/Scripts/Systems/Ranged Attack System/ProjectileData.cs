using UnityEngine;

public struct ProjectileData
{
    public float speed;
    public float damage;
    public float lifetime;

    public IProjectileMove Move;
    public Vector3 baseDir;
    public Transform target;

}
