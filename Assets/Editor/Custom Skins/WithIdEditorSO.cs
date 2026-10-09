using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WithIdSO), true)]
public class WithIdEditorSO : Editor
{
    public override void OnInspectorGUI()
    {
        WithIdSO obj = (WithIdSO)target;

        serializedObject.Update();

        // Поле GUID только для чтения
        EditorGUILayout.LabelField("Item ID");
        EditorGUI.BeginDisabledGroup(true);  // блокируем редактирование
        EditorGUILayout.TextField(obj.id);
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        // Стандартные поля
        DrawDefaultInspector();

        GUILayout.Space(10);

       

        // Кнопка пересоздания GUID
        if (GUILayout.Button("Recreate GUID"))
        {
            Undo.RecordObject(obj, "Regenerate Item GUID");
            obj.id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(obj);
        }

        serializedObject.ApplyModifiedProperties();
    }
}