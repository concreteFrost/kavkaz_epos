using Unity.VisualScripting;
using UnityEngine;

public enum CombatMode
{
    Melee = 0,
    Magic = 1
}
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
    
    //[Header("Состояние боя")]
    //[SerializeField] private float currCombatCooldown;
    //[SerializeField] private float maxCombatCooldown;
    //[SerializeField] private bool isComboRunning;
    [SerializeField] bool canAttack = true;
    
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

        SetCanAttack(true);
        currentDodgeChance = stats.initialDodgeChance;

    }

    #region Combat

    public bool CanAttack() => canAttack;

    public void SetCanAttack(bool val)=> canAttack = val;    

    public float GetAttackDistanceWithOffset(float distance) => distance + comboDistanceOffset;

    public float GetMinAttackCooldown() => stats.minCombatCooldown;
    public float GetMaxAttackCooldown() => stats.maxCombatCooldown;


    //работает только для смешанных врагов
    public CombatMode DecideCombatMode(float distance)
    {
        // Близко → почти всегда ближний бой
        if (distance < 2f)
        {
            return Random.value < 0.8f ? CombatMode.Melee : CombatMode.Magic;
        }

        // Средняя дистанция → смешанное поведение
        if (distance < 5f)
        {
            return Random.value < 0.5f ? CombatMode.Melee : CombatMode.Magic;
        }

        // Далеко → почти всегда магия
        return Random.value < 0.8f ? CombatMode.Magic : CombatMode.Melee;
    }

    #endregion

    #region Power Attack
    public float GetPowerAttackChance() => powerAttackChance;

    public bool WillPowerAttack() => powerAttackChance > Random.value;

    public void IncreasePowerAttackChance() => powerAttackChance += stats.powerAttackChanceMultiplier;

    public void ResetPowerAttackChance()=> powerAttackChance = stats.initialPoweAttackChance;  
    #endregion

    #region Dodge
    public float GetDodgeChance() => currentDodgeChance;

    public void UpdateDodgeChance()
    {
        if (Time.time - lastDamageTime > dodgeCounterResetTimer)
        {
            currentDodgeChance = stats.initialDodgeChance;
            isStrafeBlocked = false;
        }
    }

    public void SetStrafeBlocked(bool blocked) => isStrafeBlocked = blocked;    

    public void IncreaseDodgeChance()=> currentDodgeChance += stats.dodgeChanceMultiplier;  

    public void ResetDodgeChance() => currentDodgeChance = stats.initialDodgeChance;


    #endregion

    #region Distance to Target
    //public bool IsRunningDistance(float distance) => distance > stats.switchToRunDistance;
    public bool IsChaseDistance(float distance) => distance >= stats.maxDistanceInCombat;
    //public bool IsLockOnDistance(float distance) => distance <= stats.targetLockOnDistance;

    #endregion

    #region Damage Handler
    public void RegisterDamage()
    {
        lastDamageTime = Time.time;
       
        IncreaseDodgeChance();  
        IncreasePowerAttackChance();
        IncreastAttackChances();
        
        SetCanAttack(true);
        SetStrafeBlocked(true);
    }

    public void OnDamageTaken(Transform attackSource)
    {
        RegisterDamage();
       
    }
    #endregion

    #region Defence Handler
    public void ToggleShield(bool willRaise, IWeaponSetter inventory, IHumanoidMeleeCombat combatController)
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
        if (currentDodgeChance >= Random.value) 
            return CombatTransition.Dodge;

        return CombatTransition.Attack;
    }

    private void IncreastAttackChances()
    {
        float adjuster = 0.025f;

        currAttackTransitionChance += adjuster;
        currAttackTransitionChance = Mathf.Clamp(currAttackTransitionChance, 0f, 1f);   

       
    }

}
