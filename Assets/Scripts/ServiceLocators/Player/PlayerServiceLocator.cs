using UnityEngine;


public class PlayerServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] Animator animator;

    [SerializeField] private PlayerController motor;
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
        PlayerInputServiceProvider inpurService = new PlayerInputServiceProvider(motor, combatController, interaction, characterAnimator, targetLock);
        PlayerControllerServiceProvider controllerService = new PlayerControllerServiceProvider(animator,stats,statsModifier,combatController);
        PlayerCombatControllerServiceProvider combatControllerService = new PlayerCombatControllerServiceProvider(motor, combatInventory, stats, statsModifier);
        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(motor, combatInventory, combatController);
        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerStatsModifierServiceProvider modifierServiceProvider = new PlayerStatsModifierServiceProvider(uID,stats,uIServiceLocator.GetPlayerStatsUI(), input,combatController, combatInventory,motor);
        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(combatController, statsModifier);

        motor.Init(controllerService);
        input.Init(inpurService);
        stats.Init(statsService);
        statsModifier.Init(modifierServiceProvider);

        combatInventory.Init(combatInventoryService);
        combatController.Init(combatControllerService);
        interaction.Init(interactionService);

        characterAnimator.Init(animator, motor, combatController, statsModifier);
        targetLock.Init(motor, uIServiceLocator.GetLockOnTargetUI()); 
       
    }


}
