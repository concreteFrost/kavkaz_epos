using UnityEngine;


public class PlayerControllerService
{
    public PlayerMotor controller;
    public HumanoidCombatController combatController;
    public PlayerStatsController statsModifier;
    public PlayerStats stats;
    public PlayerInteract interact;
    public PlayerClimbing climbing;
    public Animator animator;

    public PlayerControllerService(PlayerMotor controller, HumanoidCombatController combatController, PlayerStatsController statsModifier, PlayerStats stats, PlayerInteract interact, PlayerClimbing climbing, Animator animator)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.stats = stats;
        this.interact = interact;
        this.climbing = climbing;
        this.animator = animator;
    }

}

public class PlayerInputServiceProvider
{

    public PlayerController controller;
    public PlayerAnimator animator;
    public PlayerTargetLock targetLock;

    public PlayerInputServiceProvider(PlayerController controller, PlayerAnimator animator, PlayerTargetLock targetLock)
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
    public HumanoidCombatController combatController;
    public PlayerStatsController statsModifier;
    public PlayerTargetLock targetLock;
   
    public PlayerAnimatorServiceProvider(Animator animator, PlayerMotor motor, HumanoidCombatController combatController, PlayerStatsController statsModifier, PlayerTargetLock targetLock)
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
    public HumanoidCombatController combatController;
    public PlayerStatsController statsModifier;
    public string sourceId;

    public PlayerCombatInventoryServiceProvider(HumanoidCombatController combatController, PlayerStatsController stats, string sourceId)
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
    public HumanoidCombatController combatController;
    public PlayerCombatInventory inventory;

    public PlayerStatsModifierServiceProvider(string uniqueId, PlayerStats stats, PlayerStatsUI ui, PlayerInput input, HumanoidCombatController combatController, PlayerCombatInventory inventory)
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
    public PlayerStatsController statsModifier;

    public PlayerTargetLockServiceProvider(LockOnTargetUI lockOnTargetUI, PlayerController controller, PlayerStatsController statsModifier)
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.statsModifier = statsModifier;
    }
}


