using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{
    [Header("Уникальный идентификатор")]
    [SerializeField] private UniqueId uniqueId;
    private string uid;

    [Header("Анимация")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;
    [SerializeField] private PlayerAnimatorController animatorController = new PlayerAnimatorController();

    [Header("Мотор и перемещение")]
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;

    [Header("Ввод")]
    [SerializeField] private PlayerInput input;

    [Header("Контроллер")]
    [SerializeField] private PlayerController controller;


    [Header("Статы")]
    [SerializeField] private HumanoidStats stats;
    [SerializeField] private CharacterStatsController statsController;

    [Header("Система взаимодействия")]
    [SerializeField] private CharacterInteract interaction;

    [Header("Боевая система")]
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;    

    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;

    [Header("Система прицеливания")]
    [SerializeField] private PlayerTargetLock targetLock;

    [Header("UI")]
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;

    private void Awake()
    {
        uid = uniqueId.uniqueId;

        CoreInit();
        UiInit();
    }

    private void CoreInit()
    {
        stats.Init();
        motor.Init(animator);
      
        // Input
        PlayerInputService inputService = new PlayerInputService(controller, animatorController, targetLock);
        input.Init(inputService);

        // Interaction
        HumanoidInteractService interactionService = new HumanoidInteractService(this.transform, animatorController ,combatInventory, damageController,attackSource);
        interaction.Init(interactionService);

        // Stats
        HumanoidStatsControllerServices statsService = new HumanoidStatsControllerServices(stats);
        statsController.Init(statsService);

        // Animator
        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(
            animator,
            overrideController,
            motor,
            combatController,
            targetLock,
            damageController
        );
        animatorController.Init(animatorService);

        // Combat
        AttackSourceServices attackSourceServices = new AttackSourceServices(transform, (int)damageController.CharacterType);
        attackSource.Init(attackSourceServices);

        BaseHumanoidCombatControllerServices combatControllerService =
            new BaseHumanoidCombatControllerServices(combatInventory, animatorController,damageController);
        combatController.Init(combatControllerService);

        HumanoidCombatInventoryServices combatInventoryService =
            new HumanoidCombatInventoryServices(
                animatorController,
                combatController,
                interaction,
                attackSource,
                transform,
                (int)damageController.CharacterType
            );
        combatInventory.Init(combatInventoryService);

        pushController.Init(attackSource, combatController, animatorController,transform);

        // Target lock
        PlayerTargetLockService targetLockService =
            new PlayerTargetLockService(lockOnTargetUI, controller, damageController, stats);
        targetLock.Init(targetLockService);

        // Controller (brain/state machine игрока)
        PlayerControllerService controllerService =
            new PlayerControllerService(
                motor,
                combatController,
                damageController,
                stats,
                statsController,
                interaction,
                targetLock,
                pushController,
                climbing,
                animator
            );
        controller.Init(controllerService);

        // Damage

        PlayerDamageControllerService damageControllerService = new PlayerDamageControllerService(animatorController, motor,statsController , stats, input, combatController, combatInventory,uid);
        damageController.Init(damageControllerService);
    }

    private void UiInit()
    {
        playerStatsUI.Init(stats);
    }
}
