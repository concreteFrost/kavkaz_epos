using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoDrawer
{

    public static void DrawWithCube(Color colorToUse, Transform transform,Vector3 size)
    {

        Color gizmoColor = colorToUse;

        // полупрозрачный куб
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.3f);
        Gizmos.DrawCube(transform.position, size);

        // (опционально) обводка, чтобы лучше читалось
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.9f);
        Gizmos.DrawWireCube(transform.position, size);

    }

    public static void DrawWithSphere(Color colorToUse, Transform transform, string label, float radius, int fontSize = 30)
    {
        float labelOffset = 1.2f;

        Color gizmoColor = colorToUse;
        Color textColor = colorToUse;
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);

#if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.7f);
        style.fontSize = fontSize;
        UnityEditor.Handles.Label(transform.position + Vector3.up * radius * labelOffset, label, style);
#endif
    }

    public static void DrawText(Transform transform, Color gizmoColor,string label, int fontSize=18)
    {
#if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        style.fontSize = fontSize;
        UnityEditor.Handles.Label(transform.position + Vector3.up * 3f, label,style);
#endif
    }


}

