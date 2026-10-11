using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraCutout : MonoBehaviour
{
    [Header("Ссылки")]
    private Transform target;
    [SerializeField] private LayerMask cutoutLayer;

    [Header("Настройки выреза")]
    [SerializeField] private float cutoutSize = 0.75f;
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

    private static readonly int OPACITY = Shader.PropertyToID("_Dissolve");


    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    private void OnCameraUpdated(CinemachineBrain brain)
    {
        if (target == null || cam == null || brain.OutputCamera != cam) return;

        UpdateCutouts();
        ApplyCutoutValues();
    }

    public void Init(Transform target, Camera cam)
    {
        this.cam = cam;
        this.target = target;   
    }

    private void UpdateCutouts()
    {
        currentHits.Clear();

        // Луч из точки игрока обнаруживает препятствие и при камере внутри его коллайдера.
        Vector3 origin = target.position;
        Vector3 offset = cam.transform.position - origin;
        float distance = offset.magnitude;
        Vector3 dir = distance > 0f ? offset / distance : Vector3.zero;

        int hitCount = Physics.RaycastNonAlloc(origin, dir, hitsBuffer, distance, cutoutLayer);

        for (int h = 0; h < hitCount; h++)
        {
            var hit = hitsBuffer[h];
            var root = hit.transform;

            Renderer[] renderers;

            if (!rendererCache.TryGetValue(root, out renderers))
            {
                var lodGroup = root.GetComponent<LODGroup>();

                if (lodGroup != null)
                {
                    renderers = lodGroup.GetComponentsInChildren<Renderer>();
                  
                }
                   
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
        if (!currentHits.Add(renderer)) return;

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
        target = Mathf.Clamp01(target);
        for (int i = 0; i < materials.Length; i++)
        {
            var material = materials[i];
            if (!material.HasProperty(OPACITY)) continue;

            if (Mathf.Approximately(material.GetFloat(OPACITY), target))
                targetValues.Remove(material);
            else
                targetValues[material] = target;
        }
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
            if (next != current)
                mat.SetFloat(OPACITY, next);

            if (Mathf.Approximately(next, target))
                targetValues.Remove(mat);
        }
    }
}
