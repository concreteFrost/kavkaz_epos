using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    PlayerController controller;
    PlayerTargetLock targetLock;
    PlayerAnimatorController animator;
    
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

    [Header("Менеджер атаки")]
    
    private bool attackHeld;
    private bool attackReleased;
    private float powerAttackThreshold = 0.5f;
    private float currAttackHoldTime = 0f;

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
        controls.Player.Attack.started += ctx =>
        {
            attackHeld = true;
            currAttackHoldTime = 0f;
        };

        controls.Player.Attack.canceled += ctx =>
        {
            attackHeld = false;
            attackReleased = true;
        };
        controls.Player.Block.performed += ctx => blockHeld = true;
        controls.Player.Block.canceled += ctx => blockHeld = false;
        controls.Player.LockTarget.performed += ctx => lockOnTargetPressed = true;

        // Interaction
        controls.Player.Interaction.performed += ctx => interactPressed = true;
        cameraMain = Camera.main;

    }

    public void Init(PlayerInputService serviceProvider)
    {
        controller = serviceProvider.controller;
        animator = serviceProvider.animator;
        targetLock = serviceProvider.targetLock;
    }


    protected virtual void OnEnable() => controls.Enable();
    protected virtual void OnDisable() => controls.Disable();

    protected virtual void FixedUpdate()
    {
       
        animator.UpdateAnimatorParameters();
    }

    protected virtual void Update()
    {
        InputHandle();
    }


    protected virtual void InputHandle()
    {
        Vector3 moveDir = new Vector3(moveInput.x,0, moveInput.y);

        controller.MoveAndRotate(moveDir);

        SprintInput();
        JumpInput();

        AttackInput();

        BlockInput();
        LockOnTargetInput();

        InteractionInput();

    }

    #region Motion Inputs



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
        if (attackHeld)
            currAttackHoldTime += Time.deltaTime;

        if (!attackReleased)
            return;

        if (throwHeld)
        {
            controller.ThrowWeapon();
        }
        else
        {
            if (currAttackHoldTime >= powerAttackThreshold)
                controller.PerformPowerAttack();
            else
                controller.PerformAttack();
        }

        attackReleased = false;

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
            targetLock.SetLockedTarget();
            lockOnTargetPressed = false;
        }

        targetLock.SwitchTarget(lookInput.x);
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

  
}