using UnityEngine;
using UnityEngine.AI;

public class HumanoidDamageServices
{
    public IRagdollController ragdollController;
    public HumanoidAIMotor motor;
    public ICharacterStatsController statsModifier;
    public HumanoidStats stats;
    public NavMeshAgent agent;
    public CapsuleCollider col;

    public string uniqueID;

    public HumanoidDamageServices(IRagdollController ragdollController, HumanoidAIMotor motor, ICharacterStatsController statsController, HumanoidStats stats, NavMeshAgent agent, CapsuleCollider col, string uniqueID)
    {
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
    public IHumanoidMovement motor;
    public ICharacterStatsController statsController;
    public HumanoidStats stats;
    public PlayerInput input;

    public IHumanoidCombat combatController;
    public ICombatInventory attackSource;

    public string uid;

    public PlayerDamageControllerService(IHumanoidMovement motor, ICharacterStatsController statsController, HumanoidStats stats, PlayerInput input, IHumanoidCombat combatController, ICombatInventory attackSource, string uid)
    {
        this.motor = motor;
        this.statsController = statsController;
        this.stats = stats;
        this.input = input;

        this.attackSource = attackSource;
        this.combatController = combatController;

        this.uid = uid;

    }
}

