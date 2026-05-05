using System.Collections.Generic;
using UnityEngine;

public class CameraCutout : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask cutoutLayer;

    [Header("Cutout Settings")]
    [SerializeField] private float cutoutSize = 7f;
    [SerializeField] private float cutoutSpeed = 2f;

    private Camera cam;

    // кеши
    private Dictionary<Transform, Renderer[]> rendererCache = new();
    private Dictionary<Renderer, Material[]> materialsCache = new();

    private Dictionary<Material, float> targetValues = new();
    private Dictionary<Renderer, float[]> originalCutoutSizes = new();
    private HashSet<Renderer> currentHits = new();

    // буферы без аллокаций
    private RaycastHit[] hitsBuffer = new RaycastHit[32];
    private List<Renderer> tempRenderers = new();
    private List<Material> tempMaterials = new();

    const string OPACITY = "_Dissolve";

    private void Awake()
    {
        cam = GetComponent<Camera>();
 
    }

    private void Update()
    {
        UpdateCutouts();
        ApplyCutoutValues();
    }

    private void UpdateCutouts()
    {
        currentHits.Clear();

        Vector3 origin = cam.transform.position;
        Vector3 dir = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);

        int hitCount = Physics.RaycastNonAlloc(origin, dir, hitsBuffer, distance, cutoutLayer);

        for (int h = 0; h < hitCount; h++)
        {
            var hit = hitsBuffer[h];
            var root = hit.collider.transform;

            Renderer[] renderers;

            if (!rendererCache.TryGetValue(root, out renderers))
            {
                var lodGroup = root.GetComponentInParent<LODGroup>();

                if (lodGroup != null)
                    renderers = lodGroup.GetComponentsInChildren<Renderer>();
                else
                    renderers = root.GetComponentsInChildren<Renderer>();

                rendererCache[root] = renderers;
            }

            for (int i = 0; i < renderers.Length; i++)
                ProcessCutout(renderers[i]);
        }

        // обработка возврата
        tempRenderers.Clear();
        foreach (var r in originalCutoutSizes.Keys)
            tempRenderers.Add(r);

        foreach (var renderer in tempRenderers)
        {
            if (currentHits.Contains(renderer))
                continue;

            var materials = GetMaterials(renderer);
            var originals = originalCutoutSizes[renderer];

            bool allReachedOriginal = true;

            for (int i = 0; i < materials.Length; i++)
            {
                var mat = materials[i];
                if (!mat.HasProperty(OPACITY)) continue;

                targetValues[mat] = originals[i];

                if (!Mathf.Approximately(mat.GetFloat(OPACITY), originals[i]))
                    allReachedOriginal = false;
            }

            if (allReachedOriginal)
                originalCutoutSizes.Remove(renderer);
        }
    }

    private void ProcessCutout(Renderer renderer)
    {
        currentHits.Add(renderer);

        var materials = GetMaterials(renderer);

        SetTargetCutout(materials, cutoutSize);

        if (!originalCutoutSizes.ContainsKey(renderer))
        {
            float[] sizes = new float[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].HasProperty(OPACITY))
                    sizes[i] = materials[i].GetFloat(OPACITY);
            }

            originalCutoutSizes.Add(renderer, sizes);
        }
    }

    private Material[] GetMaterials(Renderer renderer)
    {
        if (!materialsCache.TryGetValue(renderer, out var materials))
        {
            materials = renderer.materials;
            materialsCache[renderer] = materials;
        }
        return materials;
    }

    private void SetTargetCutout(Material[] materials, float target)
    {
        for (int i = 0; i < materials.Length; i++)
            targetValues[materials[i]] = target;
    }

    private void ApplyCutoutValues()
    {
        tempMaterials.Clear();
        foreach (var m in targetValues.Keys)
            tempMaterials.Add(m);

        foreach (var mat in tempMaterials)
        {
            if (!mat.HasProperty(OPACITY))
            {
                targetValues.Remove(mat);
                continue;
            }

            float current = mat.GetFloat(OPACITY);
            float target = targetValues[mat];

            float next = Mathf.MoveTowards(current, target, cutoutSpeed * Time.deltaTime);
            mat.SetFloat(OPACITY, next);

            if (Mathf.Approximately(next, target))
                targetValues.Remove(mat);
        }
    }
}