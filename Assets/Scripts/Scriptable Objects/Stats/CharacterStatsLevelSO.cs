using UnityEngine;

[CreateAssetMenu(fileName = "Character Level", menuName = ScriptablePaths.CHARACTER_LEVEL_PATH + "/Character Level")]
public class CharacterStatsLevelSO : ScriptableObject
{
    [Header("stats level")]
    public int startHealthLevel = 1;
    public int startStaminaLevel = 1;
    public int startKnowledgeLevel = 1;
    public int startStrengthLevel = 1;
}
