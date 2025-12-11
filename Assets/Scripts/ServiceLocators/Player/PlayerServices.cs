using UnityEngine;

public class PlayerInputServiceProvider
{
    public PlayerController controller;
    public PlayerCombatController combatController;
    public vThirdPersonCamera vThirdPersonCamera;
    public PlayerInteract interact;
    public CharacterAnimator animator;

    public PlayerInputServiceProvider(PlayerController controller, PlayerCombatController combatController, vThirdPersonCamera vThirdPersonCamera, PlayerInteract interact, CharacterAnimator animator)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.vThirdPersonCamera = vThirdPersonCamera;
        this.interact = interact;
        this.animator = animator;
       
    }
}

public class CharacterAnimatorServiceProvider
{
    public Animator animator;
    public CharacterAnimatorServiceProvider(Animator animator)
    {
        this.animator = animator;
    }
}

public class PlayerControllerServiceProvider
{
    public Animator animator;
    public PlayerStats stats;
    public PlayerControllerServiceProvider(Animator animator, PlayerStats stats)
    {
        this.animator = animator;
        this.stats = stats;
    }
}

public class PlayerCombatControllerServiceProvider
{
    public PlayerController motor;
    public PlayerCombatInventory combatInventory;
    public PlayerStats stats;
    public PlayerCombatControllerServiceProvider(PlayerController motor, PlayerCombatInventory combatInventory, PlayerStats stats)
    {
        this.motor = motor;
        this.combatInventory = combatInventory;
        this.stats = stats;
    }
}

public class PlayerInteractServiceProvider
{
    public PlayerController motor;
    public PlayerCombatInventory combatInventory;
    public PlayerInteractServiceProvider(PlayerController motor, PlayerCombatInventory combatInventory)
    {
        this.motor = motor;
        this.combatInventory = combatInventory;
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
    public PlayerController motor;
    public PlayerStats stats;
    public PlayerCombatInventoryServiceProvider(PlayerController motor, PlayerStats stats)
    {
        this.motor = motor;
        this.stats = stats;
    }
}


