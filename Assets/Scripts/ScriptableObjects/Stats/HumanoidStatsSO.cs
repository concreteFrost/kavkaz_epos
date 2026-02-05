
using UnityEngine;

[CreateAssetMenu(fileName = "Humanoid AI Stats", menuName = ScriptablePaths.CHARACTER_STATS_PATH + "/Humanoid AI Stats")]
public class HumanoidStatsSO : BaseCharacterStatsSO
{
    [Header("stamina")]
    public float maxStamina;
    public float staminaRunReducePenalty = 0.03f;
    public float staminaPushReducePenalty = 15f;
    public float staminaJumpReducePenalty = 7f;
    public float staminaDodgeReducePenalty = 10f;
    public float staminaMinRegenDelay = 2f;
    public float staminaMaxRegenDelay = 6f;
    public float staminaRegenRate = 15f;

    [Header("Listening")]
    public float eventListenDistance = 20f;


}
