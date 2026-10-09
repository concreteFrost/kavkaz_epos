using System.Collections;
using UnityEngine;

public class ProjectileDebris : MonoBehaviour
{
    [SerializeField] float lifetime = 3f;
    [SerializeField] float scatterSpeed = 3f;

    private Rigidbody[] bodies;
    private Transform[] fragments;
    private Vector3[] positions;
    private Quaternion[] rotations;
    private ParticleSystem[] particles;
    private TrailRenderer[] trails;

    private void CacheFragments()
    {
        if (bodies != null) return;
        bodies = GetComponentsInChildren<Rigidbody>(true);
        fragments = GetComponentsInChildren<Transform>(true);
        positions = new Vector3[fragments.Length];
        rotations = new Quaternion[fragments.Length];
        for (int i = 0; i < fragments.Length; i++)
        {
            positions[i] = fragments[i].localPosition;
            rotations[i] = fragments[i].localRotation;
        }
        particles = GetComponentsInChildren<ParticleSystem>(true);
        trails = GetComponentsInChildren<TrailRenderer>(true);
    }

    public void PrepareForSpawn()
    {
        ResetForPool();
        for (int i = 0; i < fragments.Length; i++)
        {
            if (fragments[i] == transform) continue;
            fragments[i].SetLocalPositionAndRotation(positions[i], rotations[i]);
        }
    }

    public void TrySpawn()
    {
        CacheFragments();
        foreach (var rb in bodies)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Random.insideUnitSphere * scatterSpeed + Vector3.up * scatterSpeed;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
        }
        foreach (var system in particles)
            if (system.gameObject.activeInHierarchy) system.Play(true);
        StartCoroutine(CleanupCoroutine());
    }

    public void ResetForPool()
    {
        StopAllCoroutines();
        CacheFragments();
        foreach (var rb in bodies)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
            rb.Sleep();
        }
        foreach (var system in particles)
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        foreach (var trail in trails) trail.Clear();
    }

    private void OnDisable() => ResetForPool();

    private IEnumerator CleanupCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        ProjectilePoolManager.Instance.Return(gameObject);
    }
}
