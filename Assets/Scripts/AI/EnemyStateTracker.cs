using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;

    [Header("Idle state")]
    public float currIdleTime=0;
    public float maxIdleTime;

    [Header("Patrol state")]
    public float maxDestinationRadius = 10f;
    public int maxWalks = 3;
    public int currWalks = 0;

    [Header("Target chase state")]
    public float lostTargetTimer;
    public float cantReachTimer;

    [Header("Combat state")]
    // combat
    public float currCombatCooldown;
    public float maxCombatCooldown;
    public bool isComboRunning;

    // dodge
    public float lastDamageTime = -10f;
    public int damageCounter;
    public float currentDodgeChance;

    [Header("Wait state")]
    public float waitTimer = 0f;

    [Header("Strafe state")]
    [SerializeField] private float timeInStrafeState = 0f;
    [SerializeField] private float maxTimeInStrafeState;


    [Header("Interuption Reaction")]
    public Vector3 interruptionDir;
    private float interruptionTimer=0;
    private float maxInterruptionTimer = 2f;
    public bool isInterrupted = false;


    #region Idle State
    public void UpdateCurrentIdleTime() => currIdleTime += Time.deltaTime;

    public void SetMaxIdleTime() =>
        maxIdleTime = Random.Range(
            (float)stats.minIdleStationary,
            (float)stats.maxIdleStationary
        );

    public void ResetIdleState()
    {
        currIdleTime = 0;
    }
    #endregion

    #region Patrol State
    public bool HasReachedMaxWalks() => currWalks >= maxWalks;

    public void IncrementWalks() => currWalks++;

    public void ResetPatrol()
    {
        currWalks = 0;
    }
    #endregion

    #region Target Chase State
    public void ResetChaseState()
    {
        lostTargetTimer = 0f;
        cantReachTimer = 0f;
    }

    public void UpdateLostTargetTimer(bool isVisible) => lostTargetTimer = isVisible ? 0f : lostTargetTimer + Time.deltaTime;
    public void UpdateCantReachTimer(bool canReach)=> cantReachTimer = canReach ? 0f : cantReachTimer + Time.deltaTime;

    #endregion

    #region Combat State

    public void ResetAttackState()
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = 0f;
        isComboRunning = false;

        damageCounter = 0;
        currentDodgeChance = 0f;
    }

    public void UpdateCombatCooldown() => currCombatCooldown += Time.deltaTime;


    public void ResetCombatCooldown(float min, float max)
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = Random.Range(min, max);
    }

    public void RegisterDamage(
    )
    {
        lastDamageTime = Time.time;
        damageCounter++;
        currentDodgeChance = damageCounter * stats.dodgeChanceMultiplier;
    }

    public void UpdateDodgeCooldown(float resetTime)
    {
        if (Time.time - lastDamageTime > resetTime)
        {
            damageCounter = 0;
            currentDodgeChance = 0f;
        }
    }

    #endregion

    #region Target Wait State
    public void UpdateWaitTimer(bool canReach) => waitTimer = canReach ? 0f : waitTimer + Time.deltaTime;

    public void ResetWaitState()=> waitTimer = 0f;

    public void InterruptWait()=> waitTimer = stats.maxWaitTimer;

    #endregion

    #region Strafe State

    public void UpdateTimeInStrafeState() => timeInStrafeState += Time.deltaTime;

    public void SetNewMaxInStrafeTime() => maxTimeInStrafeState = Random.Range(stats.minTimeInStrafeState, stats.maxTimeInStrafeState);

    public void ResetStrafeState()=> timeInStrafeState = 0f; 

    public void InterruptStrafeState() => timeInStrafeState = maxTimeInStrafeState;

    public bool IsStrafeTimeFinished() => timeInStrafeState >= maxTimeInStrafeState;

    public bool IsStrafeTargetFar(float dist) => dist > stats.maxTargetDistanceInStrafe;


    #endregion


    #region Interruption

    public void Interrupt(Vector3 dir)
    {
        interruptionDir = dir;
        isInterrupted = true;
        interruptionTimer = 0f;
    }

    public void UpdateInterruption()
    {
        interruptionTimer += Time.deltaTime;    

        if(interruptionTimer >= maxInterruptionTimer)
        {
            ResetInterruption();
        }
    }

    public void ResetInterruption()
    {
        isInterrupted = false;
        interruptionTimer = 0f;
        interruptionDir = Vector3.zero;
    }

    public bool IsInterrupted() => isInterrupted;
    #endregion
}
