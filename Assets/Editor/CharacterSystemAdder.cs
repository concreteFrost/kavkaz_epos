#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class CharacterSystemAdder : EditorWindow
{
    private MonoScript scriptAsset;
    private string childObjectName = "Core";
    private string locatorFieldName = "newSystem";
    private string systemsRootName = "Systems";

    [MenuItem("Tools/Character System Tool")]
    public static void Open()
    {
        GetWindow<CharacterSystemAdder>("Enemy System Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add System To Enemies", EditorStyles.boldLabel);

        scriptAsset = (MonoScript)EditorGUILayout.ObjectField(
            "System Script",
            scriptAsset,
            typeof(MonoScript),
            false);

        childObjectName = EditorGUILayout.TextField("Child Object Name", childObjectName);
        locatorFieldName = EditorGUILayout.TextField("ServiceLocator Field", locatorFieldName);
        systemsRootName = EditorGUILayout.TextField("Systems Root", systemsRootName);

        if (GUILayout.Button("Add To All Enemy Prefabs"))
        {
            AddSystem();
        }
    }

    private void AddSystem()
    {
        if (scriptAsset == null)
        {
            Debug.LogError("No script selected.");
            return;
        }

        Type type = scriptAsset.GetClass();

        if (type == null || !type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            Debug.LogError("Selected script is not a MonoBehaviour.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab.GetComponent<BaseHumanoidAiServiceLocator>() == null)
                continue;

            ModifyPrefab(path, type);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Done.");
    }

    private void ModifyPrefab(string prefabPath, Type systemType)
    {
        GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);

        var locator = root.GetComponent<BaseHumanoidAiServiceLocator>();
        if (locator == null)
        {
            PrefabUtility.UnloadPrefabContents(root);
            return;
        }

        Transform systemsRoot = root.transform.Find("AIScripts/" + systemsRootName);
        if (systemsRoot == null)
        {
            Debug.Log("no core found");
            return;
        }

        Transform child = systemsRoot.Find(childObjectName);
        if (child == null)
        {
            GameObject go = new GameObject(childObjectName);
            go.transform.SetParent(systemsRoot);
            go.AddComponent(systemType);
            child = go.transform;
        }

        Component newComponent = child.GetComponent(systemType);

        SerializedObject so = new SerializedObject(locator);
        SerializedProperty prop = so.FindProperty(locatorFieldName);
        if (prop != null)
        {
            prop.objectReferenceValue = newComponent;
            so.ApplyModifiedProperties();
        }

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        PrefabUtility.UnloadPrefabContents(root);
    }
}
#endif