using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSO), true)]
public class ItemSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemSO item = (ItemSO)target;

        serializedObject.Update();

        // Поле GUID только для чтения
        EditorGUILayout.LabelField("Item ID");
        EditorGUI.BeginDisabledGroup(true);  // блокируем редактирование
        EditorGUILayout.TextField(item.id);
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        // Стандартные поля
        DrawDefaultInspector();

        GUILayout.Space(10);

       

        // Кнопка пересоздания GUID
        if (GUILayout.Button("Recreate GUID"))
        {
            Undo.RecordObject(item, "Regenerate Item GUID");
            item.id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(item);
        }

        serializedObject.ApplyModifiedProperties();
    }
}