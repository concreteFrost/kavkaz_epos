#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LootManager))]
public class LootManagerEditor : Editor
{
    private bool showLootSummary = true;
    private readonly Dictionary<Type, bool> groupFoldouts = new();

    // Порядок здесь = порядок групп в Inspector
    private static readonly (Type type, string name)[] Groups =
    {
        (typeof(WeaponSO),              "WEAPONS"),
        (typeof(ShieldSO),              "SHIELDS"),
        (typeof(ConsumableItemSO),      "CONSUMABLES"),
        (typeof(CombatItemSO),          "COMBAT ITEMS"),
        (typeof(KeyItemSO),             "KEY ITEMS"),
        (typeof(QuestItemSO),           "QUEST ITEMS"),
        (typeof(StatModifierItemSO),    "STAT MODIFIERS"),
        (typeof(WeaponRepairItemSO),  "WEAPON REPAIRS"),
        (typeof(PointsEmitterItemSO),   "POINTS"),
    };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(15);

        showLootSummary = EditorGUILayout.BeginFoldoutHeaderGroup(
            showLootSummary,
            "SCENE LOOT SUMMARY"
        );

        if (showLootSummary)
            DrawLootSummary();

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawLootSummary()
    {
        Dictionary<Type, Dictionary<ItemSO, int>> loot = CollectLoot();

        if (loot.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "No loot found in the scene.",
                MessageType.Info
            );

            return;
        }

        foreach (var group in Groups)
        {
            if (!loot.TryGetValue(group.type, out var items))
                continue;

            DrawGroup(group.type, group.name, items);
        }
    }

    private Dictionary<Type, Dictionary<ItemSO, int>> CollectLoot()
    {
        var result = new Dictionary<Type, Dictionary<ItemSO, int>>();

        StaticLootHolder[] holders = FindObjectsByType<StaticLootHolder>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (StaticLootHolder holder in holders)
        {
            if (holder.lootEntries == null)
                continue;

            foreach (LootItemEntry data in holder.lootEntries)
            {
                if (data?.itemSO == null)
                    continue;

                Type groupType = GetGroupType(data.itemSO);

                if (groupType == null)
                    continue;

                if (!result.TryGetValue(groupType, out var items))
                {
                    items = new Dictionary<ItemSO, int>();
                    result.Add(groupType, items);
                }

                items[data.itemSO] = items.GetValueOrDefault(data.itemSO) + data.quantity;
            }
        }

        return result;
    }

    private Type GetGroupType(ItemSO item)
    {
        Type itemType = item.GetType();

        foreach (var group in Groups)
        {
            // WeaponSO + все его наследники попадут в WEAPONS
            if (group.type.IsAssignableFrom(itemType))
                return group.type;
        }

        return null;
    }

    private void DrawGroup(
        Type type,
        string groupName,
        Dictionary<ItemSO, int> items)
    {
        if (!groupFoldouts.ContainsKey(type))
            groupFoldouts[type] = true;

        groupFoldouts[type] = EditorGUILayout.Foldout(
            groupFoldouts[type],
            groupName,
            true
        );

        if (!groupFoldouts[type])
            return;

        EditorGUI.indentLevel++;

        foreach (var item in items)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(item.Key.itemName);

            EditorGUILayout.LabelField(
                item.Value.ToString(),
                GUILayout.Width(60)
            );

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel--;

        EditorGUILayout.Space(5);
    }
}

#endif