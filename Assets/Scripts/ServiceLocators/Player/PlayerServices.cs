using UnityEngine;

public class PlayerInputServiceProvider
{
    public PlayerController controller;
    public PlayerCombatController combatController;
    public PlayerInteract interact;
    public PlayerAnimator animator;
    public PlayerTargetLock targetLock;

    public PlayerInputServiceProvider(PlayerController controller, PlayerCombatController combatController, PlayerInteract interact, PlayerAnimator animator, PlayerTargetLock targetLock)
    {
        this.controller = controller;
        this.combatController = combatController;  
        this.interact = interact;
        this.animator = animator;
        this.targetLock = targetLock;
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
    public PlayerStats stats;
    public PlayerStatsModifier statsModifier;
    public PlayerCombatController combatController;
    public PlayerControllerServiceProvider(Animator animator,PlayerStats stats ,PlayerStatsModifier statsModifier, PlayerCombatController combatController)
    {
        this.animator = animator;
        this.stats = stats;
        this.statsModifier = statsModifier;
        this.combatController = combatController;
    }
}

public class PlayerCombatControllerServiceProvider
{
    public PlayerController motor;
    public PlayerCombatInventory combatInventory;
    public PlayerStats stats;
    public PlayerStatsModifier statsModifier;   
    public PlayerCombatControllerServiceProvider(PlayerController motor, PlayerCombatInventory combatInventory, PlayerStats stats, PlayerStatsModifier statsModifier)
    {
        this.motor = motor;
        this.combatInventory = combatInventory;
        this.stats = stats;
        this.statsModifier = statsModifier;
    }
}

public class PlayerInteractServiceProvider
{
    public PlayerController motor;
    public PlayerCombatInventory combatInventory;
    public PlayerCombatController combatController;
    public PlayerInteractServiceProvider(PlayerController motor, PlayerCombatInventory combatInventory, PlayerCombatController combatController)
    {
        this.motor = motor;
        this.combatInventory = combatInventory;
        this.combatController = combatController;
    }
}

public class PlayerStatsServiceProvider
{
    public PlayerCombatInventory combatInventory;
    public PlayerController motor;
    public PlayerStatsUI playerStatsUI;
    public PlayerInput input;
    public PlayerStatsServiceProvider(PlayerCombatInventory combatInventory, PlayerController motor, PlayerStatsUI playerStatsUI, PlayerInput input)
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

    public PlayerStatsModifierServiceProvider(string uniqueId, PlayerStats stats, PlayerStatsUI ui, PlayerInput input,PlayerCombatController combatController, PlayerCombatInventory inventory, PlayerMotor animator)
    {
        this.uniqueId = uniqueId; 
        this.stats= stats;  
        this.ui = ui;
        this.combatController=combatController; 
        this.input = input;
        this.inventory = inventory;
        this.animator = animator;
    }
}


