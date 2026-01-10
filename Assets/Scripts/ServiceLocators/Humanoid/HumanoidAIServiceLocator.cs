using UnityEngine;

public class HumanoidAIServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] private Animator animator;
    [SerializeField] private HumanoidAnimatorIK ik;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private EnemyFOVController fovController;
    [SerializeField] private HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();
    [SerializeField] private HumanoidAIDamageController damageController;
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private CharacterInteract interaction;
    [SerializeField] private EnemyBrain brain;


    private void Awake()
    {
        string uid = uniqueId.uniqueId;

        stats.Init();
        motor.Init(animator);

        ik.Init(motor, stats);

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator,motor,combatController,fovController,damageController);
        animatorController.Init(animatorService);
       
        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        HumanoidControllerService controllerService = new HumanoidControllerService(animator, motor, animatorController, statsController, stats);
        controller.Init(controllerService);

        HumanoidCombatControllerServices combatControllerServices = new HumanoidCombatControllerServices(combatInventory, animator);

        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryService combatInventoryServices = new HumanoidCombatInventoryService(combatController, statsController, (int)damageController.CharacterType); 
        combatInventory.Init(combatInventoryServices);

        HumanoidInteractService interactService = new HumanoidInteractService(combatInventory);
        interaction.Init(interactService);

        damageController.Init(statsController, stats, motor.agent, capsuleCollider, uid);

        fovController.Init(motor);

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
            interact = interaction
        };

        brain.Init(brainContext);
 

    }
}
