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
        PlayerControllerServiceProvider controllerService = new PlayerControllerServiceProvider(animator,stats,statsModifier);
        PlayerCombatControllerServiceProvider combatControllerService = new PlayerCombatControllerServiceProvider(motor, combatInventory, stats);
        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(motor, combatInventory);
        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerStatsModifierServiceProvider modifierServiceProvider = new PlayerStatsModifierServiceProvider(uID,stats,uIServiceLocator.GetPlayerStatsUI(), input,combatInventory,motor);
        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(motor, statsModifier);

        characterAnimator.Init(animator, motor);
        input.Init(inpurService);
        stats.Init(statsService);
        statsModifier.Init(modifierServiceProvider);
        motor.Init(controllerService);

        combatInventory.Init(combatInventoryService);
        combatController.Init(combatControllerService);
        interaction.Init(interactionService);

        targetLock.Init(motor, uIServiceLocator.GetLockOnTargetUI()); 
       
    }


}
