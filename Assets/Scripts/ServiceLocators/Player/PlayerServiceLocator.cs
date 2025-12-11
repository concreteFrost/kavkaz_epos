using UnityEngine;


public class PlayerServiceLocator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] private PlayerController motor;
    [SerializeField] private PlayerInput input;
    [SerializeField] private vThirdPersonCamera thirdPersonCamera;
    
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerInteract interaction;
    [SerializeField] private PlayerCombatInventory combatInventory;
    [SerializeField] private CharacterAnimator characterAnimator;

    [SerializeField] private PlayerUIServiceLocator uIServiceLocator;

    private void Awake()
    {
        PlayerInputServiceProvider inpurService = new PlayerInputServiceProvider(motor, combatController, thirdPersonCamera, interaction, characterAnimator);
        CharacterAnimatorServiceProvider animatorService = new CharacterAnimatorServiceProvider(animator);
        PlayerControllerServiceProvider controllerService = new PlayerControllerServiceProvider(animator, stats);
        PlayerCombatControllerServiceProvider combatControllerService = new PlayerCombatControllerServiceProvider(motor, combatInventory, stats);
        PlayerInteractServiceProvider interactionService = new PlayerInteractServiceProvider(motor, combatInventory);
        PlayerStatsServiceProvider statsService = new PlayerStatsServiceProvider(combatInventory, motor, uIServiceLocator.GetPlayerStatsUI(), input);
        PlayerCombatInventoryServiceProvider combatInventoryService = new PlayerCombatInventoryServiceProvider(motor, stats);

        characterAnimator.Init(animatorService);
        input.Init(inpurService);
        stats.Init(statsService);
        motor.Init(controllerService);

        combatInventory.Init(combatInventoryService);
        combatController.Init(combatControllerService);
        interaction.Init(interactionService);
       
    }

    private void Start()
    {
        thirdPersonCamera.SetMainTarget(this.transform);
    }

}
