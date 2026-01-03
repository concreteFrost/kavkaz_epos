using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(HumanoidAITester))]
public class HumanoidAITesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); 

        HumanoidAITester tester = (HumanoidAITester)target;

        GUILayout.Space(10);
        GUILayout.Label("AI Test Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Move To Target"))
        {
            tester.MoveToTarget();
        }

        if(GUILayout.Button("Move To Default"))
        {
            tester.MoveToDefaultPosition();
        }

        tester.aiMotor.isSprinting = (EditorGUILayout.Toggle("isRunning", tester.aiMotor.isSprinting));
    }
}
