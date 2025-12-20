using UnityEngine;


public class PlayerStateServiceProvider
{
    public PlayerMotor controller;
    public PlayerCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerStats stats;
    public PlayerTargetLock targetLock;
    public PlayerInteract interact;

    public PlayerStateServiceProvider(PlayerMotor controller, PlayerCombatController combatController, PlayerStatsModifier statsModifier, PlayerStats stats, PlayerTargetLock targetLock, PlayerInteract interact)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.stats = stats;
        this.targetLock = targetLock;
        this.interact = interact;
    }
}

public class PlayerInputServiceProvider
{

    public PlayerController controller;
    public PlayerAnimator animator;

    public PlayerInputServiceProvider(PlayerController controller,  PlayerAnimator animator)
    {
        this.controller = controller;
        this.animator = animator;
    }
}

public class PlayerAnimatorServiceProvider
{
    public Animator animator;
    public PlayerAnimatorServiceProvider(Animator animator)
    {
        this.animator = animator;
    }
}

public class PlayerControllerServiceProvider
{
    public Animator animator;

    public PlayerControllerServiceProvider(Animator animator)
    {
        this.animator = animator;
    }
}

public class PlayerCombatControllerServiceProvider
{
    public PlayerCombatInventory combatInventory;
    public PlayerCombatControllerServiceProvider(PlayerCombatInventory combatInventory)
    {
        this.combatInventory = combatInventory;
    }
}

public class PlayerInteractServiceProvider
{

    public PlayerCombatInventory combatInventory;

    public PlayerInteractServiceProvider(PlayerCombatInventory combatInventory)
    {
        this.combatInventory = combatInventory;

    }
}

public class PlayerStatsServiceProvider
{
    public PlayerCombatInventory combatInventory;
    public PlayerMotor motor;
    public PlayerStatsUI playerStatsUI;
    public PlayerInput input;
    public PlayerStatsServiceProvider(PlayerCombatInventory combatInventory, PlayerMotor motor, PlayerStatsUI playerStatsUI, PlayerInput input)
    {
        this.combatInventory = combatInventory;
        this.motor = motor;
        this.playerStatsUI = playerStatsUI;
        this.input = input;
    }
}

public class PlayerCombatInventoryServiceProvider
{
    public PlayerCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerCombatInventoryServiceProvider(PlayerCombatController combatController, PlayerStatsModifier stats)
    {
        this.combatController = combatController;
        this.statsModifier = stats;
    }
}

public class PlayerStatsModifierServiceProvider
{
    public string uniqueId;
    public PlayerStats stats;
    public PlayerStatsUI ui;
    public PlayerInput input;
    public PlayerCombatController combatController;
    public PlayerCombatInventory inventory;
    public PlayerMotor animator;

    public PlayerStatsModifierServiceProvider(string uniqueId, PlayerStats stats, PlayerStatsUI ui, PlayerInput input, PlayerCombatController combatController, PlayerCombatInventory inventory, PlayerMotor animator)
    {
        this.uniqueId = uniqueId;
        this.stats = stats;
        this.ui = ui;
        this.combatController = combatController;
        this.input = input;
        this.inventory = inventory;
        this.animator = animator;
    }
}



