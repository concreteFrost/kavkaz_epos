using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] Animator animator;

    [SerializeField] PlayerController controller;

    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;
    [SerializeField] private PlayerInput input;

    [SerializeField] private HumanoidCombatController combatController;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerStatsModifier statsModifier;
    [SerializeField] private PlayerInteract interaction;
    [SerializeField] private PlayerCombatInventory combatInventory;
    [SerializeField] private PlayerAnimator characterAnimator;
    [SerializeField] private PlayerTargetLock targetLock;

    [SerializeField] private PlayerUIServiceLocator uIServiceLocator;

    private void Awake()
    {
        var uID = uniqueId.uniqueId;
        PlayerInputServiceProvider inpurService = new PlayerInputServiceProvider(controller, characterAnimator, targetLock);
        HumanoidMotorServices controllerService = new HumanoidMotorServices(animator);
        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        PlayerAnimatorServiceProvider animatorServiceProvider = new PlayerAnimatorServiceProvider(animator, motor, combatController, statsModifier, targetLock, climbing);
        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(combatInventory);
        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerStatsModifierServiceProvider modifierServiceProvider = new PlayerStatsModifierServiceProvider(uID, stats, uIServiceLocator.GetPlayerStatsUI(), input, combatController, combatInventory);
        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(combatController, statsModifier, uID);
        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, statsModifier, stats, interaction, climbing);
        PlayerTargetLockServiceProvider targetLockServiceProvider = new PlayerTargetLockServiceProvider(uIServiceLocator.GetLockOnTargetUI(), controller, statsModifier);

        controller.Init(stateServiceProvider);
        motor.Init(controllerService);
        input.Init(inpurService);
        stats.Init(statsService);
        statsModifier.Init(modifierServiceProvider);

        combatInventory.Init(combatInventoryService);
        combatController.Init(combatControllerService);
        interaction.Init(interactionService);

        characterAnimator.Init(animatorServiceProvider);
        targetLock.Init(targetLockServiceProvider);

    }

}