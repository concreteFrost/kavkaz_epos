using UnityEngine;

public class EnemyServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] private Animator animator;
  
    [SerializeField] private CapsuleCollider capsuleCollider;

    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private CharacterInteract interaction;

    [SerializeField] private HumanoidAnimatorIK ik;
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;
    
    [SerializeField] private HumanoidStats stats;
    [SerializeField] private HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();
    [SerializeField] private HumanoidAIDamageController damageController;
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
  

    [SerializeField] private EnemyFOVController fovController;
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyStateTracker stateTracker;

    string uid;
 

    private void Awake()
    {
        uid = uniqueId.uniqueId;

        CoreInit();
        BrainInit();    
    }

    public void CoreInit()
    {
        stats.Init();
        motor.Init(animator);

        ik.Init(motor, stats);
        fovController.Init();

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator, motor, combatController, fovController, damageController);
        animatorController.Init(animatorService);

        HumanoidInteractService interactService = new HumanoidInteractService(combatInventory, damageController);
        interaction.Init(interactService);

        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        HumanoidControllerService controllerService = new HumanoidControllerService(animator, motor, animatorController, statsController, stats);
        controller.Init(controllerService);

        HumanoidCombatControllerServices combatControllerServices = new HumanoidCombatControllerServices(combatInventory, animatorController);
        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryService combatInventoryServices = new HumanoidCombatInventoryService(animatorController, combatController,interaction, transform, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryServices);

        HumanoidDamageControllerService damageService = new HumanoidDamageControllerService(motor,statsController, stats, motor.agent, capsuleCollider, uid);
        damageController.Init(damageService);
    }

    public void BrainInit()
    {
        stateTracker.Init(damageController, stats);

        EnemyBrainContext brainContext = new EnemyBrainContext()
        {
            permamentPosition = transform.position,
            self = transform,
            motor = motor,
            controller = controller,
            stats = stats,
            damageController = damageController,
            combat = combatController,
            inventory = combatInventory,
            fov = fovController,
            interact = interaction,
            stateTracker = stateTracker
        };

        brain.Init(brainContext);
    }
}
