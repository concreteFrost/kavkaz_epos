using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{

    #region CORE
    [Header("CORE")]
    [SerializeField] UniqueId uniqueId;
    [SerializeField] Animator animator;

    [SerializeField] PlayerController controller;

    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;
    [SerializeField] private PlayerInput input;

    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private PlayerDamageController damageController;   
    [SerializeField] private CharacterStats stats;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private PlayerInteract interaction;
    [SerializeField] private PlayerCombatInventory combatInventory;
   
    [SerializeField] private PlayerTargetLock targetLock;

    [SerializeField] private PlayerAnimator characterAnimator = new PlayerAnimator();

    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;


    #endregion

    private void Awake()
    {
        
        CoreInit();
        UiInit();
    }

    void CoreInit()
    {
        var uID = uniqueId.uniqueId;

        stats.Init();

        PlayerInputService inpurService = new PlayerInputService(controller, characterAnimator, targetLock);
        input.Init(inpurService);

        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, damageController, stats, statsController, interaction, climbing, animator);
        controller.Init(stateServiceProvider);

        HumanoidMotorServices controllerService = new HumanoidMotorServices(animator);
        motor.Init(controllerService);

        HumanoidStatsControllerService modifierServiceProvider = new HumanoidStatsControllerService(stats);
        statsController.Init(modifierServiceProvider);

        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        combatController.Init(combatControllerService);

        PlayerAnimatorService animatorServiceProvider = new PlayerAnimatorService(animator, motor, combatController, targetLock, damageController);
        characterAnimator.Init(animatorServiceProvider);

        HumanoidInteractService interactionService = new HumanoidInteractService(combatInventory);
        interaction.Init(interactionService);

        HumanoidCombatInventoryService combatInventoryService = new HumanoidCombatInventoryService(combatController, statsController, uID);
        combatInventory.Init(combatInventoryService);

        PlayerTargetLockService targetLockServiceProvider = new PlayerTargetLockService(lockOnTargetUI, controller, damageController);
        targetLock.Init(targetLockServiceProvider);

        damageController.Init(statsController, stats, combatController, combatInventory, input, uID);
    }

    void UiInit()
    {
        playerStatsUI.Init(stats);
    }
}