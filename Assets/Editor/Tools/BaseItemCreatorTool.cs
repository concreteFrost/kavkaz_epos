using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class BaseItemCreatorTool<T> : EditorWindow where T : ConsumableItemSO
{
    protected Vector2 scroll;
    protected List<T> items = new();
    protected string search = "";
    protected Dictionary<T, bool> foldouts = new();
    protected Dictionary<T, SerializedObject> serializedCache = new();

    protected string basePath = "Assets/Resources/Items/Consumable_Items";
    protected virtual string ItemFolder { get; }

    protected virtual void OnEnable() => RefreshItems();

    protected void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
            RefreshItems();

        GUILayout.Space(10);
        search = GUILayout.TextField(search, EditorStyles.toolbarTextField, GUILayout.Width(200));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Create Item", EditorStyles.toolbarButton, GUILayout.Width(100)))
            CreateItem();

        EditorGUILayout.EndHorizontal();
    }

    protected void RefreshItems()
    {
        items.Clear();
        var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<T>(path);
            if (item != null) items.Add(item);
        }
    }

    protected void DrawScrollView()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        List<T> itemsToRemove = new List<T>();

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (!PassSearch(item)) continue;

            EditorGUILayout.BeginVertical("box");

            DrawItemHeaderCompact(item);

            if (!foldouts.ContainsKey(item)) foldouts[item] = false;
            foldouts[item] = EditorGUILayout.Foldout(foldouts[item], "Edit");

            if (foldouts[item])
            {
                DrawItem(item); // Полный контент
                if (DrawDeleteButton(item)) itemsToRemove.Add(item);
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
        }

        EditorGUILayout.EndScrollView();

        // Удаление после итерации
        foreach (var item in itemsToRemove)
        {
            string path = AssetDatabase.GetAssetPath(item);
            if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);
            items.Remove(item);
        }
        if (itemsToRemove.Count > 0) AssetDatabase.SaveAssets();
    }

    private bool PassSearch(T item) =>
        string.IsNullOrEmpty(search) || item.name.ToLower().Contains(search.ToLower());

    private void DrawItemHeaderCompact(T item)
    {
        EditorGUILayout.BeginVertical();

        // Иконка
        Texture2D img = item.itemImage != null ? AssetPreview.GetAssetPreview(item.itemImage) : Texture2D.grayTexture;
        GUILayout.Label(img, GUILayout.Width(50), GUILayout.Height(50));

        // Id и имя
        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
            serializedCache[item] = so = new SerializedObject(item);

        so.Update();
        SerializedProperty id = so.FindProperty("id");
        SerializedProperty itemName = so.FindProperty("itemName");
        SerializedProperty description = so.FindProperty("itemDescription");

        EditorGUILayout.BeginVertical();
        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(id, GUILayout.Width(500));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(itemName, GUILayout.Width(500));
        description.stringValue = EditorGUILayout.TextArea(
    description.stringValue,
    GUILayout.Width(500),
    GUILayout.Height(60)
);
        EditorGUILayout.EndVertical();



        GUILayout.FlexibleSpace();

        // Кнопка Select справа
        if (GUILayout.Button("Select", GUILayout.Width(60)))
            Selection.activeObject = item;

        EditorGUILayout.EndVertical();
        so.ApplyModifiedProperties();
    }

    private bool DrawDeleteButton(T item)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool pressed = GUILayout.Button("DELETE", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return pressed;
    }

    protected abstract void DrawItem(T item); // полный контент при раскрытии

    protected T CreateItem()
    {
        T item = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GenerateUniqueAssetPath($"{ItemFolder}/{typeof(T).Name}.asset");
        AssetDatabase.CreateAsset(item, path);
        AssetDatabase.SaveAssets();
        RefreshItems();
        Selection.activeObject = item;
        return item;
    }
}