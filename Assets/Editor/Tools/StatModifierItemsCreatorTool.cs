using UnityEditor;
using UnityEngine;

public class StatModifierItemsCreatorTool : BaseItemCreatorTool<StatModifierItemSO>
{
    protected override string ItemFolder => $"{basePath}/StatusEffect_Items/";

    public void DrawWindow()
    {
        DrawToolbar();
        DrawScrollView();
    }

    // Полный контент раскрываемого элемента
    protected override void DrawItem(StatModifierItemSO item)
    {
        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();

        EditorGUILayout.BeginVertical("box");

        // Основная информация
        DrawItemMainInfo(item, so);

        // Счётчики эффектов
        DrawEffectsCount(item);

        // Эффекты
        DrawEffectEditor(item, so);

        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();
    }

    private void DrawItemMainInfo(StatModifierItemSO item, SerializedObject so)
    {
        EditorGUILayout.BeginVertical();

        // Параметры item
        SerializedProperty consumeAnimation = so.FindProperty("consumableAnimation");

        EditorGUILayout.LabelField("Consume Animation:");
        EditorGUILayout.ObjectField(consumeAnimation, GUIContent.none);

        EditorGUILayout.EndVertical();
    }

    private void DrawEffectEditor(StatModifierItemSO item, SerializedObject so)
    {
        SerializedProperty effectData = so.FindProperty("effectData");
        if (effectData != null)
        {
            EditorGUILayout.PropertyField(effectData, true);
        }
    }

    private void DrawEffectsCount(StatModifierItemSO item)
    {
        EditorGUILayout.BeginVertical();

        int effectCount = item.effectData?.effects?.Count ?? 0;
        EditorGUILayout.LabelField($"Activate Effects: {effectCount}");

        int cancelsEffectsCount = item.effectData?.effectsToCancel?.Count ?? 0;
        EditorGUILayout.LabelField($"Cancels Effects: {cancelsEffectsCount}");

        EditorGUILayout.EndVertical();
    }
}