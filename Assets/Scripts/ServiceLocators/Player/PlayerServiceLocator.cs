using UnityEngine;


public class PlayerServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] Animator animator;

    [SerializeField] PlayerController playerState;

    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerInput input;
    
    [SerializeField] private PlayerCombatController combatController;
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
        PlayerInputServiceProvider inpurService = new PlayerInputServiceProvider(playerState, characterAnimator);
        PlayerControllerServiceProvider controllerService = new PlayerControllerServiceProvider(animator);
        PlayerCombatControllerServiceProvider combatControllerService = new PlayerCombatControllerServiceProvider(combatInventory);
        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(combatInventory);
        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerStatsModifierServiceProvider modifierServiceProvider = new PlayerStatsModifierServiceProvider(uID,stats,uIServiceLocator.GetPlayerStatsUI(), input,combatController, combatInventory,motor);
        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(combatController, statsModifier);
        PlayerStateServiceProvider stateServiceProvider = new PlayerStateServiceProvider(motor, combatController, statsModifier, stats, targetLock, interaction);

        playerState.Init(stateServiceProvider); 
        motor.Init(controllerService);
        input.Init(inpurService);
        stats.Init(statsService);
        statsModifier.Init(modifierServiceProvider);

        combatInventory.Init(combatInventoryService);
        combatController.Init(combatControllerService);
        interaction.Init(interactionService);

        characterAnimator.Init(animator, motor, combatController, statsModifier);
        targetLock.Init(uIServiceLocator.GetLockOnTargetUI(), transform); 
       
    }


}
