using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Предоставляет запуск извлечения деревьев из Inspector.
/// </summary>
[CustomEditor(typeof(TreeExtractor))]
public class TreeExtractorEditor : Editor
{
    private readonly Dictionary<GameObject, int> instanceCounts = new Dictionary<GameObject, int>();
    private readonly List<GameObject> availablePrefabs = new List<GameObject>();
    private TerrainData cachedData;
    private int cachedTreeCount = -1;
    private int cachedDirtyCount = -1;
    private int missingPrefabCount;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("parentTransform"));

        var extractor = (TreeExtractor)target;
        var terrain = extractor.GetComponent<Terrain>();
        int treeCount = terrain.terrainData != null ? terrain.terrainData.treeInstanceCount : 0;
        RefreshList(terrain.terrainData);
        int selectedCount = DrawPrefabSelection();
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("Экземпляров в разделе Trees", treeCount.ToString());
        EditorGUILayout.LabelField("Будет извлечено", selectedCount.ToString());
        EditorGUILayout.HelpBox(
            "Отметьте галочками нужные префабы из Terrain. Будут перенесены только отмеченные. Остальные объекты " +
            "раздела Trees и трава из Details останутся в Terrain. " +
            "Изменение TerrainData затронет все Terrain, использующие эти данные. " +
            "Операция поддерживает Undo. Индивидуальные оттенки деревьев Terrain не переносятся.",
            MessageType.Info);
        EditorGUILayout.HelpBox(
            "Для NavMesh по коллайдерам выберите Physics Colliders в NavMeshSurface. " +
            "У префабов должны быть включённые коллайдеры, а родитель и слои деревьев " +
            "должны входить в область сбора NavMeshSurface. После извлечения повторите Bake.",
            MessageType.Info);

        using (new EditorGUI.DisabledScope(
            EditorApplication.isPlayingOrWillChangePlaymode || selectedCount == 0 || extractor.ParentTransform == null))
        {
            if (GUILayout.Button("Извлечь деревья"))
            {
                TerrainTreeExtraction.Extract(terrain, extractor.ParentTransform, extractor.TreePrefabs);
                cachedTreeCount = -1;
            }
        }
    }

    private void RefreshList(TerrainData data)
    {
        int treeCount = data != null ? data.treeInstanceCount : 0;
        int dirtyCount = data != null ? EditorUtility.GetDirtyCount(data) : 0;
        if (data == cachedData && treeCount == cachedTreeCount && dirtyCount == cachedDirtyCount) return;

        cachedData = data;
        cachedTreeCount = treeCount;
        cachedDirtyCount = dirtyCount;
        instanceCounts.Clear();
        availablePrefabs.Clear();
        missingPrefabCount = 0;
        if (data == null) return;

        TreePrototype[] prototypes = data.treePrototypes;
        var countsByPrototype = new int[prototypes.Length];
        foreach (TreeInstance tree in data.treeInstances)
        {
            if (tree.prototypeIndex >= 0 && tree.prototypeIndex < prototypes.Length)
                countsByPrototype[tree.prototypeIndex]++;
            else
                missingPrefabCount++;
        }

        for (int i = 0; i < prototypes.Length; i++)
        {
            int count = countsByPrototype[i];
            if (count == 0) continue;
            GameObject prefab = prototypes[i].prefab;
            if (prefab == null)
            {
                missingPrefabCount += count;
                continue;
            }

            if (instanceCounts.TryGetValue(prefab, out int existingCount))
                instanceCounts[prefab] = existingCount + count;
            else
            {
                availablePrefabs.Add(prefab);
                instanceCounts.Add(prefab, count);
            }
        }
    }

    private int DrawPrefabSelection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Объекты, размещённые в Terrain Trees", EditorStyles.boldLabel);
        if (availablePrefabs.Count == 0)
            EditorGUILayout.HelpBox("Доступных для извлечения экземпляров нет.", MessageType.Info);
        if (missingPrefabCount > 0)
            EditorGUILayout.HelpBox($"Экземпляров без префаба: {missingPrefabCount}. Они останутся в Terrain.",
                MessageType.Warning);

        SerializedProperty selection = serializedObject.FindProperty("treePrefabs");
        int selectedCount = 0;
        foreach (GameObject prefab in availablePrefabs)
        {
            int selectionIndex = FindPrefab(selection, prefab);
            bool selected = selectionIndex >= 0;
            using (new EditorGUILayout.HorizontalScope())
            {
                bool newSelected = EditorGUILayout.Toggle(selected, GUILayout.Width(EditorGUIUtility.singleLineHeight));
                using (new EditorGUI.DisabledScope(true))
                    EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
                var countLabel = new GUIContent($"{instanceCounts[prefab]} шт.");
                EditorGUILayout.LabelField(countLabel, GUILayout.Width(EditorStyles.label.CalcSize(countLabel).x));

                if (newSelected != selected)
                {
                    SetSelected(selection, prefab, newSelected);
                    selected = newSelected;
                }
            }

            if (selected) selectedCount += instanceCounts[prefab];
        }

        return selectedCount;
    }

    private static int FindPrefab(SerializedProperty selection, GameObject prefab)
    {
        for (int i = 0; i < selection.arraySize; i++)
        {
            if (selection.GetArrayElementAtIndex(i).objectReferenceValue == prefab) return i;
        }

        return -1;
    }

    private static void SetSelected(SerializedProperty selection, GameObject prefab, bool selected)
    {
        if (selected)
        {
            int index = selection.arraySize++;
            selection.GetArrayElementAtIndex(index).objectReferenceValue = prefab;
            return;
        }

        int selectionIndex;
        while ((selectionIndex = FindPrefab(selection, prefab)) >= 0)
        {
            selection.GetArrayElementAtIndex(selectionIndex).objectReferenceValue = null;
            selection.DeleteArrayElementAtIndex(selectionIndex);
        }
    }
}
