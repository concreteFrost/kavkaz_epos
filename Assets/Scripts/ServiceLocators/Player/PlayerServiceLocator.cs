using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{

    #region CORE
    [Header("CORE")]
    [SerializeField] UniqueId uniqueId;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorOverrideController overrideController;

    [SerializeField] PlayerController controller;

    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;
    [SerializeField] private PlayerInput input;

    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private PlayerDamageController damageController;   
    [SerializeField] private HumanoidStats stats;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private CharacterInteract interaction;
    [SerializeField] private HumanoidCombatInventory combatInventory;
   
    [SerializeField] private PlayerTargetLock targetLock;

    [SerializeField] private PlayerAnimatorController characterAnimator = new PlayerAnimatorController();

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

        HumanoidInteractService interactionService = new HumanoidInteractService(combatInventory, damageController);
        interaction.Init(interactionService);

        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, damageController, stats, statsController, interaction, targetLock, climbing, animator);
        controller.Init(stateServiceProvider);

        HumanoidStatsControllerService modifierServiceProvider = new HumanoidStatsControllerService(stats);
        statsController.Init(modifierServiceProvider);

        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, characterAnimator);
        combatController.Init(combatControllerService);

        HumanoidAnimatorService animatorServiceProvider = new HumanoidAnimatorService(animator,overrideController, motor, combatController, targetLock, damageController);
        characterAnimator.Init(animatorServiceProvider);

        PlayerTargetLockService targetLockServiceProvider = new PlayerTargetLockService(lockOnTargetUI, controller, damageController,stats);
        targetLock.Init(targetLockServiceProvider);

        damageController.Init(motor,statsController, stats, combatController, combatInventory, input, uID);

        HumanoidCombatInventoryService combatInventoryService = new HumanoidCombatInventoryService(characterAnimator,combatController,interaction, transform, (int)damageController.CharacterType);
        combatInventory.Init(combatInventoryService);
    }

    void UiInit()
    {
        playerStatsUI.Init(stats);
    }
}