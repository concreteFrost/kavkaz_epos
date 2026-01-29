using UnityEngine;

public class HumanoidControllerServices
{
    public Animator animator;

    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
    public HumanoidAgentController agentController;

    public IDamagable damageController;

    public ICharacterStatsController statsController;
    public HumanoidStats stats;

    public HumanoidControllerServices(Animator animator, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, HumanoidAgentController agentController, ICharacterStatsController statsController, IDamagable damageController, HumanoidStats stats)
    {

        this.animator = animator;
        this.aiMotor = aIMotor;
        this.aiAnimatorController = aIAnimator;
        this.agentController = agentController;
        this.statsController = statsController;
        this.damageController = damageController;
        this.stats = stats;
    }
}

public class PlayerControllerService
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


