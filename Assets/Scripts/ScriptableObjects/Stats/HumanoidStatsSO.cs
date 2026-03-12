
using UnityEngine;

[CreateAssetMenu(fileName = "Humanoid AI Stats", menuName = ScriptablePaths.CHARACTER_STATS_PATH + "/Humanoid AI Stats")]
public class HumanoidStatsSO : BaseCharacterStatsSO
{
    [Header("base stats")]
    public float baseHealth = 100f;
    public float baseStamina = 100f;
    public float baseKnowledge = 100f;
    public float baseStrength = 1.0f;   

    [Header("stats regen ")]
    public float statMinRegenDelay = 2f;
    public float statMaxRegenDelay = 6f;
    public float statRegenRate = 15f;

    [Header("stamina modifiers")]
    public float staminaRunReducePenalty = 0.03f;
    public float staminaPushReducePenalty = 15f;
    public float staminaJumpReducePenalty = 7f;
    public float staminaDodgeReducePenalty = 10f;

    [Header("Listening")]
    public float eventListenDistance = 20f;



}
