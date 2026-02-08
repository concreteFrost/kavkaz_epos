using UnityEngine;

public class HumanoidControllerServices
{
    public Transform self;
    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
    public HumanoidAgentController agentController;

    public IDamagable damageController;

    public ICharacterStatsController statsController;
    public HumanoidStats stats;

    public HumanoidControllerServices(Transform self, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, HumanoidAgentController agentController, ICharacterStatsController statsController, IDamagable damageController, HumanoidStats stats)
    {

        this.self = self;
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
    public PlayerAnimatorController animatorController;

    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ICharacterStatsController statsController;
    public ICollector interact;
    public ITargetLocker locker;
    public AgressivePushController pushSource;
    public PlayerClimbing climbing;

    public PlayerControllerService(
        PlayerMotor controller,
        IHumanoidCombat combatController,
        IDamagable damageController,
        HumanoidStats stats,
        ICharacterStatsController statsModifier,
        ICollector interact,
        ITargetLocker locker,
        AgressivePushController pushSource,
        PlayerClimbing climbing,
        PlayerAnimatorController animatorController)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.damageController = damageController;
        this.stats = stats;
        this.interact = interact;
        this.climbing = climbing;
        this.animatorController = animatorController;
        this.locker = locker;
        this.statsController = statsModifier;
        this.pushSource = pushSource;   
    }
}


