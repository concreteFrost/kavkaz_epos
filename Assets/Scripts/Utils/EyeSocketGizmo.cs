using UnityEngine;

/// <summary>
/// Показывает полупрозрачный сектор от глаз по направлению корпуса персонажа.
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Отладка/Направление сокета глаз")]
public class EyeSocketGizmo : MonoBehaviour
{
    [SerializeField, Tooltip("Настройки зрения врага. Если назначены, радиус, угол и препятствия берутся из них.")]
    private FovDataSO fovData;

    [SerializeField, Min(0f), Tooltip("Радиус сектора в мировых единицах, если настройки зрения не назначены.")]
    private float lineLength = 5f;

    [SerializeField, Range(0f, 360f), Tooltip("Угол сектора обзора, если настройки зрения не назначены.")]
    private float viewAngle = 70f;

    [SerializeField, Tooltip("Слои препятствий, обрезающих сектор, если настройки зрения не назначены. Исключите слой самого персонажа.")]
    private LayerMask obstacleMask;

    [SerializeField, Range(4, 256), Tooltip("Количество лучей для построения сектора. Большее значение делает границу точнее.")]
    private int segments = 64;

    [SerializeField, Range(0f, 1f), Tooltip("Прозрачность синей заливки сектора обзора.")]
    private float fillOpacity = 0.15f;

    [SerializeField, Min(1f), Tooltip("Толщина линии направления взгляда в пикселях.")]
    private float lineWidth = 3f;

#if UNITY_EDITOR
    private Vector3[] outline;
    private readonly Vector3[] triangle = new Vector3[3];
#endif

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!enabled || UnityEditor.SceneView.currentDrawingSceneView == null) return;

        float radius = fovData != null ? fovData.viewRadius : lineLength;
        float angle = Mathf.Clamp(fovData != null ? fovData.viewAngle : viewAngle, 0f, 360f);
        LayerMask mask = fovData != null ? fovData.obstacleMask : obstacleMask;
        if (radius <= 0f) return;

        int segmentCount = Mathf.Clamp(segments, 4, 256);
        if (outline == null || outline.Length != segmentCount + 3)
            outline = new Vector3[segmentCount + 3];

        Vector3 origin = transform.position;
        var locator = GetComponentInParent<BaseHumanoidAiServiceLocator>();
        if (locator == null) return;

        var locatorData = new UnityEditor.SerializedObject(locator);
        var motor = locatorData.FindProperty("motor").objectReferenceValue as BaseHumanoidMotor;
        if (motor == null) return;

        Vector3 forward = motor.transform.forward;
        Vector3 up = motor.transform.up;
        outline[0] = origin;
        outline[outline.Length - 1] = origin;

        for (int i = 0; i <= segmentCount; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(-angle / 2f + angle * i / segmentCount, up) * forward;
            outline[i + 1] = GetEndPoint(origin, direction, radius, mask);
        }

        var previousDepthTest = UnityEditor.Handles.zTest;
        try
        {
            UnityEditor.Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            using (new UnityEditor.Handles.DrawingScope(new Color(0f, 0f, 1f, fillOpacity)))
            {
                triangle[0] = origin;
                for (int i = 1; i <= segmentCount; i++)
                {
                    triangle[1] = outline[i];
                    triangle[2] = outline[i + 1];
                    UnityEditor.Handles.DrawAAConvexPolygon(triangle);
                }
            }

            using (new UnityEditor.Handles.DrawingScope(Color.blue))
            {
                UnityEditor.Handles.DrawAAPolyLine(lineWidth, outline);
                UnityEditor.Handles.DrawLine(
                    origin,
                    GetEndPoint(origin, forward, radius, mask),
                    lineWidth);
            }
        }
        finally
        {
            UnityEditor.Handles.zTest = previousDepthTest;
        }
#endif
    }

#if UNITY_EDITOR
    private static Vector3 GetEndPoint(Vector3 origin, Vector3 direction, float radius, LayerMask mask)
    {
        return Physics.Raycast(origin, direction, out RaycastHit hit, radius, mask, QueryTriggerInteraction.Ignore)
            ? hit.point
            : origin + direction * radius;
    }
#endif
}
