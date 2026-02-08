using UnityEngine;
using UnityEngine.AI;

public class HumanoidDamageServices
{
    public BaseHumanoidAnimatorController animatorController; 
    public HumanoidAIMotor motor;
    public HumanoidStats stats;
    public HumanoidAgentController agent;
    public CapsuleCollider col;

    public ICharacterStatsController statsModifier;
    public IRagdollController ragdollController;

    public string uniqueID;

    public HumanoidDamageServices(BaseHumanoidAnimatorController animatorController,IRagdollController ragdollController, HumanoidAIMotor motor, ICharacterStatsController statsController, HumanoidStats stats, HumanoidAgentController agent, CapsuleCollider col, string uniqueID)
    {

        this.animatorController = animatorController;
        this.ragdollController = ragdollController;
        this.motor = motor;
        this.statsModifier = statsController;
        this.stats = stats;
        this.agent = agent;
        this.col = col;
        this.uniqueID = uniqueID;

    }
}

public class PlayerDamageControllerService
{
    public BaseHumanoidAnimatorController animatorController;
    public IHumanoidMovement motor;
    public ICharacterStatsController statsController;
    public HumanoidStats stats;
    public PlayerInput input;

    public IHumanoidCombat combatController;
    public ICombatInventory attackSource;

    public PlayerDamageControllerService(BaseHumanoidAnimatorController animatorController, IHumanoidMovement motor, ICharacterStatsController statsController, HumanoidStats stats, PlayerInput input, IHumanoidCombat combatController, ICombatInventory attackSource)
    {
        this.motor = motor;
        this.statsController = statsController;
        this.stats = stats;
        this.input = input;

        this.attackSource = attackSource;
        this.combatController = combatController;
        this.animatorController = animatorController;
    }
}

