using UnityEngine;

public enum EmitStartingPosition
{
    Self = 0,
    Ground = 1,
    Sky = 2,
}
public abstract class ProjectileSO : ItemSO
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Damage")]
    [Tooltip("Base damage before multipliers")]
    public float baseDamage = 1f;

    public DamageData damageData;

    [Header("Movement")]
    public float speed = 3f;
    public float lifetime = 10f;
    public ProjectileMoveSO moveSO;

    [Header("Emission")]
    public int amountToSpawn = 1;

    [Tooltip("Delay between projectiles if multiple are spawned")]
    public float spawnDelay = 0.1f;

    public EmitStartingPosition emitStartingPosition;

    [Header("Attack Logic")]
    public ProjectileAttackSO attackSO;

    public abstract bool CanEmit(int level);


    public void CreateProjectile(Vector3 startingPosition, IDamagable target,IAttackSource source ,Vector3 baseDir, float attackMultiplier)
    {

        ProjectileData data = new ProjectileData();
        
        data.target = target;     
        data.speed = speed;
        data.lifetime = lifetime;
        data.moveSO = moveSO;
        data.source = source;   
        data.baseDir = baseDir;

        data.damageData = damageData;
        data.damageData.SetFinalDamage(baseDamage, attackMultiplier);   

        GameObject clone = Instantiate(prefab, startingPosition, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();
        projectile.Init(data);
      
    }

}
