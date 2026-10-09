using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConsumableItemsCreatorTool : BaseItemCreatorTool<ConsumableItemSO>
{
    private List<Type> consumableTypes = new List<Type>();
    private string[] consumableTypeNames = Array.Empty<string>();
    private int selectedTypeIndex;

    protected override string ItemFolder => GetFolderForType(GetSelectedType());

    protected override void OnEnable()
    {
        RefreshConsumableTypes();
        base.OnEnable();
    }

    protected override void DrawCreateOptions()
    {
        if (consumableTypes.Count == 0)
        {
            GUILayout.Label("No concrete consumable types found.");
            return;
        }

        GUILayout.Label("Type:", GUILayout.Width(35));
        selectedTypeIndex = EditorGUILayout.Popup(
            selectedTypeIndex,
            consumableTypeNames,
            EditorStyles.toolbarPopup,
            GUILayout.Width(190));
    }

    protected override Type GetCreationType() => GetSelectedType();

    protected override void RefreshItems()
    {
        if (consumableTypes.Count == 0)
            RefreshConsumableTypes();

        items.Clear();
        var loadedPaths = new HashSet<string>();

        foreach (Type type in consumableTypes)
        {
            foreach (string guid in AssetDatabase.FindAssets($"t:{type.Name}"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!loadedPaths.Add(path))
                    continue;

                var item = AssetDatabase.LoadAssetAtPath<ConsumableItemSO>(path);
                if (item != null && item.GetType() == type)
                    items.Add(item);
            }
        }

        items.Sort((left, right) => string.Compare(left.itemName, right.itemName, StringComparison.OrdinalIgnoreCase));
    }

    protected override void DrawItem(ConsumableItemSO item)
    {
        if (item == null)
            return;

        if (!serializedCache.TryGetValue(item, out var serializedObject) || serializedObject.targetObject == null)
        {
            serializedObject = new SerializedObject(item);
            serializedCache[item] = serializedObject;
        }

        serializedObject.Update();
        EditorGUILayout.BeginVertical("box");

        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (property.propertyPath == "m_Script" ||
                property.propertyPath == "id" ||
                property.propertyPath == "itemName" ||
                property.propertyPath == "itemImage" ||
                property.propertyPath == "itemDescription")
                continue;

            EditorGUILayout.PropertyField(property, true);
        }

        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }

    private void RefreshConsumableTypes()
    {
        consumableTypes = TypeCache.GetTypesDerivedFrom<ConsumableItemSO>()
            .Where(type => type.IsClass && !type.IsAbstract && !type.ContainsGenericParameters)
            .OrderBy(GetTypeDisplayName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        consumableTypeNames = consumableTypes.Select(GetTypeDisplayName).ToArray();
        selectedTypeIndex = Mathf.Clamp(selectedTypeIndex, 0, Mathf.Max(0, consumableTypes.Count - 1));
    }

    private Type GetSelectedType()
    {
        if (consumableTypes == null || consumableTypes.Count == 0)
            return typeof(ConsumableItemSO);

        selectedTypeIndex = Mathf.Clamp(selectedTypeIndex, 0, consumableTypes.Count - 1);
        return consumableTypes[selectedTypeIndex];
    }

    private string GetFolderForType(Type type)
    {
        if (type == typeof(StatModifierItemSO))
            return $"{basePath}/Consumable/StatusEffect_Items/";
        if (type == typeof(PointsEmitterItemSO))
            return $"{basePath}/Consumable/PointsEmitter_Items/";
        if (type == typeof(WeaponRepairItemSO))
            return $"{basePath}/Consumable/WeaponRepair_Items/";
        if (type == typeof(KeyItemSO))
            return $"{basePath}/Consumable/KeyItems/";
        if (type == typeof(QuestItemSO))
            return $"{basePath}/QuestItems/";

        return $"{basePath}/Consumable/";
    }

    private static string GetTypeDisplayName(Type type)
    {
        string name = type.Name;
        if (name.EndsWith("SO", StringComparison.Ordinal))
            name = name.Substring(0, name.Length - 2);

        return ObjectNames.NicifyVariableName(name);
    }
}