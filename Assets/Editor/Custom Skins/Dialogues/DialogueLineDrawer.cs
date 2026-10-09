using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueLine))]
public class DialogueLineDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty textProp = property.FindPropertyRelative("dialogueLine");
        return EditorGUI.GetPropertyHeight(textProp, true) + 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty textProp = property.FindPropertyRelative("dialogueLine");

        position.height = EditorGUI.GetPropertyHeight(textProp, true);
        EditorGUI.PropertyField(position, textProp, GUIContent.none, true);
    }
}