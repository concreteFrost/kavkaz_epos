using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal static class SceneObjectContextMenu
{
    private const int LeftMouseButton = 0;
    private const int DoubleClickCount = 2;
    private const BindingFlags InstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private const string ShowInHierarchyLabel = "Показать в иерархии";

    private static readonly Type HierarchyWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
    private static readonly Type HierarchyType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchy");
    private static readonly MethodInfo GetHierarchyWindows = HierarchyWindowType?.GetMethod(
        "GetAllSceneHierarchyWindows", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    private static readonly PropertyInfo SceneHierarchy = HierarchyWindowType?.GetProperty("sceneHierarchy", InstanceMembers);
    private static readonly PropertyInfo IsLocked = HierarchyType?.GetProperty("isLocked", InstanceMembers);
    private static readonly MethodInfo FrameObject = HierarchyWindowType?.GetMethod(
        "FrameObject", InstanceMembers, null, new[] { typeof(int), typeof(bool) }, null);

    static SceneObjectContextMenu()
    {
        // Unity не предоставляет публичного API для блокировки и раскрытия окна Hierarchy.
        if (GetHierarchyWindows == null || SceneHierarchy == null || IsLocked == null || FrameObject == null)
        {
            Debug.LogWarning("Меню объектов сцены отключено: API окна Hierarchy несовместим с этой версией Unity.");
            return;
        }

        SceneView.beforeSceneGui += OnBeforeSceneGui;
        EditorApplication.delayCall += LockHierarchyWindows;
        AssemblyReloadEvents.beforeAssemblyReload += Unsubscribe;
    }

    private static void Unsubscribe()
    {
        SceneView.beforeSceneGui -= OnBeforeSceneGui;
        EditorApplication.delayCall -= LockHierarchyWindows;
        AssemblyReloadEvents.beforeAssemblyReload -= Unsubscribe;
    }

    private static void OnBeforeSceneGui(SceneView sceneView)
    {
        Event currentEvent = Event.current;
        if (currentEvent.type != EventType.MouseDown || currentEvent.button != LeftMouseButton)
            return;

        // Проверяем только открытые окна при клике, без поиска объектов сцены и опроса в Update.
        LockHierarchyWindows();

        if (currentEvent.clickCount != DoubleClickCount || currentEvent.modifiers != EventModifiers.None || Tools.viewToolActive)
            return;

        GameObject target = HandleUtility.PickGameObject(currentEvent.mousePosition, true);
        if (target == null)
            return;

        Selection.activeGameObject = target;
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent(ShowInHierarchyLabel), false, () => ShowInHierarchy(target));
        menu.DropDown(new Rect(currentEvent.mousePosition, Vector2.zero));
        currentEvent.Use();
    }

    private static void LockHierarchyWindows()
    {
        IList windows = (IList)GetHierarchyWindows.Invoke(null, null);
        for (int i = 0; i < windows.Count; i++)
        {
            EditorWindow window = windows[i] as EditorWindow;
            if (window != null)
                LockHierarchy(window);
        }
    }

    private static void LockHierarchy(EditorWindow window)
    {
        object hierarchy = SceneHierarchy.GetValue(window);
        if (hierarchy == null || (bool)IsLocked.GetValue(hierarchy))
            return;

        IsLocked.SetValue(hierarchy, true);
        window.Repaint();
    }

    private static void ShowInHierarchy(GameObject target)
    {
        if (target == null)
            return;

        EditorWindow window = EditorWindow.GetWindow(HierarchyWindowType);
        LockHierarchy(window);
        Selection.activeGameObject = target;
        FrameObject.Invoke(window, new object[] { target.GetInstanceID(), true });
        window.Focus();
        window.Repaint();
    }
}
