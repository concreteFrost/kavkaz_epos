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
    [SerializeField] private PlayerStatsController statsModifier;
    [SerializeField] private PlayerInteract interaction;
    [SerializeField] private PlayerCombatInventory combatInventory;
   
    [SerializeField] private PlayerTargetLock targetLock;

    [SerializeField] private PlayerUIServiceLocator uIServiceLocator;

    [SerializeField] private PlayerAnimator characterAnimator = new PlayerAnimator();

    private void Awake()
    {
        
        var uID = uniqueId.uniqueId;
        
        PlayerInputServiceProvider inpurService = new PlayerInputServiceProvider(controller, characterAnimator, targetLock);
        input.Init(inpurService);

        PlayerControllerService stateServiceProvider = new PlayerControllerService(motor, combatController, statsModifier, stats, interaction, climbing, animator);
        controller.Init(stateServiceProvider);

        HumanoidMotorServices controllerService = new HumanoidMotorServices(animator);
        motor.Init(controllerService);

        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        stats.Init(statsService);

        PlayerStatsModifierServiceProvider modifierServiceProvider = new PlayerStatsModifierServiceProvider(uID, stats, uIServiceLocator.GetPlayerStatsUI(), input, combatController, combatInventory);
        statsModifier.Init(modifierServiceProvider);

        HumanoidCombatControllerServices combatControllerService = new HumanoidCombatControllerServices(combatInventory, animator);
        combatController.Init(combatControllerService);

        PlayerAnimatorServiceProvider animatorServiceProvider = new PlayerAnimatorServiceProvider(animator, motor, combatController, statsModifier, targetLock);
        characterAnimator.Init(animatorServiceProvider);

        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(combatInventory);
        interaction.Init(interactionService);

        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(combatController, statsModifier, uID);
        combatInventory.Init(combatInventoryService);

        PlayerTargetLockServiceProvider targetLockServiceProvider = new PlayerTargetLockServiceProvider(uIServiceLocator.GetLockOnTargetUI(), controller, statsModifier);
        targetLock.Init(targetLockServiceProvider);

    }

}