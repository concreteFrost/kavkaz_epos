using UnityEngine;

public class EnemyServiceLocator : BaseHumanoidAiServiceLocator
{

    [Header("Система взаимодействия")]
    [SerializeField] private CharacterInteract interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource; 

    [Header("Система прерывания состояний")]
    [SerializeField] private InterruptionManager interruptionManager;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    [SerializeField] protected EnemyBrain brain;
    [SerializeField] protected EnemyStateTracker stateTracker;

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
        //HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator, overrideController, motor, combatController, fovController, damageController, pushReceiver);
        //animatorController.Construct(animatorService);
    }

    private void InteractInit()
    {
        //HumanoidInteractService interactService = new HumanoidInteractService(this.transform, animatorController, combatInventory, damageController, attackSource, motor);
        //interaction.Construct(interactService);
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
        //HumanoidAICombatControllerServices combatControllerServices = new HumanoidAICombatControllerServices(combatInventory, animatorController, damageController);
        //combatController.Init(combatControllerServices);

        //HumanoidCombatInventoryServices combatInventoryServices = new HumanoidCombatInventoryServices(animatorController, combatController, interaction, attackSource, transform, (int)damageController.CharacterType);
        //combatInventory.Construct(combatInventoryServices);

        //AttackSourceServices attackSourceServices = new AttackSourceServices(transform, (int)damageController.CharacterType);
        //attackSource.Construct(attackSourceServices);
    }

    protected override void BrainInit()
    {
        
        EnemyStateTrackerServices stateTrackerServices = new EnemyStateTrackerServices(damageController,statsManager);
        stateTracker.Init(stateTrackerServices);

        EnemyBrainContext brainContext = new EnemyBrainContext()
        {
            permamentPosition = transform.position,
            self = transform,
            animator = animator,
            motor = motor,
            controller = controller,
            stats = statsManager,
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
