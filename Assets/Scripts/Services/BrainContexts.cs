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

    public IHumanoidMeleeCombat combat;
    public HumanoidCombatInventory inventory;

    public CharacterEmitter emitter;
    public CharacterSpellInventory spellInventory;

    public EnemyFOVController fov;
    public EnemyStateTracker stateTracker;
    public ItemCollector interact;

    public InterruptionManager interruptionManager;

    public EnemyNotifierManager notifierManager;

}
