using UnityEngine;

public enum CombatTransition
{
    Attack = 0,
    Strafe = 1,
    FindWeapon = 2,
    Dodge = 3
}

[System.Serializable]
public class EnemyCombatHandler
{
    CharacterBehaviourStatsSO stats;
    CharacterStatsController statsController;

    [Header("Состояние боя")]
    [SerializeField] private float currCombatCooldown;
    [SerializeField] private float maxCombatCooldown;
    //[SerializeField] private bool isComboRunning;
    
    //power attack
    [SerializeField] private float powerAttackChance;

    [Header("Учёт повреждений")]
    [SerializeField] private float lastDamageTime = -10f;
    //[SerializeField] private int damageCounter;
    

    [SerializeField] private float comboDistanceOffset = 0.2f;

    [Header("Додж")]
    [SerializeField] private float currentDodgeChance;
    [SerializeField] private float dodgeCounterResetTimer = 10f;

    [Header("Стрейф")]
    [SerializeField] private float blockStrafeCooldown = 0f;
    [SerializeField] private float maxBlockStrafeTimer = 5f;
    [SerializeField] private bool isStrafeBlocked = false;

    [Header("Транзит состояний")]
    [SerializeField] private float currAttackTransitionChance;

    public EnemyCombatHandler(CharacterBehaviourStatsSO behaviourStats, CharacterStatsController statsController)
    {
        this.stats = behaviourStats;
        this.statsController = statsController;

        currAttackTransitionChance = behaviourStats.attackTransitionChance;


        powerAttackChance = behaviourStats.initialPoweAttackChance;
        currentDodgeChance = behaviourStats.initialDodgeChance;
    }

    public void ResetCombatState()
    {   
        //isComboRunning = false;
        currCombatCooldown = 0f;
        maxCombatCooldown = 0f;
     
        //damageCounter = 0;
        currentDodgeChance = stats.initialDodgeChance;

        //currAttackTransitionChance = stats.attackTransitionChance;
        //currStrafeTransitionChance = stats.strafeTransitionChance;

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

    #region Power Attack
    public float GetPowerAttackChance() => powerAttackChance;

    public bool WillPowerAttack() => powerAttackChance > Random.value;

    public void IncreasePowerAttackChance() => powerAttackChance += stats.powerAttackChanceMultiplier;

    public void ResetPowerAttackChance()=> powerAttackChance = stats.initialPoweAttackChance;  
    #endregion

    #region Dodge
    public float GetDodgeChance() => currentDodgeChance;

    public void UpdateDodgeCooldown()
    {
        if (Time.time - lastDamageTime > dodgeCounterResetTimer)
        {
            //damageCounter = 0;
            currentDodgeChance = stats.initialDodgeChance;
            isStrafeBlocked = false;
        }
    }

    public void SetStrafeBlocked(bool blocked) => isStrafeBlocked = blocked;    

    public void IncreaseDodgeChance()=> currentDodgeChance += stats.dodgeChanceMultiplier;  

    public void ResetDodgeChance() => currentDodgeChance = stats.initialDodgeChance;

    private bool WillDodge()=> currentDodgeChance >= Random.value;

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
       
        IncreaseDodgeChance();  
        IncreasePowerAttackChance();
    }

    public void OnDamageTaken(Transform attackSource)
    {
        RegisterDamage();
        AdjustChances();
        SetStrafeBlocked(true);
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

    #region Block Strafe 
    public void UpdateBlockStrafeTimer()
    {
        if (!isStrafeBlocked) return;

        blockStrafeCooldown += Time.deltaTime;

        if(blockStrafeCooldown >= maxBlockStrafeTimer)
        {

            blockStrafeCooldown = 0;
            isStrafeBlocked = false;
        }
    }

    public bool IsStrafeBlocked() => isStrafeBlocked;

    #endregion


    public CombatTransition GetNextDecision()
    {
        if (isStrafeBlocked) return AttackOrDodge();
           
        float roll = Random.value;

        if (roll < currAttackTransitionChance)
            return AttackOrDodge();

        return CombatTransition.Strafe;
    }

    private CombatTransition AttackOrDodge()
    {
        if (WillDodge()) return CombatTransition.Dodge;

        return CombatTransition.Attack;
    }

    private void AdjustChances()
    {
        float adjuster = 0.025f;

        currAttackTransitionChance += adjuster;
        currAttackTransitionChance = Mathf.Clamp(currAttackTransitionChance, 0f, 1f);   

       
    }

}
