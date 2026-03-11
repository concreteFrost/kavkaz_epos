using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemViewerTool : EditorWindow
{
    private Vector2 scroll;
    private List<StatModifierItemSO> items = new();
    private Dictionary<StatModifierItemSO, bool> foldouts = new();

    private const string ITEM_FOLDER = "Assets/Resources/Items/Consumable_Items/";

    [MenuItem("Tools/Items Viewer/Stat Items Viewer")]
    public static void Open() => GetWindow<ItemViewerTool>("Stat Items Viewer");

    private void OnEnable()
    {
        RefreshItems();
    }

    private void RefreshItems()
    {
        items.Clear();
        foldouts.Clear();

        var guids = AssetDatabase.FindAssets("t:StatModifierItemSO");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<StatModifierItemSO>(path);
            items.Add(item);
            foldouts[item] = false;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Existing Stat Modifier Items", EditorStyles.boldLabel);

        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var item in items)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            // »конка
            GUILayout.Label(item.itemImage != null ? AssetPreview.GetAssetPreview(item.itemImage) : Texture2D.grayTexture,
                GUILayout.Width(50), GUILayout.Height(50));

            // »м€ и количество эффектов
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(item.itemName, EditorStyles.boldLabel);
            int effectCount = item.effectData?.effects?.Count ?? 0;
            EditorGUILayout.LabelField($"Effects: {effectCount}");
            EditorGUILayout.EndVertical();

            //  нопка выбора
            if (GUILayout.Button("Select", GUILayout.Width(60)))
                Selection.activeObject = item;

            EditorGUILayout.EndHorizontal();

            // Foldout дл€ деталей
            foldouts[item] = EditorGUILayout.Foldout(foldouts[item], "Details");
            if (foldouts[item] && item.effectData?.effects != null)
            {
                EditorGUI.indentLevel++;
                foreach (var effect in item.effectData.effects)
                {
                    EditorGUILayout.BeginVertical("box");

                    if (effect.effect != null)
                    {
                        Color prevColor = GUI.contentColor;
                        GUI.contentColor = effect.effect.effectColor;

                        EditorGUILayout.LabelField($"Stat: {effect.effect.statToAffect}");
                        EditorGUILayout.LabelField($"Operation: {effect.effect.operationType}");
                        EditorGUILayout.LabelField($"Amount: {effect.amount}");
                        EditorGUILayout.LabelField($"Duration: {effect.duration}");
                        EditorGUILayout.LabelField($"Type: {effect.effect.effectType}");

                        GUI.contentColor = prevColor;
                    }

                    if (effect.effectsToCancel != null && effect.effectsToCancel.Count > 0)
                    {
                        GUILayout.Label("Cancel Effects:");
                        foreach (var cancel in effect.effectsToCancel)
                        {
                            if (cancel != null)
                                EditorGUILayout.LabelField(cancel.effectType.ToString());
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        GUILayout.EndScrollView();
    }
}