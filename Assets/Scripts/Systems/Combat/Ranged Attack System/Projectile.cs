using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private Vector3 currentDir;
    private float aliveTime = 0;

    public ProjectileData data;
    float currLifeTime; //текущий жизненый цикл
    [SerializeField] DamageCollider damageCollider;

    [SerializeField] GameObject lifetimeParticles;
    [SerializeField] GameObject hitParticles;

    Coroutine destroyCoroutine = null;
    
    void Update()
    {
        Vector3 velocity = data.direction.MoveBehaviour.Move(
            transform,
            data.target,
            currentDir,
            data.projectileSO.speed,
            aliveTime
        );

        aliveTime += Time.deltaTime;
        currLifeTime += Time.deltaTime;
        currentDir = velocity.normalized;
        

        if (!damageCollider.isAttackRegistered)
        {
            transform.position += velocity * Time.deltaTime;
        }

        else if (damageCollider.isAttackRegistered)
        {
            damageCollider.DisableCollider();   
            ActivateHitParticles();
            PerformDestroy();
        }

        if (currLifeTime >= data.projectileSO.lifetime)
        {
            damageCollider.DisableCollider();
            PerformDestroy();
        }
           
    }

    public void Init(ProjectileData data)
    {
        this.data = data;
        currentDir = data.direction.baseDir;

        if (damageCollider == null)
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }

        damageCollider.Init();
        damageCollider.EnableCollider(data.projectileSO.damageData, data.attackSource.TargetsToIgnore, data.attackSource.Source());

        ActivateLifetimeParticles();

    }

    private void PerformDestroy()
    {
        if (destroyCoroutine != null) return;

        destroyCoroutine = StartCoroutine(DestroyCoroutine());  
    }

    private void ActivateLifetimeParticles()
    {
        lifetimeParticles.gameObject.SetActive(true);
        hitParticles.gameObject.SetActive(false);
    }

    private void ActivateHitParticles()
    {
        lifetimeParticles.gameObject.SetActive(false);
        hitParticles.gameObject.SetActive(true);
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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
