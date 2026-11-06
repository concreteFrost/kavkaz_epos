using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Создаёт локальный просвет в рельефе перед игроком, сохраняя положение камеры.
/// </summary>
public class TerrainCameraCutout : MonoBehaviour
{
    [SerializeField, Tooltip("Вариант Terrain/Lit с локальным просветом перед игроком.")]
    private Shader terrainShader;

    [Header("Маска TransparentObject")]
    [SerializeField, Range(0f, 1f), Tooltip("Значение _CutoutSize из материала TransparentObject.")]
    private float maskSize = 0f;
    [SerializeField, Min(0f), Tooltip("Множитель узла Multiply перед Dither в исходном графе.")]
    private float maskContrast = 7.45f;
    [SerializeField, Tooltip("Значение узла Add после Dither в исходном графе.")]
    private float alphaOffset = 0.1f;
    [SerializeField, Range(0f, 1f), Tooltip("Значение _AlphaThreshold из материала TransparentObject.")]
    private float alphaThreshold = 0.61f;

    [Header("Ограничение по глубине")]
    [SerializeField, Min(0f), Tooltip("Отступ границы выреза перед целью камеры в метрах. За этой границей рельеф остаётся видимым.")]
    private float targetPadding = 0.25f;

    private const string SupportedShaderName = "Universal Render Pipeline/Terrain/Lit";
    private static readonly int SettingsId = Shader.PropertyToID("_TerrainCutoutSettings");
    private static readonly int AlphaId = Shader.PropertyToID("_TerrainCutoutAlpha");
    private static readonly int DepthId = Shader.PropertyToID("_TerrainCutoutDepth");

    private readonly Dictionary<Terrain, Material> originalMaterials = new();
    private readonly Dictionary<Terrain, Material> cutoutMaterials = new();
    private readonly Dictionary<Terrain, TerrainCollider> terrainColliders = new();
    private Transform target;
    private Camera outputCamera;
    private float cutoutSize;
    private float cutoutSpeed;
    private float currentDissolve;

    public void Init(Transform target, Camera outputCamera, float cutoutSize, float cutoutSpeed)
    {
        this.target = target;
        this.outputCamera = outputCamera;
        this.cutoutSize = cutoutSize;
        this.cutoutSpeed = cutoutSpeed;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        PrepareTerrains();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        Shader.SetGlobalVector(SettingsId, Vector4.zero);

        foreach (var entry in cutoutMaterials)
        {
            if (entry.Key != null && entry.Key.materialTemplate == entry.Value)
                entry.Key.materialTemplate = originalMaterials[entry.Key];

            Destroy(entry.Value);
        }

        cutoutMaterials.Clear();
        originalMaterials.Clear();
        terrainColliders.Clear();
        currentDissolve = 0f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PrepareTerrains();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Камера переживает смену уровня, поэтому освобождаем копии материалов выгруженного Terrain.
        var terrains = new List<Terrain>(cutoutMaterials.Keys);
        foreach (var terrain in terrains)
        {
            if (terrain != null) continue;

            Destroy(cutoutMaterials[terrain]);
            cutoutMaterials.Remove(terrain);
            originalMaterials.Remove(terrain);
            terrainColliders.Remove(terrain);
        }
    }

    private void PrepareTerrains()
    {
        if (terrainShader == null) return;

        // Материалы сцены и пакета URP не изменяются: каждый Terrain получает временную копию.
        foreach (var terrain in Terrain.activeTerrains)
        {
            if (cutoutMaterials.ContainsKey(terrain)) continue;

            var original = terrain.materialTemplate;
            if (original == null || original.shader == null || original.shader.name != SupportedShaderName)
                continue;

            var material = new Material(original)
            {
                shader = terrainShader,
                hideFlags = HideFlags.DontSave
            };

            originalMaterials.Add(terrain, original);
            cutoutMaterials.Add(terrain, material);
            terrainColliders.Add(terrain, terrain.GetComponent<TerrainCollider>());
            terrain.materialTemplate = material;
        }
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera != outputCamera || target == null)
        {
            Shader.SetGlobalVector(SettingsId, Vector4.zero);
            return;
        }

        float desiredDissolve = IsTargetOccluded() ? Mathf.Clamp01(cutoutSize) : 0f;
        currentDissolve = Mathf.MoveTowards(currentDissolve, desiredDissolve, cutoutSpeed * Time.deltaTime);

        float cutoutDepth = -camera.worldToCameraMatrix.MultiplyPoint(target.position).z - targetPadding;
        Shader.SetGlobalVector(SettingsId, new Vector4(maskSize, currentDissolve, maskContrast, currentDissolve > 0f ? 1f : 0f));
        Shader.SetGlobalVector(AlphaId, new Vector4(alphaOffset, alphaThreshold, 0f, 0f));
        Shader.SetGlobalFloat(DepthId, cutoutDepth);
    }

    private bool IsTargetOccluded()
    {
        Vector3 offset = outputCamera.transform.position - target.position;
        float distance = offset.magnitude;
        if (distance <= 0f) return false;

        // Та же точка и направление, что в CameraCutout: перекрытие только ног не вызывает просвет.
        var ray = new Ray(target.position, offset / distance);
        foreach (var collider in terrainColliders.Values)
        {
            if (collider != null && collider.enabled && collider.Raycast(ray, out _, distance))
                return true;
        }

        return false;
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == outputCamera)
            Shader.SetGlobalVector(SettingsId, Vector4.zero);
    }
}
