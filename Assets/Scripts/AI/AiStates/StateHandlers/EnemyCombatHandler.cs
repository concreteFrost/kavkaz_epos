using Unity.VisualScripting;
using UnityEngine;

public enum CombatTransition
{
    Attack = 0,
    Strafe = 1,
    FindWeapon = 2,
    Dodge =3
}

[System.Serializable]
public class EnemyCombatHandler
{
    CharacterBehaviourStatsSO stats;

    // combat
    [SerializeField] private float currCombatCooldown;
    [SerializeField] private float maxCombatCooldown;
    [SerializeField] private bool isComboRunning;

    // dodge
    [SerializeField] private float lastDamageTime = -10f;
    [SerializeField] private int damageCounter;
    [SerializeField] private float currentDodgeChance;

    [SerializeField] private float comboDistanceOffset = 0.2f;
    [SerializeField] private float dodgeCounterResetTimer = 5f;

    public EnemyCombatHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    public void ResetAttackState()
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = 0f;
        isComboRunning = false;

        damageCounter = 0;
        currentDodgeChance = 0f;
    }

    #region Combat

    public void UpdateCombatCooldown() => currCombatCooldown += Time.deltaTime;

    public void ResetCombatCooldown(float min, float max)
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = Random.Range(min, max);
    }

    public float GetAttackDistanceWithOffset() => stats.attackDistance + comboDistanceOffset;

    #endregion

    #region Combo
    public bool IsComboRuning() => isComboRunning;

    public void SetComboRunning(bool runing) => isComboRunning = runing;
    #endregion

    #region Dodge
    public float GetDodgeChance() => currentDodgeChance;

    public void UpdateDodgeCooldown()
    {
        if (Time.time - lastDamageTime > dodgeCounterResetTimer)
        {
            damageCounter = 0;
            currentDodgeChance = 0f;
        }
    }

    public void ResetDodgeChance()
    {
        damageCounter = 0;
        currentDodgeChance = 0f;
    }
    #endregion

    #region Distance to Target
    public bool IsRunningDistance(float distance) => distance > stats.distanceToRun;
    public bool IsComboDistance(float distance) => distance < stats.maxCombatDistance;
    public bool IsInAttackRange(float distance) => distance < stats.attackDistance;

    #endregion


    #region Damage Handler
    public void RegisterDamage()
    {
        lastDamageTime = Time.time;
        damageCounter++;
        currentDodgeChance = damageCounter * stats.dodgeChanceMultiplier;
    }

    public void OnDamageTaken(Transform attackSource)
    {
        RegisterDamage();
    }
    #endregion


    public CombatTransition GetNextDecision(float dist)
    {
        float attackWeight = GetAttackWeight(dist);
        float dodgeWeight = GetDodgeWeight(dist);
        float strafeWeight = GetStrafeWeight(dist);

        float sum = attackWeight + dodgeWeight + strafeWeight;
        
        if (sum <= 0f)
            return CombatTransition.Attack;

        float roll = Random.value * sum;

        if (roll < attackWeight)
            return CombatTransition.Attack;

        roll -= attackWeight;

        if (roll < dodgeWeight)
            return CombatTransition.Dodge;

        return CombatTransition.Strafe;
    }

    private float GetAttackWeight(float dist)
    {
        if (!IsInAttackRange(dist))
            return 0f;

        if (isComboRunning)
            return 0f;

        return stats.attackChance;
    }

    private float GetDodgeWeight(float dist)
    {

        return currentDodgeChance;
    }

    private float GetStrafeWeight(float dist)
    {
        return stats.strafeChance;
    }




}
