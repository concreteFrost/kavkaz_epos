using UnityEditor;
using UnityEngine;

public class ShieldCreatorTool : BaseItemCreatorTool<ShieldSO>
{
    protected override string ItemFolder => $"{basePath}/Shields";

    protected override void DrawItem(ShieldSO item)
    {
        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();

        EditorGUILayout.BeginVertical("box");

        SerializedProperty defenceBonus = so.FindProperty("defenceBonus");
        EditorGUILayout.PropertyField(defenceBonus);    

        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();
    }
}
