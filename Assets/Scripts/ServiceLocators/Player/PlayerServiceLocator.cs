using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] Animator animator;

    [SerializeField] PlayerController controller;

    [SerializeField] private PlayerMotor motor;
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
        PlayerInputServices inpurService = new PlayerInputServices(controller, characterAnimator, targetLock);
        PlayerControllerServices controllerService = new PlayerControllerServices(animator);
        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        PlayerAnimatorServices animatorServiceProvider = new PlayerAnimatorServices(animator, motor, combatController, statsModifier, targetLock);
        PlayerInteractServices interactionService = new PlayerInteractServices(combatInventory);
        PlayerStatsServices statsService = new PlayerStatsServices(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerStatsModifierServices modifierServiceProvider = new PlayerStatsModifierServices(uID, stats, uIServiceLocator.GetPlayerStatsUI(), input, combatController, combatInventory);
        PlayerCombatInventoryServices combatInventoryService = new PlayerCombatInventoryServices(combatController, statsModifier, uID);
        PlayerStateServices stateServiceProvider = new PlayerStateServices(motor, combatController, statsModifier, stats, interaction);
        PlayerTargetLockServices targetLockServiceProvider = new PlayerTargetLockServices(uIServiceLocator.GetLockOnTargetUI(), controller, statsModifier);

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