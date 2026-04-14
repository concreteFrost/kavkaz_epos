using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NpcDialoguesSO))]
public class NpcDialoguesSOEditor : Editor
{
    SerializedProperty questDialogueLines;
    SerializedProperty neutralDialogueLines;

    private void OnEnable()
    {
        questDialogueLines = serializedObject.FindProperty("questDialogueLines");
        neutralDialogueLines = serializedObject.FindProperty("neutralDialogueLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("NPC Dialogues", EditorStyles.boldLabel);

        EditorGUILayout.Space(5);
        DrawSection("Quest Dialogues", questDialogueLines);

        EditorGUILayout.Space(10);
        DrawSection("Neutral Dialogues", neutralDialogueLines);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawSection(string title, SerializedProperty property)
    {
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