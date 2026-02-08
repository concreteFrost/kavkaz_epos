using UnityEngine;

[CreateAssetMenu(fileName = "Character Behaviour Stats", menuName = ScriptablePaths.CHARACTER_BEHAVIOUR_STATS_PATH + "/Character Behaviour Stats")]
public class CharacterBehaviourStatsSO : ScriptableObject
{

    [Header("Idle")]
    [RangeAttribute(3, 6)]
    public int minIdleStationary;

    [RangeAttribute(6, 20)]
    public int maxIdleStationary;

    [Header("Patrol")]
    public float maxDestiantionRadius = 10f;
    public int maxPatrolAttempts = 3;

    [Header("Chase")]
    public float distanceToStop = 2f;
    public float distanceToRun = 3f;
    public float maxCantReachTimer = 7f;
    public float maxLostTargetTimer = 10f;
    public float maxChaseDistance = 17f;
    public float maxWaitTimer = 7f;

    [Header("Combat")]
    public float attackTransitionChance = .8f;
    public float initialPoweAttackChance = 0.15f;
    public float maxCombatDistance = 8f;
    public float attackDistance = 1.3f;
    public float powerAttackChanceMultiplier = 0.05f;

    [RangeAttribute(0, 1f)]
    public float initialDodgeChance = 0.2f;
    public float dodgeChanceMultiplier = 0.15f;

    [Header("Strafe")]
    //public float strafeTransitionChance = .2f;
    public float maxTargetDistanceInStrafe = 10f;

    [RangeAttribute(3, 7)]
    public int minTimeInStrafeState=7;

    [RangeAttribute(7, 12)]
    public int maxTimeInStrafeState=12;

  
    
}
