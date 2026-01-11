using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;

    [Header("Idle state")]
    public float currIdleTime;
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

    public void UpdateLostTargetTimer(bool isVisible)
    {
        lostTargetTimer = isVisible ? 0f : lostTargetTimer + Time.deltaTime;
    }

    public void UpdateCantReachTimer(bool canReach)
    {
        cantReachTimer = canReach ? 0f : cantReachTimer + Time.deltaTime;
    }

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

    public void UpdateCombatCooldown()
    {
        currCombatCooldown += Time.deltaTime;
    }

    public void ResetCombatCooldown(float min, float max)
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = Random.Range(min, max);
    }

    public void RegisterDamage(
        float dodgeChanceMultiplier
    )
    {
        lastDamageTime = Time.time;
        damageCounter++;
        currentDodgeChance = damageCounter * dodgeChanceMultiplier;
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

}
