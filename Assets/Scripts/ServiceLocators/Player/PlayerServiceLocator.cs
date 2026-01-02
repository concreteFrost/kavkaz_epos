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
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerStatsController statsController;
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

        PlayerInputService inpurService = new PlayerInputService(controller, characterAnimator, targetLock);
        input.Init(inpurService);

        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, damageController, stats, statsController, interaction, climbing, animator);
        controller.Init(stateServiceProvider);

        HumanoidMotorServices controllerService = new HumanoidMotorServices(animator);
        motor.Init(controllerService);

        PlayerStatsService statsService = new PlayerStatsService(combatInventory, motor, input);
        stats.Init(statsService);

        PlayerStatsControllerService modifierServiceProvider = new PlayerStatsControllerService(stats);
        statsController.Init(modifierServiceProvider);

        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        combatController.Init(combatControllerService);

        PlayerAnimatorService animatorServiceProvider = new PlayerAnimatorService(animator, motor, combatController, targetLock, damageController);
        characterAnimator.Init(animatorServiceProvider);

        PlayerInteractService interactionService = new PlayerInteractService(combatInventory);
        interaction.Init(interactionService);

        PlayerCombatInventoryService combatInventoryService = new PlayerCombatInventoryService(combatController, statsController, uID);
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