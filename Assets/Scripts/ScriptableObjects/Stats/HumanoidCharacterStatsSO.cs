using UnityEngine;

[CreateAssetMenu(fileName = "HumanoidCharacterStatsSO", menuName = "Scriptable Systems/Character/Stats/Humanoid")]
public class HumanoidCharacterStatsSO : BaseCharacterStatsSO
{
    [Header("Behaviours")]
    [Header("Idle")]

    [RangeAttribute(3, 6)]
    public int minIdleStationary;

    [RangeAttribute(6, 20)]
    public int maxIdleStationary;

    [Header("Chase")]
    public float distanceToStop = 2f;
    public float distanceToRun = 5f;
    public float maxCantReachTimer = 7f;
    public float maxLostTargetTimer = 10f;
    public float maxChaseDistance = 17f;
}
