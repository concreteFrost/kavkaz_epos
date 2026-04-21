using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelHolder : MonoBehaviour
{
    public string objectName;
    public Color col;
    public bool isVisible = true;
    public int fontSize = 18;

    private void OnDrawGizmos()
    {
        if(isVisible)   
        GizmoDrawer.DrawText(transform, col, objectName, fontSize);
    }
}
