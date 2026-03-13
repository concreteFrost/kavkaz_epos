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

    Transform emitterPosition;
    
    void Update()
    {
        Vector3 velocity = data.moveSO.Move(
            emitterPosition,
            transform,
            data.target,
            currentDir,
            data.speed,
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

        if (currLifeTime >= data.lifetime)
        {
            damageCollider.DisableCollider();
            PerformDestroy();
        }
           
    }

    public void Init(ProjectileData data)
    {
        this.data = data;
        currentDir = data.baseDir;
        emitterPosition = data.source.Source();

        if (damageCollider == null)
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }

        damageCollider.Init();
        damageCollider.EnableCollider(data.damageData, data.source.TargetsToIgnore, data.source.Source());

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


}
