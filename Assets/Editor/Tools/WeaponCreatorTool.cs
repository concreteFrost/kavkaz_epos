using UnityEditor;
using UnityEngine;

public class WeaponCreatorTool : BaseItemCreatorTool<WeaponSO>
{
    protected override string ItemFolder => $"{basePath}/Weapons/";
    protected override void DrawItem(WeaponSO item)
    {
        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();

        EditorGUILayout.BeginVertical("box");

        DrawOverride(so);
        EditorGUILayout.Space(10);

        DrawWeaponType(so);
        EditorGUILayout.Space(10);

        DrawDamage(so);
        EditorGUILayout.Space(10);

        DrawAttackSet(so);
        EditorGUILayout.Space(10);

        DrawAnimation(so);

        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();
    }

    private void DrawOverride(SerializedObject so)
    {
        SerializedProperty canOverride = so.FindProperty("canOverride");
        EditorGUILayout.PropertyField(canOverride);
    }

    private void DrawWeaponType(SerializedObject so)
    {
        SerializedProperty weaponType = so.FindProperty("weaponType");
        EditorGUILayout.PropertyField(weaponType);
    }

    private void DrawDamage(SerializedObject so)
    {
        SerializedProperty baseDamage = so.FindProperty("baseDamage");
        EditorGUILayout.PropertyField(baseDamage);
    }

    private void DrawAttackSet(SerializedObject so)
    {
        SerializedProperty attackSet = so.FindProperty("attackSet");
        EditorGUILayout.PropertyField(attackSet);
    }

    private void DrawAnimation(SerializedObject so)
    {
        SerializedProperty idleAnimation = so.FindProperty("idleAnimation");
        EditorGUILayout.PropertyField(idleAnimation);
    }
}