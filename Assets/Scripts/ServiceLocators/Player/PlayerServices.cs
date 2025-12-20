using UnityEngine;

public class PlayerStateServiceProvider
{
    public PlayerMotor controller;
    public PlayerCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerStats stats;
    public PlayerInteract interact;

    public PlayerStateServiceProvider(PlayerMotor controller, PlayerCombatController combatController, PlayerStatsModifier statsModifier, PlayerStats stats, PlayerInteract interact)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.stats = stats;
        this.interact = interact;
    }
}

public class PlayerInputServiceProvider
{

    public PlayerController controller;
    public PlayerAnimator animator;
    public PlayerTargetLock targetLock;

    public PlayerInputServiceProvider(PlayerController controller,  PlayerAnimator animator, PlayerTargetLock targetLock)
    {
        this.controller = controller;
        this.animator = animator;
        this.targetLock = targetLock;
    }
}

public class PlayerAnimatorServiceProvider
{
    public Animator animator;
    public PlayerMotor motor;
    public PlayerCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerTargetLock targetLock;
    public PlayerAnimatorServiceProvider (Animator animator, PlayerMotor motor, PlayerCombatController combatController, PlayerStatsModifier statsModifier, PlayerTargetLock targetLock)
    {
        this.animator = animator;
        this.motor = motor;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.targetLock = targetLock;
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
    public Animator animator;

    public PlayerCombatControllerServiceProvider(PlayerCombatInventory combatInventory, Animator animator)
    {
        this.combatInventory = combatInventory;
        this.animator = animator;
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
    public string sourceId;
    
    public PlayerCombatInventoryServiceProvider(PlayerCombatController combatController, PlayerStatsModifier stats, string sourceId)
    {
        this.combatController = combatController;
        this.statsModifier = stats;
        this.sourceId = sourceId;
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

    public PlayerStatsModifierServiceProvider(string uniqueId, PlayerStats stats, PlayerStatsUI ui, PlayerInput input, PlayerCombatController combatController, PlayerCombatInventory inventory)
    {
        this.uniqueId = uniqueId;
        this.stats = stats;
        this.ui = ui;
        this.combatController = combatController;
        this.input = input;
        this.inventory = inventory;
    }
}

public class PlayerTargetLockServiceProvider
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public PlayerStatsModifier statsModifier;

    public PlayerTargetLockServiceProvider(LockOnTargetUI lockOnTargetUI, PlayerController controller, PlayerStatsModifier statsModifier)
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.statsModifier = statsModifier;
    }
}



