using UnityEngine;

public class DestrucviteProjectile : Projectile
{
    [SerializeField] private GameObject debrisPrefab;


    protected override void OnHit()
    {
        damageCollider.DisableCollider();
        DeactivateAllParticles();

        SetDestroyAudioState(true);

        PerformDestroy();

        GetComponent<MeshRenderer>().enabled = false;
        

        if (debrisPrefab == null) return;

        GameObject go = Instantiate(debrisPrefab);
        go.transform.position = transform.position;

        ProjectileDebris debris = go.GetComponent<ProjectileDebris>();

        debris.TrySpawn();
    }
}
