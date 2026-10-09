using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NpcQuestDialoguesSO))]
public class NpcDialoguesSOEditor : Editor
{
    private SerializedProperty questStartedLines;
    private SerializedProperty questInProgressLines;
    private SerializedProperty questCompletedLines;
    private SerializedProperty neutralLines;

    private void OnEnable()
    {
        questStartedLines = serializedObject.FindProperty("questStartedLines");
        questInProgressLines = serializedObject.FindProperty("questInProgressLines");
        questCompletedLines = serializedObject.FindProperty("questCompletedLines");
        neutralLines = serializedObject.FindProperty("neutralLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("NPC Quest Dialogues", EditorStyles.boldLabel);

        EditorGUILayout.Space(5);
        DrawSection("Quest Started", questStartedLines);

        EditorGUILayout.Space(5);
        DrawSection("Quest In Progress", questInProgressLines);

        EditorGUILayout.Space(5);
        DrawSection("Quest Completed", questCompletedLines);

        EditorGUILayout.Space(10);
        DrawSection("Neutral", neutralLines);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSection(string title, SerializedProperty property)
    {
        if (property == null)
        {
            EditorGUILayout.HelpBox(
                $"Property \"{title}\" not found.",
                MessageType.Error);

            return;
        }

        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(property, true);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            property.arraySize++;
        }

        if (GUILayout.Button("Clear"))
        {
            property.ClearArray();
        }

        EditorGUILayout.EndHorizontal();
    }
}