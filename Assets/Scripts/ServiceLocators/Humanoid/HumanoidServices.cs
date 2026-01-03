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

public struct HumanoidMotorServices
{
    public Animator animator;

    public HumanoidMotorServices(Animator animator)
    {
        this.animator = animator;
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

