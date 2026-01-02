using UnityEngine;

public struct PlayerControllerService
{
    public PlayerMotor controller;
    public HumanoidCombatController combatController;
    public PlayerDamageController damageController;
    public PlayerStatsController statsController;
    public PlayerStats stats;
    public PlayerInteract interact;
    public PlayerClimbing climbing;
    public Animator animator;

    public PlayerControllerService(
        PlayerMotor controller,
        HumanoidCombatController combatController,
        PlayerDamageController damageController,
        PlayerStats stats,
        PlayerStatsController statsModifier,
        PlayerInteract interact,
        PlayerClimbing climbing,
        Animator animator)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.damageController = damageController;
        this.stats = stats;
        this.interact = interact;
        this.climbing = climbing;
        this.animator = animator;
        this.statsController = statsModifier;
    }
}

public struct PlayerInputService
{
    public PlayerController controller;
    public PlayerAnimator animator;
    public PlayerTargetLock targetLock;

    public PlayerInputService(
        PlayerController controller,
        PlayerAnimator animator,
        PlayerTargetLock targetLock)
    {
        this.controller = controller;
        this.animator = animator;
        this.targetLock = targetLock;
    }
}

public struct PlayerAnimatorService
{
    public Animator animator;
    public PlayerMotor motor;
    public HumanoidCombatController combatController;
    public PlayerDamageController damageController;
    public PlayerTargetLock targetLock;

    public PlayerAnimatorService(
        Animator animator,
        PlayerMotor motor,
        HumanoidCombatController combatController,
        PlayerTargetLock targetLock,
        PlayerDamageController damageController)
    {
        this.animator = animator;
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
    }
}


public struct PlayerInteractService
{
    public PlayerCombatInventory combatInventory;

    public PlayerInteractService(PlayerCombatInventory combatInventory)
    {
        this.combatInventory = combatInventory;
    }
}

public struct PlayerCombatInventoryService
{
    public HumanoidCombatController combatController;
    public PlayerStatsController statsModifier;
    public string sourceId;

    public PlayerCombatInventoryService(
        HumanoidCombatController combatController,
        PlayerStatsController stats,
        string sourceId)
    {
        this.combatController = combatController;
        this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public struct PlayerStatsService
{
    public PlayerCombatInventory combatInventory;
    public PlayerMotor motor;
    public PlayerInput input;

    public PlayerStatsService(
        PlayerCombatInventory combatInventory,
        PlayerMotor motor,
        PlayerInput input)
    {
        this.combatInventory = combatInventory;
        this.motor = motor;
        this.input = input;
    }
}


public struct PlayerStatsControllerService
{
    public PlayerStats stats;

    public PlayerStatsControllerService(PlayerStats stats)
    {
        this.stats = stats;
    }
}

public struct PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public PlayerDamageController damageController;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        PlayerDamageController damageController)
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
    }
}




