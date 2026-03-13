using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellProjectileViewer : EditorWindow
{
    private Vector2 scroll;
    private string search = "";

    private List<SpellProjectileSO> projectiles = new();
    private Dictionary<SpellProjectileSO, bool> foldouts = new();

    [MenuItem("Tools/Items Viewer/Spell Projectiles")]
    public static void Open()
    {
        GetWindow<SpellProjectileViewer>("Spell Projectiles Viewer");
    }

    private void OnEnable()
    {
        RefreshProjectiles();
    }

    private void RefreshProjectiles()
    {
        projectiles.Clear();
        foldouts.Clear();

        var guids = AssetDatabase.FindAssets("t:SpellProjectileSO");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var proj = AssetDatabase.LoadAssetAtPath<SpellProjectileSO>(path);

            projectiles.Add(proj);

            if (!foldouts.ContainsKey(proj))
                foldouts.Add(proj, false);
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(5);

        GUILayout.Label("Spell Projectile Database", EditorStyles.boldLabel);

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        search = EditorGUILayout.TextField("Search", search);

        if (GUILayout.Button("Refresh", GUILayout.Width(80)))
            RefreshProjectiles();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var proj in projectiles)
        {
            if (proj == null)
                continue;

            if (!string.IsNullOrEmpty(search) &&
                !proj.itemName.ToLower().Contains(search.ToLower()))
                continue;

            DrawProjectileCard(proj);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawProjectileCard(SpellProjectileSO proj)
    {
        EditorGUILayout.BeginVertical("box");

        DrawHeader(proj);

        foldouts[proj] = EditorGUILayout.Foldout(foldouts[proj], "Details", true);

        if (foldouts[proj])
        {
            EditorGUI.indentLevel++;

            DrawProjectileBlock(proj);
            DrawSpellBlock(proj);
            DrawDamageBlock(proj);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(4);
    }

    private void DrawHeader(SpellProjectileSO proj)
    {
        EditorGUILayout.BeginHorizontal();

        Texture icon = proj.itemImage != null
            ? AssetPreview.GetAssetPreview(proj.itemImage)
            : Texture2D.grayTexture;

        GUILayout.Label(icon, GUILayout.Width(50), GUILayout.Height(50));

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(proj.itemName, EditorStyles.boldLabel);

        if (proj.prefab != null)
            EditorGUILayout.LabelField("Prefab: " + proj.prefab.name);
        else
            EditorGUILayout.LabelField("Prefab: None");

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Select", GUILayout.Width(60)))
            Selection.activeObject = proj;

        EditorGUILayout.EndHorizontal();
    }

    private void DrawProjectileBlock(SpellProjectileSO proj)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Projectile", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Speed", proj.speed.ToString());
        EditorGUILayout.LabelField("Lifetime", proj.lifetime.ToString());
        EditorGUILayout.LabelField("Amount", proj.amountToSpawn.ToString());
        EditorGUILayout.LabelField("Emit Position", proj.emitStartingPosition.ToString());

        EditorGUILayout.EndVertical();
    }

    private void DrawSpellBlock(SpellProjectileSO proj)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Spell", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Stamina Penalty", proj.staminaPenalty.ToString());

        if (proj.Requirements != null)
            EditorGUILayout.LabelField("Requirements", proj.Requirements.minRequired.ToString());

        //if (proj.attackSO != null)
        //    EditorGUILayout.ObjectField("AttackSO", proj.attackSO, typeof(Object), false);

        if (proj.animation != null)
            EditorGUILayout.ObjectField("Animation", proj.animation, typeof(Object), false);

        EditorGUILayout.EndVertical();
    }

    private void DrawDamageBlock(SpellProjectileSO proj)
    {


        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Health Multiplier", proj.damageData.damageMultiplier.ToString());
        EditorGUILayout.LabelField("Balance Type", proj.damageData.balanceDamageType.ToString());
        EditorGUILayout.LabelField("Impact Force", proj.damageData.impactForce.ToString());

        DrawStatusEffects(proj.damageData);

        EditorGUILayout.EndVertical();
    }

    private void DrawStatusEffects(DamageData damage)
    {
        if (damage.statusEffectData == null)
            return;

        if (damage.statusEffectData.effects == null)
            return;

        GUILayout.Space(3);

        EditorGUILayout.LabelField("Status Effects", EditorStyles.boldLabel);

        foreach (var effect in damage.statusEffectData.effects)
        {
            EditorGUILayout.BeginHorizontal("box");

            var statToAffect = effect.effect.statToAffect.ToString();
            var effectType = effect.effect.effectType.ToString();
            var operationType = effect.effect.operationType.ToString();

            EditorGUILayout.LabelField($"Effect:{effectType} , Affects:{statToAffect} , Method: {operationType}");

            EditorGUILayout.EndHorizontal();
        }
    }
}