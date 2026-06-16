using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;

public class CharacterInspectorTool : EditorWindow
{
    private Vector2 scroll;
    private Dictionary<int, bool> foldoutStates = new();

    [MenuItem("Tools/Character Tools/Character Inspector")]
    public static void ShowWindow()
    {
        GetWindow<CharacterInspectorTool>("Character Inspector");
    }

    private void OnGUI()
    {
        DrawHeader("Player");

        scroll = EditorGUILayout.BeginScrollView(scroll);

        var pl = GameObject.FindGameObjectWithTag("Player");

        if(pl != null)
        {
            DrawBaseCharacterCard(pl);
        }
       
        DrawHeader("Enemies");


        var allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in allEnemies)
        {
            DrawBaseCharacterCard(enemy);
            GUILayout.Space(8);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawHeader(string header)
    {
        GUILayout.Space(5);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
    }

    private void DrawBaseCharacterCard(GameObject obj)
    {
        int id = obj.GetInstanceID();

        if (!foldoutStates.ContainsKey(id))
            foldoutStates[id] = false;

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        foldoutStates[id] = EditorGUILayout.Foldout(
            foldoutStates[id],
            obj.name,
            true,
            EditorStyles.foldoutHeader
        );

        if (GUILayout.Button("Ping", GUILayout.Width(45)))
        {
            if (SceneView.lastActiveSceneView != null)
            {
                Vector3 focusPosition = Vector3.one;
                focusPosition.y += 2;
                SceneView.lastActiveSceneView.Frame(
                    new Bounds(obj.transform.position, focusPosition),
                    false
                );
            }
        }

        EditorGUILayout.EndHorizontal();

        if (foldoutStates[id])
        {
            EditorGUILayout.Space(5);
            DrawBaseComponents(obj);    
    
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawBaseComponents(GameObject obj)
    {
        DrawStatsInfo(obj);
        DrawCombatInventory(obj);
        DrawSpellInventory(obj);
        DrawConsumableInventory(obj);
        DrawPointsData(obj);
        DrawBehaviourStats(obj);
        DrawLoot(obj);
    }

    private void DrawBehaviourStats(GameObject go)
    {
        var stateTracker = go.GetComponentInChildren<EnemyStateTracker>();

        if (stateTracker == null)
        {
            EditorGUILayout.HelpBox("No EnemyStateTracker found", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Behaviour stats", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        EditorGUI.BeginChangeCheck();

        var newStats = (CharacterBehaviourStatsSO)EditorGUILayout.ObjectField("Behaviour stats", stateTracker.stats, typeof(CharacterBehaviourStatsSO), false);

        if (newStats == null)
        {
            EditorGUILayout.HelpBox("No behaviour stats assigned", MessageType.Warning);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(stateTracker, "Change behaviour stats");
            stateTracker.stats = newStats;
            EditorUtility.SetDirty(stateTracker);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawStatsInfo(GameObject go)
    {
        var statsController = go.GetComponentInChildren<CharacterStatsController>();

        if(statsController == null)
        {
            EditorGUILayout.HelpBox("No CharacterStatsController found", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Stats controller", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        EditorGUI.BeginChangeCheck();

        SerializedObject so = new SerializedObject(statsController);

        so.Update();

        var hp = so.FindProperty("initialHealthLevel");
        var stamina = so.FindProperty("initialStaminaLevel");
        var strength = so.FindProperty("initialStrengthLevel");
        var knowledge = so.FindProperty("initialKnowledgeLevel");

        EditorGUILayout.PropertyField(hp);
        EditorGUILayout.PropertyField(stamina);
        EditorGUILayout.PropertyField(strength);
        EditorGUILayout.PropertyField(knowledge);

        if (EditorGUI.EndChangeCheck())
        {
            so.ApplyModifiedProperties();
        }



        EditorGUI.indentLevel--;    
    }

    private void DrawLoot(GameObject go)
    {
        var characterLoot = go.GetComponentInChildren<CharacterLootDistributer>();

        if(characterLoot == null)
        {
            EditorGUILayout.HelpBox("No loot distributer found", MessageType.Warning);
            return;
        }


        EditorGUILayout.LabelField("Loot Items", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;        

        EditorGUI.BeginChangeCheck();

        SerializedObject so = new SerializedObject(characterLoot);

        so.Update();

        var list = so.FindProperty("listSO");

        EditorGUILayout.PropertyField(list);

        if (EditorGUI.EndChangeCheck())
        {
            so.ApplyModifiedProperties();   
        }

        EditorGUI.indentLevel--;    
    }



    private void DrawCombatInventory(GameObject go)
    {
        var combatInventory = go.GetComponentInChildren<CharacterWeaponInventory>();

        if (combatInventory == null)
        {
            EditorGUILayout.HelpBox("No HumanoidCombatInventory found", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Melee Inventory", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        EditorGUI.BeginChangeCheck();

        var newStarterSet = (CombatInventorySO)EditorGUILayout.ObjectField(
            "Starter Set",
            combatInventory.starterSet,
            typeof(CombatInventorySO),
            false
        );

        if (newStarterSet == null)
        {
            EditorGUILayout.HelpBox("Starter Set is not assigned", MessageType.Info);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(combatInventory, "Change Starter Set");
            combatInventory.starterSet = newStarterSet;
            EditorUtility.SetDirty(combatInventory);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawSpellInventory(GameObject obj)
    {
        var spellInventory = obj.GetComponentInChildren<CharacterSpellInventory>();

        if (spellInventory == null)
        {
            EditorGUILayout.HelpBox("No CharacterSpellInventory found", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Spell Inventory", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        SerializedObject so = new SerializedObject(spellInventory);
        SerializedProperty spellsProp = so.FindProperty("items");

        so.Update();

        EditorGUILayout.PropertyField(spellsProp, true); // true = рисовать весь список

        if (so.ApplyModifiedProperties())
        {
            Undo.RecordObject(spellInventory, "Modify Spell List");
            EditorUtility.SetDirty(spellInventory);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawConsumableInventory(GameObject obj)
    {
        var consumableInventory = obj.GetComponentInChildren<PlayerConsumableInventory>();

        if (consumableInventory == null)
        {
            EditorGUILayout.HelpBox("No CharacterConsumableInventory found", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Consumable Inventory", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        SerializedObject so = new SerializedObject(consumableInventory);
        SerializedProperty consumableProp = so.FindProperty("items");

        so.Update();

        EditorGUILayout.PropertyField(consumableProp, true); // true = рисовать весь список

        if (so.ApplyModifiedProperties())
        {
            Undo.RecordObject(consumableInventory, "Modify Consumable List");
            EditorUtility.SetDirty(consumableInventory);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawPointsData(GameObject obj)
    {
        var pointsEmitter = obj.GetComponentInChildren<PointsEmitter>();

        if (pointsEmitter == null)
        {
            EditorGUILayout.HelpBox("No PointsEmitter found", MessageType.Warning);
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
}