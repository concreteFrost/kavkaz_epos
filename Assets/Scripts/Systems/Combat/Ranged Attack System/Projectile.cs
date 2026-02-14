using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{

    public ProjectileData data;
    float currLifeTime; //текущий жизненый цикл
    [SerializeField] DamageCollider damageCollider;


    void Update()
    {
        Vector3 velocity = data.direction.MoveBehaviour.Move(
            transform,
            data.target,
            data.direction.baseDir,
            data.projectileSO.speed
        );

        transform.position += velocity * Time.deltaTime;

        currLifeTime += Time.deltaTime;
        if (currLifeTime >= data.projectileSO.lifetime || damageCollider.isAttackRegistered)
        {
            damageCollider.DisableCollider();
            Destroy(gameObject);
        }
           
    }

    public void Init(ProjectileData data)
    {
        this.data = data;
        if(damageCollider == null)
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }
        damageCollider.Init();
        damageCollider.EnableCollider(data.projectileSO.damageData, data.attackSource.TargetsToIgnore, data.attackSource.Source());
       
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Projectile>() != null) return;

    //    if (TryGetDamagable(other, out var damagable))
    //    {
    //        ApplyDamage(data, damagable);
    //    }

       
    //    Destroy(gameObject);
    //}

    //protected bool TryGetDamagable(Collider other, out IDamagable damagable)
    //{
    //    damagable = other.GetComponentInChildren<IDamagable>() ?? other.GetComponent<IDamagable>();
    //    return damagable != null;
    //}

    //protected virtual void ApplyDamage(ProjectileData data,IDamagable target)
    //{
    //    target.TakeDamage(data.damageData, data.source);
    //}
}
