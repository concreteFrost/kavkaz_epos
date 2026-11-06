using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Переносит экземпляры деревьев Terrain в связанные с префабами объекты сцены.
/// </summary>
public static class TerrainTreeExtraction
{
    private const string UndoName = "Извлечение деревьев Terrain";

    public static void Extract(Terrain terrain, Transform parent, IReadOnlyCollection<GameObject> treePrefabs)
    {
        if (!Validate(terrain, parent)) return;
        if (treePrefabs == null || treePrefabs.Count == 0) return;

        TerrainData data = terrain.terrainData;
        TreeInstance[] trees = data.treeInstances;
        if (trees.Length == 0) return;

        TreePrototype[] prototypes = data.treePrototypes;
        var selectedPrefabs = new HashSet<GameObject>(treePrefabs);
        var remainingTrees = new List<TreeInstance>();
        int extractedCount = 0;

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName(UndoName);

        try
        {
            foreach (TreeInstance tree in trees)
            {
                if (tree.prototypeIndex < 0 || tree.prototypeIndex >= prototypes.Length ||
                    prototypes[tree.prototypeIndex].prefab == null)
                {
                    remainingTrees.Add(tree);
                    continue;
                }

                GameObject prefab = prototypes[tree.prototypeIndex].prefab;
                if (!selectedPrefabs.Contains(prefab))
                {
                    remainingTrees.Add(tree);
                    continue;
                }

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, terrain.gameObject.scene);
                if (instance == null)
                    throw new InvalidOperationException($"Не удалось создать экземпляр дерева {prefab.name}.");

                Undo.RegisterCreatedObjectUndo(instance, UndoName);

                Transform instanceTransform = instance.transform;
                instanceTransform.SetPositionAndRotation(
                    terrain.GetPosition() + Vector3.Scale(tree.position, data.size),
                    Quaternion.Euler(0f, tree.rotation * Mathf.Rad2Deg, 0f));
                instanceTransform.localScale = Vector3.Scale(
                    prefab.transform.localScale,
                    new Vector3(tree.widthScale, tree.heightScale, tree.widthScale));
                Undo.SetTransformParent(instanceTransform, parent, UndoName);
                PrefabUtility.RecordPrefabInstancePropertyModifications(instanceTransform);
                extractedCount++;
            }

            if (extractedCount > 0)
            {
                Undo.RegisterCompleteObjectUndo(data, UndoName);
                data.SetTreeInstances(remainingTrees.ToArray(), false);
                EditorUtility.SetDirty(data);
                terrain.Flush();
                EditorSceneManager.MarkSceneDirty(terrain.gameObject.scene);
            }

            Undo.CollapseUndoOperations(undoGroup);
            Debug.Log($"Извлечено деревьев: {extractedCount}. " +
                $"Оставлено в Terrain: {remainingTrees.Count}.", terrain);
        }
        catch (Exception exception)
        {
            Undo.RevertAllDownToGroup(undoGroup);
            terrain.Flush();
            Debug.LogException(exception, terrain);
        }
    }

    private static bool Validate(Terrain terrain, Transform parent)
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("Извлекать деревья можно только вне режима Play.");
            return false;
        }

        if (terrain == null || terrain.terrainData == null)
        {
            Debug.LogError("Для извлечения необходим Terrain с назначенным TerrainData.");
            return false;
        }

        if (parent == null || EditorUtility.IsPersistent(parent) ||
            !terrain.gameObject.scene.IsValid() || !terrain.gameObject.scene.isLoaded ||
            parent.gameObject.scene != terrain.gameObject.scene ||
            PrefabStageUtility.GetPrefabStage(terrain.gameObject) != null)
        {
            Debug.LogError("Назначьте родительский Transform из той же открытой сцены. " +
                "Извлечение в режиме редактирования префаба не поддерживается.", terrain);
            return false;
        }

        Vector3 parentScale = parent.lossyScale;
        if (parentScale.x <= 0f || !Mathf.Approximately(parentScale.x, parentScale.y) ||
            !Mathf.Approximately(parentScale.x, parentScale.z))
        {
            Debug.LogError("Родитель деревьев должен иметь положительный равномерный мировой масштаб, " +
                "чтобы сохранить форму деревьев и коллайдеров.", parent);
            return false;
        }

        return true;
    }
}
