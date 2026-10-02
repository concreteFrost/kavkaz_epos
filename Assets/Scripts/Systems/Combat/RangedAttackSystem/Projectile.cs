using FMOD.Studio;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private Vector3 currentDir;
    private float aliveTime;
    private float currLifeTime;

    public ProjectileData data;

    [SerializeField] private DamageCollider damageCollider;
    [SerializeField] private GameObject lifetimeParticles;
    [SerializeField] private GameObject hitParticles;

    private Transform emitterPosition;
    private EventInstance audioEvent;

    private bool isDestroying;

    private void Update()
    {
        if (isDestroying)
            return;

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

        transform.position += velocity * Time.deltaTime;
    }

    public void Init(ProjectileData data)
    {
        this.data = data;

        aliveTime = 0f;
        currLifeTime = 0f;
        isDestroying = false;

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
        audioEvent = AudioEventPlayer.Play3D(
            data.ev_audio,
            gameObject,
            "ProjectileState",
            1f
        );
    }

    private void OnHit()
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

    private void SetDestroyAudioState(bool playHit = false)
    {
        if (!audioEvent.isValid())
            return;

        if (playHit)
        {
            AudioEventPlayer.SetParameter(audioEvent,"ProjectileState",2f);
            audioEvent.release();
        }

        else
        {
            AudioEventPlayer.StopAndRelease(audioEvent, false);
        }

        

    }

    private void PerformDestroy()
    {
        if (isDestroying)
            return;

        isDestroying = true;

        StartCoroutine(DestroyCoroutine());
    }

    private void ActivateLifetimeParticles()
    {
        lifetimeParticles.SetActive(true);
        hitParticles.SetActive(false);
    }

    private void ActivateHitParticles()
    {
        lifetimeParticles.SetActive(false);
        hitParticles.SetActive(true);
    }

    private System.Collections.IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}