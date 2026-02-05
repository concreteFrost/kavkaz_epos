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
    [SerializeField] private AttackSource attackSource; 

    [Header("Система урона")]
    [SerializeField] private HumanoidAIDamageController damageController;
    [SerializeField] private HumanoidAIPushReceiver pushReceiver;

    [Header("Система прерывания состояний")]
    [SerializeField] private InterruptionManager interruptionManager;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyStateTracker stateTracker;

    [Header("Система событий")]
    [SerializeField] private EnemyNotifierManager notifierManager;

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
        ragdollController = new AiRagdollController(this,animator, agentController, transform);

        stats.Init();
       
        motor.Init(animator, agentController);
      
        ik.Init(motor, stats,damageController);
        
        fovController.Init();

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator,overrideController, motor, combatController, fovController, damageController,pushReceiver);
        animatorController.Init(animatorService);

        HumanoidInteractService interactService = new HumanoidInteractService(this.transform,animatorController,combatInventory, damageController, attackSource);
        interaction.Init(interactService);

        HumanoidStatsControllerServices service = new HumanoidStatsControllerServices(stats);
        statsController.Init(service);

        HumanoidControllerServices controllerService = new HumanoidControllerServices(animator, motor, animatorController,agentController ,statsController,damageController, stats);
        controller.Init(controllerService);

        HumanoidAICombatControllerServices combatControllerServices = new HumanoidAICombatControllerServices(combatInventory, animatorController, damageController);
        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryServices combatInventoryServices = new HumanoidCombatInventoryServices(animatorController, combatController,interaction,attackSource ,transform, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryServices);

        AttackSourceServices attackSourceServices = new AttackSourceServices(transform, (int)damageController.CharacterType);
        attackSource.Init(attackSourceServices);

        HumanoidDamageServices damageService = new HumanoidDamageServices(ragdollController,motor,statsController, stats, agentController, capsuleCollider, uid);
        damageController.Init(damageService);

        EnemyInterruptionServices interruptionServices = new EnemyInterruptionServices(damageController, pushReceiver);
        interruptionManager.Init(interruptionServices);

        EnemyNotifierServices notifierServices = new EnemyNotifierServices(transform,fovController);
        notifierManager.Init(notifierServices);

        HumanoidPushServices pushServices = new HumanoidPushServices(transform, motor, animatorController, damageController, ragdollController);
        pushReceiver.Init(pushServices);

    }

    public void BrainInit()
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
