using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ImpactPool
{
    public DamagableSurfaceSO impactVFXSo;
    public ParticleSystem prefab;

    [HideInInspector]
    public List<ParticleSystem> instances = new();
}

public class CombatVFXManager : MonoBehaviour
{
    [SerializeField] private List<ImpactPool> pools;

    private readonly Dictionary<DamagableSurfaceSO, ImpactPool> _poolLookup = new();

    public static Action<DamagableSurfaceSO, Vector3, Vector3> ImpactResolved;

    private void OnEnable()
    {
        ImpactResolved += PlayImpact;
    }

    private void OnDisable()
    {
        ImpactResolved -= PlayImpact;   
    }

    private void Awake()
    {
        foreach (var pool in pools)
        {
            if (!_poolLookup.ContainsKey(pool.impactVFXSo))
            {
                _poolLookup.Add(pool.impactVFXSo, pool);
            }
        }
    }

    public void PlayImpact(DamagableSurfaceSO impactVFX, Vector3 position, Vector3 normal)
    {
        if (impactVFX == null) return;

        var particle = GetParticle(impactVFX, position, normal);

        if (particle == null)
            return;

        particle.Play();
    }

    private ParticleSystem GetParticle(
        DamagableSurfaceSO impactVFX,
        Vector3 position,
        Vector3 normal)
    {
        if (!_poolLookup.TryGetValue(impactVFX, out var pool))
        {
            Debug.LogWarning($"No pool configured for {impactVFX}");
            return null;
        }

        // »щем свободную частицу
        foreach (var particle in pool.instances)
        {
            if (!particle.IsAlive(true))
            {
                particle.transform.SetPositionAndRotation(
                    position,
                    Quaternion.LookRotation(normal));

                return particle;
            }
        }

        // Ќе нашли Ч создаЄм новую
        var newParticle = Instantiate(
            pool.prefab,
            position,
            Quaternion.LookRotation(normal),
            transform);

        pool.instances.Add(newParticle);

        return newParticle;
    }
}