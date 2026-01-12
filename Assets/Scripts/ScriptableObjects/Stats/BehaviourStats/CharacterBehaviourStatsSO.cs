using UnityEngine;

[CreateAssetMenu(fileName = "Character Behaviour Stats", menuName = "Scriptable Systems/Character/Behaviour Stats/Character Behaviour Stats")]
public class CharacterBehaviourStatsSO : ScriptableObject
{

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
    public float maxWaitTimer = 7f;

    [Header("Combat")]
    public float maxCombatDistance = 8f;
    public float attackDistance = 1.3f;

    [RangeAttribute(0, 1f)]
    public float dodgeChanceMultiplier = 0.15f;
}
