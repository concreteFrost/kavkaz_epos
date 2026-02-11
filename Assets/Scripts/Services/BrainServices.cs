using UnityEngine;

public class EnemyBrainContext
{
    public Vector3 permamentPosition;

    public Transform self;
    public Animator animator;
    public HumanoidAgentController agentController;
    public HumanoidAIMotor motor;
    public HumanoidAIController controller;
    public CharacterStatsController stats;
   
    public IDamagable damageController;
    public IRagdollController ragdollController;

    public BaseHumanoidCombatController combat;
    public HumanoidCombatInventory inventory;
    public EnemyFOVController fov;
    public EnemyStateTracker stateTracker;
    public CharacterInteract interact;

    public InterruptionManager interruptionManager;

    public EnemyNotifierManager notifierManager;

}

