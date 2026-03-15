using UnityEditor;


public class SpellProjectileCreatorTool : BaseItemCreatorTool<SpellProjectileSO>
{
    protected override string ItemFolder => $"{basePath}/Spells/";


    protected override void DrawItem(SpellProjectileSO item)
    {
        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();

        EditorGUILayout.BeginVertical("box");

        DrawPrefab(so);
        EditorGUILayout.Space(20);

        DrawAnimation(so);
        EditorGUILayout.Space(20);

        DrawDamage(so);
        EditorGUILayout.Space(20);

        DrawAttack(so);
        EditorGUILayout.Space(20);

        DrawMovement(so);
        EditorGUILayout.Space(20);

        DrawSpawning(so);
        EditorGUILayout.Space(20);

        DrawRequirements(so);
        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();
    }

    private void DrawPrefab(SerializedObject so)
    {
        SerializedProperty prefab = so.FindProperty("prefab");
        EditorGUILayout.PropertyField(prefab);
    }

    private void DrawAnimation(SerializedObject so)
    {

        SerializedProperty castAnimation = so.FindProperty("castAnimation");
        EditorGUILayout.PropertyField(castAnimation);
    }

    private void DrawDamage(SerializedObject so)
    {

        SerializedProperty baseDamage = so.FindProperty("baseDamage");
        EditorGUILayout.PropertyField(baseDamage);

        SerializedProperty damageData = so.FindProperty("damageData");
        EditorGUILayout.PropertyField(damageData);
    }

    private void DrawAttack(SerializedObject so)
    {
        SerializedProperty attackSO = so.FindProperty("attackSO");
        EditorGUILayout.PropertyField(attackSO);
    }

    private void DrawMovement(SerializedObject so)
    {

        SerializedProperty moveSo = so.FindProperty("moveSO");
        EditorGUILayout.PropertyField(moveSo);

        SerializedProperty speed = so.FindProperty("speed");
        EditorGUILayout.PropertyField(speed);

        SerializedProperty lifetime = so.FindProperty("lifetime");
        EditorGUILayout.PropertyField(lifetime);
    }

    private void DrawSpawning(SerializedObject so)
    {

        SerializedProperty amountToSpawn = so.FindProperty("amountToSpawn");
        EditorGUILayout.PropertyField(amountToSpawn);

        SerializedProperty spawnDelay = so.FindProperty("spawnDelay");
        EditorGUILayout.PropertyField(spawnDelay);

        SerializedProperty emitStartingPosition = so.FindProperty("emitStartingPosition");
        EditorGUILayout.PropertyField(emitStartingPosition);
    }

    private void DrawRequirements(SerializedObject so)
    {
        SerializedProperty staminaPenalty = so.FindProperty("staminaPenalty");
        EditorGUILayout.PropertyField(staminaPenalty);

        SerializedProperty requirements = so.FindProperty("requirements");
        EditorGUILayout.PropertyField(requirements);
    }

  
}