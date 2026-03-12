using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusEffectSO), true)]
public class StatusEffectSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StatusEffectSO item = (StatusEffectSO)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Recreate GUID"))
        {
            item.id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(item);
        }
    }
}
