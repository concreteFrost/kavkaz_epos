using UnityEngine;

public struct ProjectileDirection
{
    public IProjectileMove MoveBehaviour;
    public Vector3 baseDir;
}

public enum EmitStartingPosition
{
    Self = 0,
    Ground = 1,
    Sky = 2,
}
public abstract class ProjectileSO : ItemSO
{
    public GameObject prefab;

    [Header("Base stats")]
    public float speed = 3f;
    public float lifetime = 10f;
    public int amountToSpawn = 1;

    public ProjectileAttackSO attackSO;
    public DamageData damageData;
 
    public EmitStartingPosition emitStartingPosition;

    public abstract bool CanEmit(int level);

}
