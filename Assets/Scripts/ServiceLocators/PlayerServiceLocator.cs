using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] private PlayerController motor;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private vThirdPersonCamera thirdPersonCamera;
    
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerInteract interaction;
    [SerializeField] private PlayerCombatInventory combatInventory;
    [SerializeField] private CharacterAnimator characterAnimator;  
    [SerializeField] private PlayerDamageManager damageManager;

    private void Awake()
    {

        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;

        characterAnimator.Init(animator);
        playerInput.Init(motor, combatController, thirdPersonCamera,interaction,characterAnimator );
        motor.Init(animator,stats);

        combatInventory.Init(motor);
        combatController.Init(motor,combatInventory);

        interaction.Init(motor, combatInventory);
       
    }

    private void Start()
    {
        thirdPersonCamera.SetMainTarget(this.transform);
    }

}
