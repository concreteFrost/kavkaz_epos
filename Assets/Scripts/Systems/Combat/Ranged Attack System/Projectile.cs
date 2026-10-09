using FMOD.Studio;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private Vector3 currentDir;
    private float aliveTime;
    private float currLifeTime;
    private Vector3 gravityVelocity;

    protected virtual float Gravity => 0f;

    public ProjectileData data;

    [SerializeField] protected DamageCollider damageCollider;
    [SerializeField] private GameObject lifetimeParticles;
    [SerializeField] private GameObject hitParticles;

    private Transform emitterPosition;
    private EventInstance audioEvent;

    private bool isDestroying;

    protected virtual void Update()
    {
        if (isDestroying)
            return;

        if (data == null) return;


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

        // Попадание
        if (damageCollider.isAttackRegistered)
        {
            OnHit();
            return;
        }

        // Истёк lifetime
        if (currLifeTime >= data.lifetime)
        {
            OnLifetimeEnd();
            return;
        }

        // Keep gravity separate from currentDir so movement strategies do not
        // feed the accumulated falling velocity back into their next update.
        Vector3 gravityAcceleration = Vector3.down * Gravity;
        float deltaTime = Time.deltaTime;
        transform.position += (velocity + gravityVelocity) * deltaTime
            + gravityAcceleration * (0.5f * deltaTime * deltaTime);
        gravityVelocity += gravityAcceleration * deltaTime;
    }

    public virtual void Init(ProjectileData data)
    {
        ResetForPool();
        this.data = data;

        aliveTime = 0f;
        currLifeTime = 0f;
        isDestroying = false;
        gravityVelocity = Vector3.zero;

        currentDir = data.baseDir;
        emitterPosition = data.source.Source();

        if (damageCollider == null)
            damageCollider = GetComponentInChildren<DamageCollider>();

        damageCollider.Init();

        damageCollider.EnableCollider(
            data.damageData,
            data.source.TargetsToIgnore,
            data.source
        );

        ActivateLifetimeParticles();

        // Projectile: Lifetime -> Destroy
        audioEvent = data.ev_audio.IsNull ? default : AudioEventPlayer.Play3D(
            data.ev_audio,
            gameObject,
            "ProjectileState",
            1f
        );
    }

    protected virtual void OnHit()
    {
        damageCollider.DisableCollider();

        ActivateHitParticles();

        SetDestroyAudioState(true);

        PerformDestroy();
    }

    private void OnLifetimeEnd()
    {
        damageCollider.DisableCollider();

        ActivateHitParticles();

        SetDestroyAudioState();

        PerformDestroy();
    }

    protected void SetDestroyAudioState(bool playHit = false)
    {
        if (!audioEvent.isValid())
            return;

        if (playHit)
        {
            AudioEventPlayer.SetParameter(audioEvent,"ProjectileState",2f);
            // Keep ownership until return so loading can stop the hit sound.
        }

        else
        {
            StopAudio();
        }

    }

    protected void PerformDestroy()
    {
        if (isDestroying)
            return;

        isDestroying = true;

        StartCoroutine(DestroyCoroutine());
    }

    private void ActivateLifetimeParticles()
    {
        SetParticles(lifetimeParticles, true);
        SetParticles(hitParticles, false);
    }

    private void ActivateHitParticles()
    {
        SetParticles(lifetimeParticles, false);
        SetParticles(hitParticles, true);
    }

    protected void DeactivateAllParticles()
    {
        SetParticles(lifetimeParticles, false);
        SetParticles(hitParticles, false);
    }

    private ParticleSystem[] particles;
    private TrailRenderer[] trails;

    private void CacheEffects()
    {
        if (particles == null) particles = GetComponentsInChildren<ParticleSystem>(true);
        if (trails == null) trails = GetComponentsInChildren<TrailRenderer>(true);
    }

    private static void SetParticles(GameObject root, bool play)
    {
        if (root == null) return;
        foreach (var system in root.GetComponentsInChildren<ParticleSystem>(true))
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        root.SetActive(play);
        if (play)
            foreach (var system in root.GetComponentsInChildren<ParticleSystem>(true))
                if (system.gameObject.activeSelf) system.Play(true);
    }

    private void StopAudio()
    {
        if (audioEvent.isValid())
        {
            FMODUnity.RuntimeManager.DetachInstanceFromGameObject(audioEvent);
            AudioEventPlayer.StopAndRelease(audioEvent, false);
        }
        audioEvent = default;
    }

    public virtual void ResetForPool()
    {
        StopAllCoroutines();
        StopAudio();
        if (damageCollider == null) damageCollider = GetComponentInChildren<DamageCollider>(true);
        if (damageCollider != null) damageCollider.DisableCollider();
        CacheEffects();
        foreach (var system in particles)
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        foreach (var trail in trails) trail.Clear();
        DeactivateAllParticles();
        data = null;
        emitterPosition = null;
        currentDir = Vector3.zero;
        gravityVelocity = Vector3.zero;
        aliveTime = currLifeTime = 0f;
        isDestroying = true;
    }

    protected virtual void OnDisable() => ResetForPool();
    protected virtual void OnDestroy() => StopAudio();
    private System.Collections.IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3f);

        ProjectilePoolManager.Instance.Return(gameObject);
    }
}
