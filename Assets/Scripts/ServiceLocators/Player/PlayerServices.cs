using UnityEngine;

public struct PlayerControllerService
{
    public PlayerMotor controller;
    public CharacterStats stats;
    public Animator animator;

    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ICharacterStatsModifier statsController;
    public ICollector interact;
    public PlayerClimbing climbing;

    public PlayerControllerService(
        PlayerMotor controller,
        IHumanoidCombat combatController,
        IDamagable damageController,
        CharacterStats stats,
        ICharacterStatsModifier statsModifier,
        ICollector interact,
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
    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public PlayerTargetLock targetLock;

    public PlayerAnimatorService(
        Animator animator,
        PlayerMotor motor,
        IHumanoidCombat combatController,
        PlayerTargetLock targetLock,
        IDamagable damageController)
    {
        this.animator = animator;
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
    }
}


public struct PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public IDamagable damageController;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        IDamagable damageController)
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
    }
}

public struct PlayerDamageControllerServices
{
    public ICharacterStatsModifier statsController;
    public CharacterStats stats;
    public PlayerInput input;

    public IHumanoidCombat combatController;
    public IAttackSource attackSource;

    public PlayerDamageControllerServices(ICharacterStatsModifier statsController, CharacterStats stats, PlayerInput input,  IHumanoidCombat combatController, IAttackSource attackSource)
    {
        this.statsController = statsController;
        this.stats = stats;
        this.input = input;
  
        this.attackSource = attackSource;
        this.combatController = combatController;

    }
}




