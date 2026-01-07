using UnityEngine;

public class HumanoidAIServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private CharacterFOV targetLock;
    [SerializeField] private HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();
    [SerializeField] private HumanoidAIDamageController damageController;
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private CharacterInteract interaction;
    [SerializeField] private HumanoidAIBrain brain;

    [Header("states")]
    [SerializeField] private EnemyIdleState idle;


    private void Awake()
    {
        string uid = uniqueId.uniqueId;

        stats.Init();
        motor.Init(animator);

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator,motor,combatController,targetLock,damageController);
        animatorController.Init(animatorService);
       
        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        HumanoidControllerService controllerService = new HumanoidControllerService(animator, motor, animatorController, statsController, stats);
        controller.Init(controllerService);

        damageController.Init(statsController, stats,motor.agent,capsuleCollider, uid);

        CharacterTargetLockService targetLockService = new CharacterTargetLockService(controller, damageController, stats);
        targetLock.Init();

        HumanoidCombatControllerServices combatControllerServices = new HumanoidCombatControllerServices(combatInventory, animator);

        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryService combatInventoryServices = new HumanoidCombatInventoryService(combatController, statsController, (int)damageController.CharacterType); 
        combatInventory.Init(combatInventoryServices);

        HumanoidInteractService interactService = new HumanoidInteractService(combatInventory);
        interaction.Init(interactService);

        HumanoidAIContext context = new HumanoidAIContext(transform,motor,controller,combatController,combatInventory,targetLock,interaction,null);
        brain.InitContext(context);
        brain.InitBehaviours(idle);

    }
}
