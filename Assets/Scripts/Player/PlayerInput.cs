using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerInputServiceProvider provider;
    private Camera cameraMain;

    [HideInInspector] public PlayerControls controls;

    private Vector2 moveInput;   // Move
    private Vector2 lookInput;   // Mouse/Gamepad look
    private bool sprintHeld;
    private bool jumpPressed;
    private bool throwHeld;
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

        controls.Player.ThrowItem.performed += ctx => throwHeld = true;
        controls.Player.ThrowItem.canceled += ctx => throwHeld = false;

        controls.Player.Attack.performed += ctx =>
        {
            if (throwHeld)
               provider.combatController.ThrowWeapon();
            else
                provider.combatController.PerformAttack();

        };

        controls.Player.Block.performed += ctx =>
        {
            if (throwHeld)
                provider.combatController.ThrowShield();
            else
                provider.combatController.PerformBlock();
        };

        controls.Player.Block.canceled += ctx => provider.combatController.CancelBlock();

        // Interaction
        controls.Player.Interaction.performed += ctx => provider.interact.Interact();

    }

    protected virtual void OnEnable() => controls.Enable();
    protected virtual void OnDisable() => controls.Disable();

    protected virtual void FixedUpdate()
    {

       provider.controller.ControlLocomotionType();
       provider.controller.ControlRotationType();

       provider.animator.UpdateAnimator(provider.controller);
       provider.animator.SetAnimatorMoveSpeed(provider.controller);
    }


    protected virtual void Update()
    {
        
        InputHandle();
        provider.controller.UpdateMotor();

    }

    public void Init(PlayerInputServiceProvider serviceProvider)
    {
        provider = serviceProvider;
    }

    //public virtual void OnAnimatorMove()
    //{
    //    characterController.ControlAnimatorRootMotion();
    //}

    #region Controller Setup



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
       provider.controller.input.x = moveInput.x;
       provider.controller.input.z = moveInput.y;
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
                provider.controller.rotateTarget = cameraMain.transform;
            }
        }

        if (cameraMain)
            provider.controller.UpdateMoveDirection(cameraMain.transform);

        if (provider.vThirdPersonCamera == null)
            return;

        provider.vThirdPersonCamera.RotateCamera(lookInput.x, lookInput.y);
    }


    protected virtual void SprintInput()
    {
        provider.controller.Sprint(sprintHeld);
    }

    protected virtual bool JumpConditions()
    {
        return provider.controller.isGrounded &&

               provider.controller.GroundAngle() < provider.controller.slopeLimit &&
               !provider.controller.isJumping &&
               !provider.controller.isDamaged &&
               !provider.controller.stopMove;
    }

    protected virtual void JumpInput()
    {
        if (jumpPressed && JumpConditions())
            provider.controller.Jump();

        jumpPressed = false; // consume press
    }

    #endregion
}
