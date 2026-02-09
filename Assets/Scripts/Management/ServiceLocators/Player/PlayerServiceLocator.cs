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
    [SerializeField] private HumanoidStatsManager statsManager;

    [Header("Система взаимодействия")]
    [SerializeField] private CharacterInteract interaction;

    [Header("Боевая система")]
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;

    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;
    [SerializeField] private PlayerPushReceiver pushReceiver;
    [SerializeField] private PlayerFallController fallController;

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
        AnimatorInit();
        StatsInit();
        MotorInit();
        InputInit();
        InteractionInit();
        CombatInit();
        TargetLockInit();
        ControllerInit();
        DamageInit();
    }

    private void AnimatorInit()
    {
        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(
            animator,
            overrideController,
            motor,
            combatController,
            targetLock,
            damageController,
            pushReceiver
        );

        animatorController.Init(animatorService);
    }

    private void StatsInit()
    {
        statsManager.Init();
    }

    private void MotorInit()
    {
        motor.Init(animatorController);
    }

    private void InputInit()
    {
        PlayerInputService inputService =
            new PlayerInputService(controller, animatorController, targetLock);
        input.Init(inputService);
    }

    private void InteractionInit()
    {
        HumanoidInteractService interactionService =
            new HumanoidInteractService(
                transform,
                animatorController,
                combatInventory,
                damageController,
                attackSource,
                motor
            );

        interaction.Init(interactionService);
    }

    private void CombatInit()
    {
        AttackSourceServices attackSourceServices =
            new AttackSourceServices(transform, (int)damageController.CharacterType);
        attackSource.Init(attackSourceServices);

        BaseHumanoidCombatControllerServices combatControllerServices =
            new BaseHumanoidCombatControllerServices(
                combatInventory,
                animatorController,
                damageController
            );
        combatController.Init(combatControllerServices);

        HumanoidCombatInventoryServices combatInventoryServices =
            new HumanoidCombatInventoryServices(
                animatorController,
                combatController,
                interaction,
                attackSource,
                transform,
                (int)damageController.CharacterType
            );
        combatInventory.Init(combatInventoryServices);

        AgressivePushControllerServices pushControllerServices =
            new AgressivePushControllerServices(
                attackSource,
                combatController,
                animatorController,
                transform
            );
        pushController.Init(pushControllerServices);
    }

    private void TargetLockInit()
    {
        PlayerTargetLockService targetLockService =
            new PlayerTargetLockService(
                lockOnTargetUI,
                controller,
                damageController,
                statsManager
            );

        targetLock.Init(targetLockService);
    }

    private void ControllerInit()
    {
        PlayerControllerService controllerService =
            new PlayerControllerService(
                motor,
                combatController,
                damageController,
                statsManager,
                interaction,
                targetLock,
                pushController,
                climbing,
                animatorController
            );

        controller.Init(controllerService);
    }

    private void DamageInit()
    {
        PlayerDamageControllerService damageControllerService =
            new PlayerDamageControllerService(
                animatorController,
                motor,
                statsManager
            );

        damageController.Init(damageControllerService);
        fallController.Init(motor, damageController);
    }

    private void UiInit()
    {
        playerStatsUI.Init(statsManager.Stats);
    }
}
