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
    [SerializeField] private CharacterInteract interaction;
    [SerializeField] private HumanoidCombatInventory combatInventory;
   
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
        motor.Init(animator);

        PlayerInputService inpurService = new PlayerInputService(controller, characterAnimator, targetLock);
        input.Init(inpurService);

        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, damageController, stats, statsController, interaction, climbing, animator);
        controller.Init(stateServiceProvider);

        HumanoidStatsControllerService modifierServiceProvider = new HumanoidStatsControllerService(stats);
        statsController.Init(modifierServiceProvider);

        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        combatController.Init(combatControllerService);

        HumanoidAnimatorService animatorServiceProvider = new HumanoidAnimatorService(animator, motor, combatController, targetLock, damageController);
        characterAnimator.Init(animatorServiceProvider);

        HumanoidInteractService interactionService = new HumanoidInteractService(combatInventory);
        interaction.Init(interactionService);


        PlayerTargetLockService targetLockServiceProvider = new PlayerTargetLockService(lockOnTargetUI, controller, damageController,stats);
        targetLock.Init(targetLockServiceProvider);

        damageController.Init(statsController, stats, combatController, combatInventory, input, uID);

        HumanoidCombatInventoryService combatInventoryService = new HumanoidCombatInventoryService(combatController, statsController, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryService);
    }

    void UiInit()
    {
        playerStatsUI.Init(stats);
    }
}