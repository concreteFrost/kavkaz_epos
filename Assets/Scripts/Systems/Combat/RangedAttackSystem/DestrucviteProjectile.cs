using UnityEngine;

public class DestrucviteProjectile : Projectile
{
    [SerializeField] private GameObject debrisPrefab;
    [SerializeField, Min(0f)] private float gravity = 2f;

    protected override float Gravity => gravity;

    private MeshRenderer projectileRenderer;
    private bool initialVisibility;
    private bool rendererCached;

    public override void ResetForPool()
    {
        base.ResetForPool();
        if (!rendererCached)
        {
            projectileRenderer = GetComponent<MeshRenderer>();
            initialVisibility = projectileRenderer != null && projectileRenderer.enabled;
            rendererCached = true;
        }
        if (projectileRenderer != null) projectileRenderer.enabled = initialVisibility;
    }

    protected override void OnHit()
    {
        damageCollider.DisableCollider();
        DeactivateAllParticles();

        SetDestroyAudioState(true);

        PerformDestroy();

        if (projectileRenderer != null) projectileRenderer.enabled = false;
        

        if (debrisPrefab == null) return;

        ProjectilePoolManager.Instance.SpawnDebris(debrisPrefab, transform.position, Quaternion.identity);
    }
}
