using UnityEngine;

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

//public struct HumanoidMotorServices
//{
//    public Animator animator;

//    public HumanoidMotorServices(Animator animator)
//    {
//        this.animator = animator;
//    }
//}

public struct CharacterTargetLockService
{

    public HumanoidAIController controller;
    public IDamagable damageController;

    public CharacterTargetLockService(
        
        HumanoidAIController controller,
        IDamagable damageController)
    {
        this.controller = controller;
        this.damageController = damageController;
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
    public ICharacterStatsModifier statsModifier;
    public string sourceId;

    public HumanoidCombatInventoryService(
        IHumanoidCombat combatController,
        ICharacterStatsModifier stats,
        string sourceId)
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

