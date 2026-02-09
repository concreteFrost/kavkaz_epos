using UnityEngine;
using UnityEngine.AI;

public class HumanoidDamageServices
{
    public Transform self;
    public BaseHumanoidAnimatorController animatorController; 
    public HumanoidAIMotor motor;
    public HumanoidAgentController agent;

    public HumanoidStatsManager statsManager;
    public IRagdollController ragdollController;

    public string uniqueID;

    public HumanoidDamageServices(Transform self, BaseHumanoidAnimatorController animatorController,IRagdollController ragdollController, HumanoidAIMotor motor,HumanoidStatsManager statsManager, string uniqueID)
    {
        this.self = self;   
        this.animatorController = animatorController;
        this.ragdollController = ragdollController;
        this.motor = motor;
        this.statsManager = statsManager;
        this.uniqueID = uniqueID;

    }
}

public class PlayerDamageControllerService
{
    public BaseHumanoidAnimatorController animatorController;
    public IHumanoidMovement motor;
    public HumanoidStatsManager statsManager;

    public PlayerInput input;


    public PlayerDamageControllerService(BaseHumanoidAnimatorController animatorController, IHumanoidMovement motor, HumanoidStatsManager statsManager)
    {
        this.motor = motor;
        this.statsManager = statsManager;
        this.animatorController = animatorController;
    }
}

