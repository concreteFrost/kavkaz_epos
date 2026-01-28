using UnityEngine;
using UnityEngine.AI;

public class EnemyServiceLocator : MonoBehaviour
{

    [Header("Анимация")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;
    [SerializeField] private HumanoidAnimatorIK ik;
    [SerializeField] private HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();

    [Header("Агент")]
    [SerializeField] NavMeshAgent agent;
    HumanoidAgentController agentController;

    [Header("Рагдол")]
    private AiRagdollController ragdollController;

    [Header("Коллайдер")]   
    [SerializeField] private CapsuleCollider capsuleCollider;

    [Header("Мотор")]
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;

    [Header("Статы")]
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private HumanoidStats stats;

    [Header("Система взаимодействия")]
    [SerializeField] private CharacterInteract interaction;

    [Header("Боевая система")]
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;

    [Header("Система урона")]
    [SerializeField] private HumanoidAIDamageController damageController;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyStateTracker stateTracker;

    [Header("Уникальный идентификатор")]
    [SerializeField] UniqueId uniqueId;
    string uid;
 
    private void Awake()
    {
        uid = uniqueId.uniqueId;

        CoreInit();
        BrainInit();    
    }

    public void CoreInit()
    {

        agentController = new HumanoidAgentController(agent, animator);
        ragdollController = new AiRagdollController(this,animator, agentController, GetComponentsInChildren<Rigidbody>(), transform);

        stats.Init();
       
        motor.Init(animator, agentController, ragdollController);
      
        ik.Init(motor, stats,damageController);
        fovController.Init();

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator,overrideController, motor, combatController, fovController, damageController);
        animatorController.Init(animatorService);

        HumanoidInteractService interactService = new HumanoidInteractService(combatInventory, damageController);
        interaction.Init(interactService);

        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        HumanoidControllerService controllerService = new HumanoidControllerService(animator, motor, animatorController,agentController ,statsController,damageController, stats);
        controller.Init(controllerService);

        HumanoidCombatControllerServices combatControllerServices = new HumanoidCombatControllerServices(combatInventory, animatorController);
        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryService combatInventoryServices = new HumanoidCombatInventoryService(animatorController, combatController,interaction, transform, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryServices);

        HumanoidDamageControllerService damageService = new HumanoidDamageControllerService(ragdollController,motor,statsController, stats, agent, capsuleCollider, uid);
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
            stateTracker = stateTracker,
            agentController = agentController,
            ragdollController = ragdollController
        };

        brain.Init(brainContext);
    }
}
