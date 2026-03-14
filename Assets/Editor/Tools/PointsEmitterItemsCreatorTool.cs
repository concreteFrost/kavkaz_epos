using UnityEditor;
using UnityEngine;

public class PointsEmitterItemsCreatorTool : BaseItemCreatorTool<PointsEmitterItemSO>
{
    protected override string ItemFolder => $"{basePath}/PointsEmitter_Items/";

    public void DrawWindow()
    {
        DrawToolbar();
        DrawScrollView();
    }

    // Полный контент при раскрытии
    protected override void DrawItem(PointsEmitterItemSO item)
    {
        if (item == null) return;

        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();

        EditorGUILayout.BeginVertical("box");

        // Основной заголовок с id, именем и иконкой уже показан в BaseItemCreatorTool
        // Здесь только расширенный контент

        // Поля для оружейного предмета
        SerializedProperty topUpAmount = so.FindProperty("amount");
        EditorGUILayout.LabelField("Top up amount:");
        EditorGUILayout.PropertyField(topUpAmount, GUIContent.none);

        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();
    }
}