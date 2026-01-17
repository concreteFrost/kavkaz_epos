using UnityEngine;
using UnityEngine.AI;


public struct HumanoidControllerService
{
    public Animator animator;

    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
   
    public ICharacterStatsController statsController;
    public HumanoidStats stats;

    public HumanoidControllerService(Animator animator, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, ICharacterStatsController statsController, HumanoidStats stats)
    {
        this.animator = animator;
        this.aiMotor = aIMotor;
        this.aiAnimatorController = aIAnimator;   
        this.statsController = statsController;
        this.stats = stats;
    }
}

public struct HumanoidCombatControllerServices
{
    public IAttackSource combatInventory;
    public Animator animator;

    public HumanoidCombatControllerServices(
        IAttackSource combatInventory,
        Animator animator)
    {
        this.combatInventory = combatInventory;
        this.animator = animator;
    }
}

public struct HumanoidDamageControllerService
{
    public IHumanoidMovement motor;
    public ICharacterStatsController statsModifier;
    public HumanoidStats stats;
    public NavMeshAgent agent;
    public CapsuleCollider col;

    public string uniqueID;

    public HumanoidDamageControllerService(IHumanoidMovement motor, ICharacterStatsController statsController, HumanoidStats stats, NavMeshAgent agent, CapsuleCollider col, string uniqueID)
    {
        this.motor = motor;
        this.statsModifier = statsController;
        this.stats = stats; 
        this.agent = agent;
        this.col = col;
        this.uniqueID = uniqueID;

    }
}


public struct CharacterTargetLockService
{

    public HumanoidAIController controller;
    public IDamagable damageController;
    public HumanoidStats stats;

    public CharacterTargetLockService(
        
        HumanoidAIController controller,
        IDamagable damageController,
        HumanoidStats stats)
    {
        this.controller = controller;
        this.damageController = damageController;
        this.stats = stats;
    }
}

public struct HumanoidAnimatorService
{
    public Animator animator;
    public IHumanoidMovement motor;
    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ITargetLocker targetLock;

    public HumanoidAnimatorService(
        Animator animator,
        IHumanoidMovement motor,
        IHumanoidCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController)
    {
        this.animator = animator;
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
    }
}

public struct HumanoidInteractService
{
    public IAttackSource combatInventory;

    public HumanoidInteractService(IAttackSource combatInventory)
    {
        this.combatInventory = combatInventory;
    }
}

public struct HumanoidCombatInventoryService
{
    public IHumanoidCombat combatController;
    //public ICharacterStatsController statsModifier;
    public Transform sourcePosition;
    public int sourceId;

    public HumanoidCombatInventoryService(
        IHumanoidCombat combatController,
        //ICharacterStatsController stats,
        Transform sourcePosition,
        int sourceId)
    {
        this.combatController = combatController;
        this.sourcePosition = sourcePosition;   
        //this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public struct HumanoidStatsControllerService
{
    public HumanoidStats stats;
    public HumanoidStatsControllerService(HumanoidStats stats)
    {
        this.stats = stats;
    }
}

public struct SimpleHumanoidBrainContext
{
    public Vector3 permamentPosition;

    public Transform self;
    public HumanoidAIMotor motor;
    public HumanoidAIController controller;
    public IDamagable damageController;

}

public class EnemyBrainContext
{
    public Vector3 permamentPosition;

    public Transform self;
    public HumanoidAIMotor motor;
    public HumanoidAIController controller;
    public HumanoidStats stats;
    public IDamagable damageController;

    public HumanoidCombatController combat;
    public HumanoidCombatInventory inventory;
    public EnemyFOVController fov;
    public EnemyStateTracker stateTracker;  
    public CharacterInteract interact;

}

