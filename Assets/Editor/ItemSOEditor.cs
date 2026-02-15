using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSO),true)]
public class ItemSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemSO item = (ItemSO)target;

        GUILayout.Space(10);

        if(GUILayout.Button("Recreate GUID"))
        {
            item.id = Guid.NewGuid().ToString();    
            EditorUtility.SetDirty(item);   
        }
    }
}
