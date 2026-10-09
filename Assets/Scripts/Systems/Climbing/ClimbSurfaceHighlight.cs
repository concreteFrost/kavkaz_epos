using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Fits a depth-projected highlight to the climbing trigger without changing its collider.</summary>
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider), typeof(MeshFilter), typeof(MeshRenderer))]
public sealed class ClimbSurfaceHighlight : MonoBehaviour
{
    [Tooltip("Extra projection reach on each side, in world metres. Does not enlarge the trigger.")]
    [SerializeField, Min(0f)] private float projectionPadding = 0.08f;

    private static readonly int VolumeCenterId = Shader.PropertyToID("_VolumeCenter");
    private static readonly int VolumeSizeId = Shader.PropertyToID("_VolumeSize");
    private BoxCollider surfaceCollider;
    private MeshRenderer projectionRenderer;
    private MaterialPropertyBlock properties;
    private Vector3 previousCenter;
    private Vector3 previousSize;
    private bool volumeApplied;

    private void OnEnable()
    {
        surfaceCollider = GetComponent<BoxCollider>();
        projectionRenderer = GetComponent<MeshRenderer>();
        properties = new MaterialPropertyBlock();
        projectionRenderer.shadowCastingMode = ShadowCastingMode.Off;
        projectionRenderer.receiveShadows = false;
        volumeApplied = false;
        UpdateVolume();
    }

    private void LateUpdate()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        if (surfaceCollider == null || projectionRenderer == null)
            return;

        projectionRenderer.enabled = surfaceCollider.enabled;
        Vector3 scale = transform.lossyScale;
        Vector3 localPadding = new Vector3(
            projectionPadding / Mathf.Max(Mathf.Abs(scale.x), 0.0001f),
            projectionPadding / Mathf.Max(Mathf.Abs(scale.y), 0.0001f),
            projectionPadding / Mathf.Max(Mathf.Abs(scale.z), 0.0001f));
        Vector3 size = surfaceCollider.size + localPadding * 2f;
        Vector3 center = surfaceCollider.center;

        if (volumeApplied && center == previousCenter && size == previousSize)
            return;

        projectionRenderer.GetPropertyBlock(properties);
        properties.SetVector(VolumeCenterId, center);
        properties.SetVector(VolumeSizeId, size);
        projectionRenderer.SetPropertyBlock(properties);
        // Vertex expansion must also be reflected in CPU-side frustum culling.
        projectionRenderer.localBounds = new Bounds(center, size);
        previousCenter = center;
        previousSize = size;
        volumeApplied = true;
    }

    private void OnDisable()
    {
        if (projectionRenderer != null)
            projectionRenderer.enabled = false;
    }
}
