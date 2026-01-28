using UnityEngine;
using UnityEngine.AI;


public class HumanoidControllerService
{
    public Animator animator;

    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
    public HumanoidAgentController agentController;

    public IDamagable damageController;

    public ICharacterStatsController statsController;
    public HumanoidStats stats;

    public HumanoidControllerService(Animator animator, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator,HumanoidAgentController agentController,ICharacterStatsController statsController,IDamagable damageController , HumanoidStats stats)
    {
      
        this.animator = animator;
        this.aiMotor = aIMotor;
        this.aiAnimatorController = aIAnimator;  
        this.agentController = agentController; 
        this.statsController = statsController;
        this.damageController = damageController;
        this.stats = stats;
    }
}

public class HumanoidCombatControllerServices
{
    public ICombatInventory combatInventory;
    public BaseHumanoidAnimatorController animatorController;

    public HumanoidCombatControllerServices(
        ICombatInventory combatInventory,
        BaseHumanoidAnimatorController animatorController   
       )
    {
        this.combatInventory = combatInventory;
        this.animatorController = animatorController;
    }
}

public class HumanoidDamageControllerService
{
    public IRagdollController ragdollController;
    public HumanoidAIMotor motor;
    public ICharacterStatsController statsModifier;
    public HumanoidStats stats;
    public NavMeshAgent agent;
    public CapsuleCollider col;

    public string uniqueID;

    public HumanoidDamageControllerService(IRagdollController ragdollController, HumanoidAIMotor motor, ICharacterStatsController statsController, HumanoidStats stats, NavMeshAgent agent, CapsuleCollider col, string uniqueID)
    {
        this.ragdollController = ragdollController; 
        this.motor = motor;
        this.statsModifier = statsController;
        this.stats = stats; 
        this.agent = agent;
        this.col = col;
        this.uniqueID = uniqueID;

    }
}

public class HumanoidAnimatorService
{
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public IHumanoidMovement motor;
    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ITargetLocker targetLock;

    public HumanoidAnimatorService(
        Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController)
    {
        this.animator = animator;
        this.overrideController = overrideController;   
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
    }
}

public class HumanoidInteractService
{
    public ICombatInventory combatInventory;
    public IDamagable owner;

    public HumanoidInteractService(ICombatInventory combatInventory, IDamagable owner)
    {
        this.combatInventory = combatInventory;
        this.owner = owner;
    }
}

public class HumanoidCombatInventoryService
{
    public BaseHumanoidAnimatorController animatorController;
    public IHumanoidCombat combatController;
    public ICollector collector;
    //public ICharacterStatsController statsModifier;
    public Transform sourcePosition;
    public int sourceId;

    public HumanoidCombatInventoryService(
       BaseHumanoidAnimatorController animatorController,
        IHumanoidCombat combatController,
        ICollector collector,
        //ICharacterStatsController stats,
        Transform sourcePosition,
        int sourceId)
    {
        this.animatorController = animatorController;   
        this.combatController = combatController;
        this.collector = collector;
        this.sourcePosition = sourcePosition;   
        //this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public class HumanoidStatsControllerService
{
    public HumanoidStats stats;
    public HumanoidStatsControllerService(HumanoidStats stats)
    {
        this.stats = stats;
    }
}

