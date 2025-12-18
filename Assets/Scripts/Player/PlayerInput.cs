using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    PlayerController controller;
    PlayerAnimator animator;
    private Camera cameraMain;

    [HideInInspector] public PlayerControls controls;

    private Vector2 moveInput;   // Move
    private Vector2 lookInput;   // Mouse/Gamepad look
    private bool sprintHeld;
    private bool jumpPressed;
    private bool throwHeld;
    private bool attackPressed;
    private bool blockHeld;
    private bool interactPressed;
    private bool lockOnTargetPressed;

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
        controls.Player.Attack.performed += ctx => attackPressed = true;
        controls.Player.Block.performed += ctx => blockHeld = true;
        controls.Player.Block.canceled += ctx => blockHeld = false;
        controls.Player.LockTarget.performed += ctx => lockOnTargetPressed = true;

        // Interaction
        controls.Player.Interaction.performed += ctx => interactPressed = true;
        cameraMain = Camera.main;

    }

    public void Init(PlayerInputServiceProvider serviceProvider)
    {
        controller = serviceProvider.controller;
        animator = serviceProvider.animator;
    }


    protected virtual void OnEnable() => controls.Enable();
    protected virtual void OnDisable() => controls.Disable();

    protected virtual void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        controller.MoveCharacter(moveDir);
        controller.RotateCharacter(moveDir);    

        controller.UpdateAnimator();
        animator.SetAnimatorMoveSpeed();
    }

    protected virtual void Update()
    {

        InputHandle();
        controller.UpdateMotor();

    }


    protected virtual void InputHandle()
    {
        MoveInput();

        SprintInput();
        JumpInput();

        AttackInput();
        BlockInput();
        LockOnTargetInput();

        InteractionInput();

    }

    private void LateUpdate()
    {
        CameraInput();
    }

    #region Motion Inputs

    public virtual void MoveInput()
    {
        controller.UpdateInput(moveInput);
    }


    protected virtual void SprintInput()
    {
        controller.Sprint(sprintHeld);
    }


    protected virtual void JumpInput()
    {
        if (jumpPressed)
        {
            controller.HandleJumpOrDodge(moveInput);      
        }
        jumpPressed = false; // consume press
    }

    #endregion

    #region Combat Inputs

    private void AttackInput()
    {
        if (attackPressed)
        {
            if (throwHeld)
                controller.ThrowWeapon();
            else
                controller.PerformAttack();

            attackPressed = false;
        }

    }

    private void BlockInput()
    {
        if (blockHeld)
        {
            if (throwHeld)
                controller.ThrowShield();
            else
                controller.PerformBlock();
        }
        else
        {
            controller.CancelBlock();
        }

    }


    private void LockOnTargetInput()
    {
        if (lockOnTargetPressed)
        {
            controller.SetLockTarget();
            lockOnTargetPressed = false;
        }

        controller.SwitchTarget(lookInput.x);
    }

    #endregion

    #region Interaction Inputs
    private void InteractionInput()
    {
        if (interactPressed)
        {
            controller.Interact();
            interactPressed = false;
        }
    }
    #endregion

    #region Camera Inputs

    protected virtual void CameraInput()
    {
        if (!cameraMain)
        {
            if (!Camera.main)
                Debug.Log("Missing MainCamera");
            else
            {
                cameraMain = Camera.main;
                //provider.controller.rotateTarget = cameraMain.transform;
            }
        }
    }
    #endregion
}