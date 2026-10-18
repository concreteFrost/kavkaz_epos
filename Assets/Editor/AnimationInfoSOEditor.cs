using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationInfoSO))]
public class AnimationInfoSOEditor : Editor
{
    private const float PreviewHeight = 360f;
    private Editor clipEditor;

    private void OnDisable()
    {
        DestroyImmediate(clipEditor);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UpdateClipEditor();

        EditorGUILayout.HelpBox(
            "Тайминги попадания и звука заданы в долях анимации: 0 — начало, 0.5 — середина, 1 — конец. Превью показывает исходный клип; звук оружия в нём не воспроизводится.",
            MessageType.Info);

        if (clipEditor == null || !clipEditor.HasPreviewGUI())
            return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Предпросмотр анимации", EditorStyles.boldLabel);
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            GUILayout.FlexibleSpace();
            clipEditor.OnPreviewSettings();
        }

        var rect = GUILayoutUtility.GetRect(0f, PreviewHeight, GUILayout.ExpandWidth(true));
        clipEditor.OnInteractivePreviewGUI(rect, EditorStyles.helpBox);
    }

    public override bool RequiresConstantRepaint()
    {
        return clipEditor != null;
    }

    private void UpdateClipEditor()
    {
        var clip = ((AnimationInfoSO)target).clip;
        if (clip == null)
        {
            DestroyImmediate(clipEditor);
            clipEditor = null;
            return;
        }

        CreateCachedEditor(clip, null, ref clipEditor);
    }
}
