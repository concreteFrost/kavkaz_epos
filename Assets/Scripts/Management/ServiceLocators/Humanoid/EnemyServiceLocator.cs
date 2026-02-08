using UnityEngine;


public class EnemyServiceLocator : BaseHumanoidAiServiceLocator
{

    [Header("Система взаимодействия")]
    [SerializeField] private CharacterInteract interaction;

    [Header("Боевая система")]
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource; 

    [Header("Система прерывания состояний")]
    [SerializeField] private InterruptionManager interruptionManager;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyStateTracker stateTracker;

    [Header("Система событий")]
    [SerializeField] private EnemyNotifierManager notifierManager;


    protected override void CoreInit()
    {
        base.CoreInit();
        InteractInit();
        InterruptInit();
        CombatInit();   
        fovController.Init();

    }

    protected override void AnimatorInit()
    {
        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator, overrideController, motor, combatController, fovController, damageController, pushReceiver);
        animatorController.Init(animatorService);
    }

    private void InteractInit()
    {
        HumanoidInteractService interactService = new HumanoidInteractService(this.transform, animatorController, combatInventory, damageController, attackSource, motor);
        interaction.Init(interactService);
    }

    private void InterruptInit()
    {
        EnemyInterruptionServices interruptionServices = new EnemyInterruptionServices(damageController, pushReceiver);
        interruptionManager.Init(interruptionServices);

        EnemyNotifierServices notifierServices = new EnemyNotifierServices(transform, fovController);
        notifierManager.Init(notifierServices);
    }

    private void CombatInit()
    {
        HumanoidAICombatControllerServices combatControllerServices = new HumanoidAICombatControllerServices(combatInventory, animatorController, damageController);
        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryServices combatInventoryServices = new HumanoidCombatInventoryServices(animatorController, combatController, interaction, attackSource, transform, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryServices);

        AttackSourceServices attackSourceServices = new AttackSourceServices(transform, (int)damageController.CharacterType);
        attackSource.Init(attackSourceServices);
    }

    protected override void BrainInit()
    {
        
        EnemyStateTrackerServices stateTrackerServices = new EnemyStateTrackerServices(damageController, stats);
        stateTracker.Init(stateTrackerServices);

        EnemyBrainContext brainContext = new EnemyBrainContext()
        {
            permamentPosition = transform.position,
            self = transform,
            animator = animator,
            motor = motor,
            controller = controller,
            stats = stats,
            damageController = damageController,
            combat = combatController,
            inventory = combatInventory,
            fov = fovController,
            interact = interaction,
            stateTracker = stateTracker,
            agentController = agentController,
            ragdollController = ragdollController,
            interruptionManager = interruptionManager,
            notifierManager = notifierManager

        };

        brain.Init(brainContext);
    }
}
