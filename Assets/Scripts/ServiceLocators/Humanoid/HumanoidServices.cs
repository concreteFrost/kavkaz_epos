using UnityEngine;


public struct HumanoidControllerService
{
    public Animator animator;

    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
   
    public ICharacterStatsController statsController;
    public CharacterStats stats;

    public HumanoidControllerService(Animator animator, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, ICharacterStatsController statsController, CharacterStats stats)
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


public struct CharacterTargetLockService
{

    public HumanoidAIController controller;
    public IDamagable damageController;
    public CharacterStats stats;

    public CharacterTargetLockService(
        
        HumanoidAIController controller,
        IDamagable damageController,
        CharacterStats stats)
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
    public ICharacterStatsController statsModifier;
    public int sourceId;

    public HumanoidCombatInventoryService(
        IHumanoidCombat combatController,
        ICharacterStatsController stats,
        int sourceId)
    {
        this.combatController = combatController;
        this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public struct HumanoidStatsControllerService
{
    public CharacterStats stats;
    public HumanoidStatsControllerService(CharacterStats stats)
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

public struct EnemyBrainContext
{
    public Vector3 permamentPosition;

    public Transform self;
    public HumanoidAIMotor motor;
    public HumanoidAIController controller;
    public CharacterStats stats;
    public IDamagable damageController;

    public HumanoidCombatController combat;
    public HumanoidCombatInventory inventory;
    public EnemyFOVController fov;
    public CharacterInteract interact;

}

