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
    HumanoidStats statsController;

    // combat
    [SerializeField] private float currCombatCooldown;
    [SerializeField] private float maxCombatCooldown;
    //[SerializeField] private bool isComboRunning;
    [SerializeField] private float currAttackChance;
    // dodge
    [SerializeField] private float lastDamageTime = -10f;
    [SerializeField] private int damageCounter;
    [SerializeField] private float currentDodgeChance;

    [SerializeField] private float comboDistanceOffset = 0.2f;
    [SerializeField] private float dodgeCounterResetTimer = 5f;

    // strafe chance
    [SerializeField] private float currStrafeChance;

    public EnemyCombatHandler(CharacterBehaviourStatsSO stats, HumanoidStats statsController)
    {
        this.stats = stats;
        this.statsController = statsController;

        currAttackChance = stats.attackChance;
        currStrafeChance = stats.strafeChance;
    }

    public void ResetCombatState()
    {   
        //isComboRunning = false;
        currCombatCooldown = 0f;
        maxCombatCooldown = 0f;
     
        damageCounter = 0;
        currentDodgeChance = 0f;

        currAttackChance = stats.attackChance;
        currStrafeChance = stats.strafeChance;

    }

    #region Combat

    public bool CanAttack() => currCombatCooldown >= maxCombatCooldown;

    public void UpdateCombatCooldown() => currCombatCooldown += Time.deltaTime;

    public void ResetCombatCooldown(float min, float max)
    {
        currCombatCooldown = 0f;
        maxCombatCooldown = Random.Range(min, max);
    }

    public float GetAttackDistanceWithOffset() => stats.attackDistance + comboDistanceOffset;

    #endregion

    #region Combo
    //public bool IsComboRuning() => isComboRunning;

    //public void SetComboRunning(bool runing) => isComboRunning = runing;
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
    public bool IsCombatDistance(float distance) => distance < stats.maxCombatDistance;
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

    #region Defence Handler
    public void ToggleShield(bool willRaise, ICombatInventory inventory, IHumanoidCombat combatController)
    {
        if (inventory.ShieldWeapon == null && combatController.IsShieldRaised)
        {
            combatController.CancelBlock();
            return;

        }
        if (willRaise)
        {
            combatController.PerformBlock();
        }
        else
        {
            combatController.CancelBlock();
        }

    }

    #endregion


    public CombatTransition GetNextDecision()
    {
        AdjustChances();    

        float sum = currAttackChance + currStrafeChance;
        
        if (sum <= 0f)
            return CombatTransition.Attack;

        float roll = Random.value * sum;

        if (roll < currAttackChance)
            return CombatTransition.Attack;

        return CombatTransition.Strafe;
    }

    private void AdjustChances()
    {
        float healthInfo = Mathf.Clamp01(
            statsController.Health.Current / statsController.maxHealth
        );

        float total = stats.attackChance + stats.strafeChance;

        if (total <= 0f)
        {
            currAttackChance = 0f;
            currStrafeChance = 0f;
            return;
        }

        // 0 → full HP, 1 → low HP
        float aggression = 1f - healthInfo;

        currAttackChance = Mathf.Lerp(stats.attackChance, total, aggression);
        currStrafeChance = total - currAttackChance;
    }






}
