using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestNpcDialogueController))]
public class NpcQuestDialogueDrawer : Editor
{
    private SerializedProperty dialoguesSO;
    private SerializedProperty questToGiveSO;

    private void OnEnable()
    {
        dialoguesSO = serializedObject.FindProperty("dialoguesSO");
        questToGiveSO = serializedObject.FindProperty("questToGiveSO");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField(
            "Quest NPC",
            EditorStyles.boldLabel);

        EditorGUILayout.Space(5);

        // Quest
        EditorGUILayout.PropertyField(
            questToGiveSO,
            new GUIContent("Quest"));

        EditorGUILayout.Space(5);

        // Dialogues
        EditorGUILayout.PropertyField(
            dialoguesSO,
            new GUIContent("Dialogues"));

        EditorGUILayout.Space(10);

        serializedObject.ApplyModifiedProperties();
    }
}