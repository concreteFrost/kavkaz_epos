using UnityEngine;
using UnityEditor;

public class CharacterLevelTesterWindow : EditorWindow
{
    GameObject temp;
    CharacterLevelData levelData;
    CharacterLevelController levelController;
    CharacterStatsController statsController;

    int addXPAmount = 100;
    StatType statToSpend = StatType.Health;

    [MenuItem("Tools/Character Tools/Level Tester")]
    public static void ShowWindow()
    {
        GetWindow<CharacterLevelTesterWindow>("Level Tester");
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        temp = new GameObject("TempLevelController");

        levelController = temp.AddComponent<CharacterLevelController>();
        statsController = temp.AddComponent<CharacterStatsController>();
        statsController.statsSO = ScriptableObject.CreateInstance<HumanoidStatsSO>();
        statsController.statsLevelSO = ScriptableObject.CreateInstance<CharacterStatsLevelSO>();

        levelData = new CharacterLevelData();

        statsController.Init();
        levelController.Init(statsController);
        levelController.levelData = levelData;
    }

    private void OnDisable()
    {
        if (temp != null)
        {
            DestroyImmediate(temp);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Level Testing", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // Показ состояния
        EditorGUILayout.LabelField("Level", levelData.currentCharacterLevel.ToString());
        EditorGUILayout.LabelField("XP", levelData.currentXP.ToString());
        EditorGUILayout.LabelField("XP to next level", levelData.xpToNextLevel.ToString());
        EditorGUILayout.LabelField("Unspent Points", levelData.unspentPoints.ToString());

        EditorGUILayout.Space();

        GUILayout.Label("Health", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Health Level", statsController.healthLevel.ToString());
        EditorGUILayout.LabelField("Current Health Points", statsController.Health.CurrentMax.ToString());
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Stamina Level", statsController.staminaLevel.ToString());
        EditorGUILayout.LabelField("Knowledge Level", statsController.knowledgeLevel.ToString());
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Strength Level", statsController.strengthLevel.ToString()); 
        EditorGUILayout.LabelField("Current Strength Points", statsController.Strength.CurrentMax.ToString());  

        // Добавление XP
        addXPAmount = EditorGUILayout.IntField("Add XP amount", addXPAmount);
        if (GUILayout.Button("Add XP"))
        {
            levelController.AddXP(addXPAmount);
        }

        EditorGUILayout.Space();

        // Потратить очко
        statToSpend = (StatType)EditorGUILayout.EnumPopup("Spend Point on", statToSpend);
        if (GUILayout.Button("Spend Point"))
        {
            levelController.SpendPoint(statToSpend);
        }

        EditorGUILayout.Space();

        // Сброс
        if (GUILayout.Button("Reset"))
        {
            if (temp != null)
            {
                DestroyImmediate(temp);
            }

            Init();
        }
    }
}