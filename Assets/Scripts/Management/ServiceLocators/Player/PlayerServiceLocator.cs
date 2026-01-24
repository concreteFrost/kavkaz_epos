using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{
    [Header("Уникальный идентификатор")]
    [SerializeField] private UniqueId uniqueId;
    private string uid;

    [Header("Анимация")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;
    [SerializeField] private PlayerAnimatorController characterAnimator = new PlayerAnimatorController();

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
    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;

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
        PlayerInputService inputService = new PlayerInputService(controller, characterAnimator, targetLock);
        input.Init(inputService);

        // Interaction
        HumanoidInteractService interactionService = new HumanoidInteractService(combatInventory, damageController);
        interaction.Init(interactionService);

        // Stats
        HumanoidStatsControllerService statsService = new HumanoidStatsControllerService(stats);
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
        characterAnimator.Init(animatorService);

        // Combat
        HumanoidCombatControllerServices combatControllerService =
            new HumanoidCombatControllerServices(combatInventory, characterAnimator);
        combatController.Init(combatControllerService);

        HumanoidCombatInventoryService combatInventoryService =
            new HumanoidCombatInventoryService(
                characterAnimator,
                combatController,
                interaction,
                transform,
                (int)damageController.CharacterType
            );
        combatInventory.Init(combatInventoryService);

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
                climbing,
                animator
            );
        controller.Init(controllerService);

        // Damage

        PlayerDamageControllerService damageControllerService = new PlayerDamageControllerService(motor,statsController , stats, input, combatController, combatInventory,uid);
        damageController.Init(damageControllerService);
    }

    private void UiInit()
    {
        playerStatsUI.Init(stats);
    }
}
