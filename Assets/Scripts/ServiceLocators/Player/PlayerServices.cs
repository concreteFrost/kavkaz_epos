using UnityEngine;

public class PlayerStateServices
{
    public PlayerMotor controller;
    public HumanoidCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerStats stats;
    public PlayerInteract interact;

    public PlayerStateServices(PlayerMotor controller,HumanoidCombatController combatController, PlayerStatsModifier statsModifier, PlayerStats stats, PlayerInteract interact)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.stats = stats;
        this.interact = interact;
    }
}

public class PlayerInputServices
{

    public PlayerController controller;
    public PlayerAnimator animator;
    public PlayerTargetLock targetLock;

    public PlayerInputServices(PlayerController controller, PlayerAnimator animator, PlayerTargetLock targetLock)
    {
        this.controller = controller;
        this.animator = animator;
        this.targetLock = targetLock;
    }
}

public class PlayerAnimatorServices
{
    public Animator animator;
    public PlayerMotor motor;
    public HumanoidCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public PlayerTargetLock targetLock;
    public PlayerAnimatorServices(Animator animator, PlayerMotor motor, HumanoidCombatController combatController, PlayerStatsModifier statsModifier, PlayerTargetLock targetLock)
    {
        this.animator = animator;
        this.motor = motor;
        this.combatController = combatController;
        this.statsModifier = statsModifier;
        this.targetLock = targetLock;
    }

}

public class PlayerControllerServices
{
    public Animator animator;

    public PlayerControllerServices(Animator animator)
    {
        this.animator = animator;
    }
}


public class PlayerInteractServices
{

    public PlayerCombatInventory combatInventory;

    public PlayerInteractServices(PlayerCombatInventory combatInventory)
    {
        this.combatInventory = combatInventory;
    }
}

public class PlayerStatsServices
{
    public PlayerCombatInventory combatInventory;
    public PlayerMotor motor;
    public PlayerStatsUI playerStatsUI;
    public PlayerInput input;
    public PlayerStatsServices(PlayerCombatInventory combatInventory, PlayerMotor motor, PlayerStatsUI playerStatsUI, PlayerInput input)
    {
        this.combatInventory = combatInventory;
        this.motor = motor;
        this.playerStatsUI = playerStatsUI;
        this.input = input;
    }
}

public class PlayerCombatInventoryServices
{
    public HumanoidCombatController combatController;
    public PlayerStatsModifier statsModifier;
    public string sourceId;

    public PlayerCombatInventoryServices(HumanoidCombatController combatController, PlayerStatsModifier stats, string sourceId)
    {
        this.combatController = combatController;
        this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public class PlayerStatsModifierServices
{
    public string uniqueId;
    public PlayerStats stats;
    public PlayerStatsUI ui;
    public PlayerInput input;
    public HumanoidCombatController combatController;
    public PlayerCombatInventory inventory;

    public PlayerStatsModifierServices(string uniqueId, PlayerStats stats, PlayerStatsUI ui, PlayerInput input, HumanoidCombatController combatController, PlayerCombatInventory inventory)
    {
        this.uniqueId = uniqueId;
        this.stats = stats;
        this.ui = ui;
        this.combatController = combatController;
        this.input = input;
        this.inventory = inventory;
    }
}

public class PlayerTargetLockServices
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public PlayerStatsModifier statsModifier;

    public PlayerTargetLockServices(LockOnTargetUI lockOnTargetUI, PlayerController controller, PlayerStatsModifier statsModifier)
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.statsModifier = statsModifier;
    }
}


