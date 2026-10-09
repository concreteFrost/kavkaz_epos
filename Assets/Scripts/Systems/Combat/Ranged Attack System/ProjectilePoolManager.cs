using System;
using System.Collections.Generic;
using UnityEngine;

// Instances are prepared under an inactive parent, before Awake/OnEnable and physics.
public sealed class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance { get; private set; }

    private sealed class Pool
    {
        public readonly Stack<GameObject> inactive = new Stack<GameObject>();
    }

    private readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
    private readonly Dictionary<GameObject, Pool> instances = new Dictionary<GameObject, Pool>();
    private readonly HashSet<GameObject> active = new HashSet<GameObject>();
    private Transform storage;
    private bool loading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            enabled = false;
            return;
        }
        Instance = this;
        var root = new GameObject("Inactive Projectiles");
        root.SetActive(false);
        root.transform.SetParent(transform, false);
        storage = root.transform;
    }

    public void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation, ProjectileData data)
    {
        Spawn(prefab, position, rotation, go =>
        {
            var projectile = go.GetComponent<Projectile>();
            if (projectile == null)
                throw new InvalidOperationException("Projectile prefab requires a Projectile component: " + prefab.name);
            ((IProjectile)projectile).Init(data);
        });
    }

    public void SpawnDebris(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var go = Spawn(prefab, position, rotation, instance =>
        {
            var debris = instance.GetComponent<ProjectileDebris>();
            if (debris == null)
                throw new InvalidOperationException("Debris prefab requires ProjectileDebris: " + prefab.name);
            debris.PrepareForSpawn();
        });
        if (go != null)
            go.GetComponent<ProjectileDebris>().TrySpawn();
    }

    private GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Action<GameObject> prepare)
    {
        if (loading) return null;
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new Pool();
            pools.Add(prefab, pool);
        }

        GameObject go = null;
        while (pool.inactive.Count > 0 && go == null)
            go = pool.inactive.Pop();
        if (go == null)
        {
            go = Instantiate(prefab, storage);
            go.SetActive(false);
            instances.Add(go, pool);
        }

        go.transform.SetPositionAndRotation(position, rotation);
        active.Add(go);
        try
        {
            prepare(go);
            go.transform.SetParent(transform, true);
            go.SetActive(true);
            return go;
        }
        catch
        {
            Return(go);
            throw;
        }
    }

    public void Return(GameObject go)
    {
        if (go == null || !active.Remove(go)) return;
        ResetInstance(go);
        go.SetActive(false);
        go.transform.SetParent(storage, false);
        instances[go].inactive.Push(go);
    }

    private static void ResetInstance(GameObject go)
    {
        if (go.TryGetComponent<Projectile>(out var projectile))
            projectile.ResetForPool();
        if (go.TryGetComponent<ProjectileDebris>(out var debris))
            debris.ResetForPool();
    }

    public void BeginLoading()
    {
        loading = true;
        ClearPools();
    }

    public void EndLoading() => loading = false;

    public void ClearPools()
    {
        foreach (var go in instances.Keys)
        {
            if (go == null) continue;
            ResetInstance(go);
            go.SetActive(false);
            Destroy(go);
        }
        active.Clear();
        instances.Clear();
        pools.Clear();
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        ClearPools();
        Instance = null;
    }
}
