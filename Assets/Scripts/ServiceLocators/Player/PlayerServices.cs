using UnityEngine;

public struct PlayerControllerService
{
    public PlayerMotor controller;
    public HumanoidStats stats;
    public Animator animator;

    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ICharacterStatsController statsController;
    public ICollector interact;
    public ITargetLocker locker;
    public PlayerClimbing climbing;

    public PlayerControllerService(
        PlayerMotor controller,
        IHumanoidCombat combatController,
        IDamagable damageController,
        HumanoidStats stats,
        ICharacterStatsController statsModifier,
        ICollector interact,
        ITargetLocker locker,
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
        this.locker = locker;   
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

public struct PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public IDamagable damageController;
    public HumanoidStats stats;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        IDamagable damageController,
        HumanoidStats stats
        )
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
        this.stats= stats;  
    }
}

public struct PlayerDamageControllerServices
{
    public ICharacterStatsController statsController;
    public HumanoidStats stats;
    public PlayerInput input;

    public IHumanoidCombat combatController;
    public IAttackSource attackSource;

    public PlayerDamageControllerServices(ICharacterStatsController statsController, HumanoidStats stats, PlayerInput input,  IHumanoidCombat combatController, IAttackSource attackSource)
    {
        this.statsController = statsController;
        this.stats = stats;
        this.input = input;
  
        this.attackSource = attackSource;
        this.combatController = combatController;

    }
}




