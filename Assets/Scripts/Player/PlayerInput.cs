using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public PlayerController characterController;
    [HideInInspector] public PlayerCombatController playerCombatController;
    [HideInInspector] public vThirdPersonCamera tpCamera;
    [HideInInspector] public CharacterAnimator characterAnimator;
    [HideInInspector] public Camera cameraMain;

    private PlayerInteract interactions;
    private PlayerControls controls;

    private Vector2 moveInput;   // Move
    private Vector2 lookInput;   // Mouse/Gamepad look
    private bool sprintHeld;
    private bool jumpPressed;

    protected virtual void Awake()
    {
        controls = new PlayerControls();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Look
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Jump
        controls.Player.Jump.performed += ctx => jumpPressed = true;

        // Sprint
        controls.Player.Sprint.performed += ctx => sprintHeld = true;
        controls.Player.Sprint.canceled += ctx => sprintHeld = false;

        // Combat
        controls.Player.Attack.performed += ctx => playerCombatController.PerformAttack();
        controls.Player.ThrowItem.performed += ctx => playerCombatController.ThrowWeapon();

        // Interaction
        controls.Player.Interaction.performed += ctx => interactions.Interact(); 

    }

    protected virtual void OnEnable() => controls.Enable();
    protected virtual void OnDisable() => controls.Disable();

    protected virtual void FixedUpdate()
    {
        characterController.ControlLocomotionType();
        characterController.ControlRotationType();
       
        characterAnimator.UpdateAnimator(characterController);
        characterAnimator.SetAnimatorMoveSpeed(characterController);
    }


    protected virtual void Update()
    {
        InputHandle();

        characterController.UpdateMotor();

    }

    public virtual void OnAnimatorMove()
    {
        characterController.ControlAnimatorRootMotion();
    }

    #region Controller Setup

    public void Init(PlayerController controller, PlayerCombatController combat, vThirdPersonCamera camera, PlayerInteract interact , CharacterAnimator anim)
    {
        characterController = controller;
        playerCombatController = combat;
        tpCamera = camera;
        interactions = interact;
        characterAnimator = anim;   

    }

    protected virtual void InputHandle()
    {
        MoveInput();
        CameraInput();
        SprintInput();
        JumpInput();

    }

    #endregion

    #region New Input System adaptation

    public virtual void MoveInput()
    {
        characterController.input.x = moveInput.x;
        characterController.input.z = moveInput.y;
    }

    protected virtual void CameraInput()
    {
        if (!cameraMain)
        {
            if (!Camera.main)
                Debug.Log("Missing MainCamera");
            else
            {
                cameraMain = Camera.main;
                characterController.rotateTarget = cameraMain.transform;
            }
        }

        if (cameraMain)
            characterController.UpdateMoveDirection(cameraMain.transform);

        if (tpCamera == null)
            return;

        tpCamera.RotateCamera(lookInput.x, lookInput.y);
    }


    protected virtual void SprintInput()
    {
        characterController.Sprint(sprintHeld);
    }

    protected virtual bool JumpConditions()
    {
        return characterController.isGrounded &&
               characterController.GroundAngle() < characterController.slopeLimit &&
               !characterController.isJumping &&
               !characterController.stopMove;
    }

    protected virtual void JumpInput()
    {
        if (jumpPressed && JumpConditions())
            characterController.Jump();

        jumpPressed = false; // consume press
    }

    #endregion
}
