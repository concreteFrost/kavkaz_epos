using System.Collections.Generic;
using UnityEngine;

public enum EmitStartingPosition
{
    Self = 0,
    Ground = 1,
    Sky = 2,
}
public abstract class ProjectileSO : ItemSO , IItemStats
{
    [Header("Prefab")]
    [Tooltip("Prefab снар€да, который будет создаватьс€ при использовании способности.")]
    public GameObject prefab;

    [Header("Damage")]
    [Tooltip("Ѕазовый урон снар€да до применени€ множителей (бафов, крита и т.п.).")]
    [SerializeField] float baseDamage;

    [Tooltip("ƒополнительные параметры урона: тип, эффекты и друга€ логика обработки.")]
    public DamageData damageData;

    [Header("Attack Logic")]
    [Tooltip("ScriptableObject, определ€ющий как именно снар€д наносит урон (single target, AoE, piercing и т.д.).")]
    public ProjectileAttackSO attackSO;

    [Header("Emission")]
    [Tooltip("—колько снар€дов создаЄтс€ за одно использование способности.")]
    public int amountToSpawn = 1;

    [Tooltip("«адержка между созданием снар€дов, если их больше одного.")]
    public float spawnDelay = 0.1f;

    [Tooltip("ќткуда по€вл€етс€ снар€д: из позиции персонажа, с земли или с неба.")]
    public EmitStartingPosition emitStartingPosition;

    [Header("Movement")]
    [Tooltip("“ип движени€ снар€да (пр€мой, самонавод€щийс€, баллистический и т.д.).")]
    public ProjectileMoveSO moveSO;

    [Tooltip("—корость движени€ снар€да.")]
    public float speed = 3f;

    [Tooltip("ћаксимальное врем€ жизни снар€да в секундах. ѕосле этого он уничтожаетс€.")]
    public float lifetime = 10f;

    public float GetBaseDamage() => baseDamage;

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat(ItemStatType.baseDamage, GetBaseDamage(), ItemStatFormatType.flat),
       
    };


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
        data.damageData.SetFinalDamage(GetBaseDamage(), attackMultiplier);   

        GameObject clone = Instantiate(prefab, startingPosition, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();
        projectile.Init(data);
      
    }

}
