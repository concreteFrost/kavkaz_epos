using UnityEditor;
using UnityEngine;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropertyPath = string.Empty; 

        foreach(SerializedProperty prop in property)
        {
            if(prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                EditorGUILayout.EndHorizontal();

                if (prop.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(prop, drawChildren);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropertyPath) && prop.propertyPath.Contains(lastPropertyPath)) continue;
                    lastPropertyPath = prop.propertyPath;
                    EditorGUILayout.PropertyField(prop,drawChildren);   
                }
            }
        }
    }
}
