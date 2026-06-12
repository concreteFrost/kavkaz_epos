using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelHolder : MonoBehaviour
{
    public string objectName;
    public Color col;
    public bool isVisible = true;
    public int fontSize = 18;

    protected virtual void DrawGizmo()
    {
        GizmoDrawer.DrawText(transform, col, objectName, fontSize);
    }

    private void OnDrawGizmos()
    {
        if (isVisible)
            DrawGizmo();
    }
}


