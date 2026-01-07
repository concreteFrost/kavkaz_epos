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

        tester.aiMotor.isSprinting = (EditorGUILayout.Toggle("isRunning", tester.aiMotor.isSprinting));

        GUILayout.Label("Movement", EditorStyles.boldLabel);

        if (GUILayout.Button("Move To Target"))
        {
            tester.MoveToTarget();
        }

        if(GUILayout.Button("Move To Default"))
        {
            tester.MoveToDefaultPosition();
        }

        if (GUILayout.Button("Dodge"))
        {
            tester.Dodge();
        }

        GUILayout.Space(10);
        GUILayout.Label("Target Lock", EditorStyles.boldLabel);

        if (GUILayout.Button("Lock Target"))
        {
            tester.SetLockOnTarget();
        }

        if(GUILayout.Button("Reset Target"))
        {
            tester.ResetLockTarget();   
        }

        GUILayout.Space(10);
        GUILayout.Label("Combat", EditorStyles.boldLabel);


        if (GUILayout.Button("Single Attack"))
        {
            tester.SingleAttack();  
        }

        if (GUILayout.Button("Combo"))
        {
            tester.Combo();
        }

        GUILayout.Space(10);
        GUILayout.Label("Interaction", EditorStyles.boldLabel);

        if (GUILayout.Button("Find Weapon"))
        {
            tester.FindWeapon();    
        }

        if (GUILayout.Button("Find Shield"))
        {
            tester.FindWShield();
        }




    }
}
