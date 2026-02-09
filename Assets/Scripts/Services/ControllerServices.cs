using UnityEngine;

public class HumanoidControllerServices
{
    public Transform self;
    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
    public HumanoidAgentController agentController;
    public HumanoidAIDamageController damageController;

    public HumanoidStatsManager statsManager;



    public HumanoidControllerServices(Transform self, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, HumanoidAgentController agentController,HumanoidAIDamageController damageController, HumanoidStatsManager statsManager)
    {

        this.self = self;
        this.aiMotor = aIMotor;
        this.aiAnimatorController = aIAnimator;
        this.agentController = agentController;  
        this.damageController = damageController;
        this.statsManager = statsManager;

    }
}

public class PlayerControllerService
{
    public PlayerMotor controller;
    public PlayerAnimatorController animatorController;

    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public HumanoidStatsManager statsManager;
    public ICollector interact;
    public ITargetLocker locker;
    public AgressivePushController pushSource;
    public PlayerClimbing climbing;

    public PlayerControllerService(
        PlayerMotor controller,
        IHumanoidCombat combatController,
        IDamagable damageController,

        HumanoidStatsManager statsManager,
        ICollector interact,
        ITargetLocker locker,
        AgressivePushController pushSource,
        PlayerClimbing climbing,
        PlayerAnimatorController animatorController)
    {
        this.controller = controller;
        this.combatController = combatController;
        this.damageController = damageController;

        this.interact = interact;
        this.climbing = climbing;
        this.animatorController = animatorController;
        this.locker = locker;
        this.statsManager = statsManager;
        this.pushSource = pushSource;   
    }
}


