using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellProjectileViewer : EditorWindow
{
    private Vector2 scroll;
    private List<SpellProjectileSO> projectiles = new();
    private Dictionary<SpellProjectileSO, bool> foldouts = new();

    [MenuItem("Tools/Items Viewer/Spell Projectiles")]
    public static void Open() => GetWindow<SpellProjectileViewer>("Spell Projectiles Viewer");

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
                foldouts[proj] = false;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Existing Spell Projectiles", EditorStyles.boldLabel);
        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var proj in projectiles)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            // »конка слева
            GUILayout.Label(proj.itemImage != null ? AssetPreview.GetAssetPreview(proj.itemImage) : Texture2D.grayTexture,
                GUILayout.Width(50), GUILayout.Height(50));

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(proj.itemName, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Prefab: " + (proj.prefab != null ? proj.prefab.name : "None"));
            EditorGUILayout.EndVertical();

            //  нопка выбрать в проекте
            if (GUILayout.Button("Select", GUILayout.Width(60)))
                Selection.activeObject = proj;

            EditorGUILayout.EndHorizontal();

            // Foldout дл€ деталей
            foldouts[proj] = EditorGUILayout.Foldout(foldouts[proj], "Details");
            if (foldouts[proj])
            {
                EditorGUI.indentLevel++;

                // Ѕазовые параметры
                var baseColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
                DrawColoredBox(() =>
                {
                    EditorGUILayout.LabelField($"Speed: {proj.speed}, Lifetime: {proj.lifetime}, Amount to spawn: {proj.amountToSpawn}");
                    EditorGUILayout.LabelField("Emit Position: " + proj.emitStartingPosition);

                    if (proj is SpellProjectileSO sp)
                    {
                        EditorGUILayout.LabelField("Stamina Penalty: " + sp.staminaPenalty);
                        if (sp.Requirements != null)
                            EditorGUILayout.LabelField("Requirements: " + sp.Requirements.minRequired);
                        if (sp.attackSO != null)
                            EditorGUILayout.LabelField("AttackSO: " + sp.attackSO.name);
                    }
                }, baseColor);

                // DamageData блок
                if (proj.damageData != null)
                {
                    var damageColor = Color.red;
                    DrawColoredBox(() =>
                    {
                        EditorGUILayout.LabelField("DamageData:");
                        EditorGUI.indentLevel++;

                        EditorGUILayout.LabelField("Health Damage Multiplier: " + proj.damageData.healthDamageMultiplier);
                        EditorGUILayout.LabelField("Balance Damage Type: " + proj.damageData.balanceDamageType);
                        EditorGUILayout.LabelField("Impact Force: " + proj.damageData.impactForce);

                        // Ёффекты
                        if (proj.damageData.statusEffectData != null && proj.damageData.statusEffectData.effects != null)
                        {
                            var effectColor = Color.green;
                            DrawColoredBox(() =>
                            {
                                EditorGUILayout.LabelField("Status Effects:");
                                EditorGUI.indentLevel++;
                                foreach (var effect in proj.damageData.statusEffectData.effects)
                                {
                                    // ÷вет дл€ статуса
                                    Color prevColor = GUI.contentColor;
                                    
                                    EditorGUILayout.LabelField($"Stat: {effect.effect.statToAffect} | Type: {effect.effect.effectType} | Operation: {effect.effect.operationType}");
                                    GUI.contentColor = prevColor;
                                }
                                EditorGUI.indentLevel--;
                            }, effectColor);
                        }

                        EditorGUI.indentLevel--;
                    }, damageColor);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        GUILayout.EndScrollView();

        //  нопка обновить список
        if (GUILayout.Button("Refresh List"))
            RefreshProjectiles();
    }

    /// <summary>
    /// ¬спомогательный метод дл€ создани€ цветного фона под блоки
    /// </summary>
    private void DrawColoredBox(System.Action content, Color color)
    {
        var prevColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        EditorGUILayout.BeginVertical("box");
        content.Invoke();
        EditorGUILayout.EndVertical();
        GUI.backgroundColor = prevColor;
    }
}