using UnityEngine;
using Zenject;

public class HumanoidControllerServices
{
    public Transform self;
    public HumanoidAIMotor aiMotor;
    public HumanoidAIAnimatorController aiAnimatorController;
    public HumanoidAgentController agentController;
    public HumanoidAIDamageController damageController;

    public CharacterStatsController statsManager;


    [Inject]
    public HumanoidControllerServices(Transform self, HumanoidAIMotor aIMotor, HumanoidAIAnimatorController aIAnimator, HumanoidAgentController agentController,HumanoidAIDamageController damageController, CharacterStatsController statsManager)
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
  
}


