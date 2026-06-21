using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterCustomInspector))]
public class CharacterCustomInspectorEditor : Editor
{
    private CharacterCustomInspector proxy;

    private void OnEnable()
    {
        proxy = (CharacterCustomInspector)target;
    }

    public override void OnInspectorGUI()
    {
        GameObject go = proxy.gameObject;

        DrawStatsInfo(go);
        DrawCombatInventory(go);
        DrawSpellInventory(go);
        DrawInitialInventorySetup(go);  
        //DrawCombatBehaviour(go);
        DrawBehaviourStats(go);
        DrawLoot(go);
        DrawPointsData(go);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(go);
        }
    }

    private void DrawStatsInfo(GameObject go)
    {
        var statsController = go.GetComponentInChildren<CharacterStatsController>();

        if (statsController == null)
        {
            EditorGUILayout.HelpBox("No CharacterStatsController found", MessageType.Warning);
            return;
        }

        SerializedObject so = new SerializedObject(statsController);

        so.Update();

        EditorGUILayout.PropertyField(
            so.FindProperty("initialHealthLevel"));

        EditorGUILayout.PropertyField(
            so.FindProperty("initialStaminaLevel"));

        EditorGUILayout.PropertyField(
            so.FindProperty("initialStrengthLevel"));

        EditorGUILayout.PropertyField(
            so.FindProperty("initialKnowledgeLevel"));

        so.ApplyModifiedProperties();
    }

    private void DrawCombatInventory(GameObject go)
    {
        var inventory = go.GetComponentInChildren<CharacterWeaponInventory>();

        if (inventory == null)
            return;

        SerializedObject so = new SerializedObject(inventory);

        so.Update();

        EditorGUILayout.PropertyField(
            so.FindProperty("starterSet"));

        so.ApplyModifiedProperties();
    }

    private void DrawSpellInventory(GameObject go)
    {
        var inventory = go.GetComponentInChildren<CharacterSpellInventory>();

        if (inventory == null)
            return;

        SerializedObject so = new SerializedObject(inventory);

        so.Update();

        EditorGUILayout.PropertyField(
            so.FindProperty("items"),
            true);

        so.ApplyModifiedProperties();
    }

    private void DrawBehaviourStats(GameObject go)
    {
        var tracker = go.GetComponentInChildren<EnemyStateTracker>();

        if (tracker == null)
            return;

        SerializedObject so = new SerializedObject(tracker);

        so.Update();

        EditorGUILayout.PropertyField(
            so.FindProperty("stats"));

        so.ApplyModifiedProperties();
    }

    private void DrawLoot(GameObject go)
    {
        var loot = go.GetComponentInChildren<CharacterLootDistributer>();

        if (loot == null)
            return;

        SerializedObject so = new SerializedObject(loot);

        so.Update();

        EditorGUILayout.PropertyField(
            so.FindProperty("listSO"));

        so.ApplyModifiedProperties();
    }

    private void DrawPointsData(GameObject obj)
    {
        var pointsEmitter = obj.GetComponentInChildren<PointsEmitter>();

        if (pointsEmitter == null)
        {
           
            return;
        }

        EditorGUILayout.LabelField("Points to collect", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        SerializedObject so = new SerializedObject(pointsEmitter);
        SerializedProperty points = so.FindProperty("points");

        so.Update();
        EditorGUILayout.PropertyField(points, true);

        if (so.ApplyModifiedProperties())
        {
            Undo.RecordObject(pointsEmitter, "Modify points");
            EditorUtility.SetDirty(pointsEmitter);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawInitialInventorySetup(GameObject obj)
    {
        var constructor = obj.GetComponentInChildren<CharacterConstructor>();

        if (constructor == null) return;

        EditorGUILayout.LabelField("Inventory Setup");

        EditorGUI.indentLevel++;
        SerializedObject so = new SerializedObject(constructor);
        
        SerializedProperty hasConumables = so.FindProperty("hasAllConsumables");
        SerializedProperty hasSpells= so.FindProperty("hasAllSpells");
        SerializedProperty hasWeapons = so.FindProperty("hasAllWeapons");

        so.Update();

        EditorGUILayout.PropertyField (hasConumables, true);
        EditorGUILayout.PropertyField(hasSpells, true);
        EditorGUILayout.PropertyField(hasWeapons, true);

        if (so.ApplyModifiedProperties())
        {
            Undo.RecordObject(constructor, "Modify Inventory Set");
            EditorUtility.SetDirty(constructor);    
        }

        EditorGUI.indentLevel--;    

    }
}