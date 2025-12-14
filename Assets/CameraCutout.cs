using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraCutout : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask cutoutLayer;

    [Header("Cutout Settings")]
    [SerializeField] private float cutoutSize = 7f;
    [SerializeField] private float cutoutSpeed = 5f;
    [SerializeField] private float neighborRadius = 6f;

    private Camera cam;

    private Dictionary<Material, float> targetValues = new Dictionary<Material, float>();
    private Dictionary<Renderer, float[]> originalCutoutSizes = new Dictionary<Renderer, float[]>();
    private HashSet<Renderer> currentHits = new HashSet<Renderer>();

    const string OPACITY = "_Opacity";

    private void Awake()
    {
        cam = GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateCutouts();
        ApplyCutoutValues();
    }

    private void UpdateCutouts()
    {
        currentHits.Clear();

        Vector3 dir = (target.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(cam.transform.position, target.position);

        // RaycastAll от камеры к игроку
        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, dir, distance, cutoutLayer);

        foreach (var hit in hits)
        {
            Renderer mainRenderer = hit.collider.GetComponent<Renderer>();
            if (mainRenderer != null)
            {
                ProcessCutout(mainRenderer, hit.point);
            }
        }

        // Объекты, которые больше не попали
        foreach (var renderer in originalCutoutSizes.Keys.ToList())
        {
            if (!currentHits.Contains(renderer))
            {
                Material[] materials = renderer.materials;
                float[] originals = originalCutoutSizes[renderer];

                bool allReachedOriginal = true;
                for (int i = 0; i < materials.Length; i++)
                {
                    // Ставим цель для плавного возврата
                    targetValues[materials[i]] = originals[i];

                    // Проверяем, достиг ли материал оригинала
                    if (!Mathf.Approximately(materials[i].GetFloat(OPACITY), originals[i]))
                        allReachedOriginal = false;
                }

                // Только когда все материалы вернулись к оригиналу, удаляем Renderer
                if (allReachedOriginal)
                    originalCutoutSizes.Remove(renderer);
            }
        }
    }


    private void ProcessCutout(Renderer mainRenderer, Vector3 hitPoint)
    {
        SetTargetCutout(mainRenderer, cutoutSize);
        currentHits.Add(mainRenderer);

        if (!originalCutoutSizes.ContainsKey(mainRenderer))
        {
            float[] sizes = new float[mainRenderer.materials.Length];
            for (int i = 0; i < mainRenderer.materials.Length; i++)
                sizes[i] = mainRenderer.materials[i].GetFloat(OPACITY);
            originalCutoutSizes.Add(mainRenderer, sizes);
        }

        // Соседние объекты
        Collider[] neighbors = Physics.OverlapSphere(hitPoint, neighborRadius, cutoutLayer);
        foreach (var col in neighbors)
        {
            Renderer r = col.GetComponent<Renderer>();
            if (r == null || r == mainRenderer) continue;

            float dist = Vector3.Distance(r.bounds.center, hitPoint);
            float effect = Mathf.Clamp01(1f - dist / neighborRadius) * cutoutSize;

            SetTargetCutout(r, effect);
            currentHits.Add(r);

            if (!originalCutoutSizes.ContainsKey(r))
            {
                float[] sizes = new float[r.materials.Length];
                for (int i = 0; i < r.materials.Length; i++)
                    sizes[i] = r.materials[i].GetFloat(OPACITY);
                originalCutoutSizes.Add(r, sizes);
            }
        }
    }

    private void SetTargetCutout(Renderer renderer, float target)
    {
        foreach (var mat in renderer.materials)
            targetValues[mat] = target;
    }

    private void ApplyCutoutValues()
    {
        foreach (var mat in targetValues.Keys.ToList())
        {
            float current = mat.GetFloat(OPACITY);
            float target = targetValues[mat];
            float next = Mathf.MoveTowards(current, target, cutoutSpeed * Time.deltaTime);
            mat.SetFloat(OPACITY, next);

            if (Mathf.Approximately(next, target))
                targetValues.Remove(mat);
        }
    }
}
