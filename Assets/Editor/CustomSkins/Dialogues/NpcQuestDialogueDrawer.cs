using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NpcQuestDialogue))]
public class NpcQuestDialogueDrawer : PropertyDrawer
{
    private bool foldout = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!foldout) return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight;

        height += GetHeight(property, "questToGiveSO");
        height += GetHeight(property, "questStartedLines");
        height += GetHeight(property, "questInProgressLines");
        height += GetHeight(property, "questCompletedLines");
        height += GetHeight(property, "rewards");

        return height + 10;
    }

    float GetHeight(SerializedProperty property, string name)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(name), true) + 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect rect = position;
        rect.height = EditorGUIUtility.singleLineHeight;

        foldout = EditorGUI.Foldout(rect, foldout, "Quest Dialogue", true);

        if (!foldout) return;

        EditorGUI.indentLevel++;

        Draw(ref rect, property, "questToGiveSO", "Quest");
        Draw(ref rect, property, "questStartedLines", "Start Lines");
        Draw(ref rect, property, "questInProgressLines", "In Progress");
        Draw(ref rect, property, "questCompletedLines", "Completed");
        Draw(ref rect, property, "rewards", "Rewards");

        EditorGUI.indentLevel--;
    }

    void Draw(ref Rect rect, SerializedProperty property, string name, string label)
    {
        var prop = property.FindPropertyRelative(name);

        rect.y += rect.height + 4;
        rect.height = EditorGUI.GetPropertyHeight(prop, true);

        EditorGUI.PropertyField(rect, prop, new GUIContent(label), true);
    }
}