using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    PlayerLocomotionActionHandler locomotionHandler;
    PlayerCombatActionHandler combatHandler;

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
    private bool powerAttackGamepadPressed;
    private bool chargeHeld;
    private bool blockHeld;
    private bool interactPressed;
    private bool lockOnTargetPressed;
    private bool isPushPressed;
    private bool isEmitPressed;

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

        controls.Player.PowerAttackHold.performed +=crt=> chargeHeld = true;
        controls.Player.PowerAttackHold.canceled += ctx => chargeHeld = false;

        controls.Player.PowerAttackGamepad.performed += ctx => powerAttackGamepadPressed = true;

        controls.Player.AgressivePush.performed += ctx => isPushPressed = true;

        controls.Player.Emit.performed += ctx => isEmitPressed = true;

        controls.Player.Block.performed += ctx => blockHeld = true;
        controls.Player.Block.canceled += ctx => blockHeld = false;
        controls.Player.LockTarget.performed += ctx => lockOnTargetPressed = true;

        // Interaction
        controls.Player.Interaction.performed += ctx => interactPressed = true;
        cameraMain = Camera.main;

    }

    public void Init(PlayerLocomotionActionHandler controller,PlayerCombatActionHandler combatHandlder, PlayerAnimatorController animatorController, PlayerTargetLock targetLock)
    {  
        this.locomotionHandler = controller;
        this.combatHandler = combatHandlder;
        this.animator = animatorController;
        this.targetLock = targetLock;
       
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
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

       
        locomotionHandler.MoveAndRotate(moveDir);

        SprintInput();
        JumpInput();

        AttackInput();
        EmitInput();
        PushInput();    
        BlockInput();
        LockOnTargetInput();

        InteractionInput();

    }

   

    #region Motion Inputs



    protected virtual void SprintInput()
    {
        locomotionHandler.Sprint(sprintHeld);
    }


    protected virtual void JumpInput()
    {
        if (jumpPressed)
        {
            locomotionHandler.HandleJumpOrDodge(moveInput);
        }
        jumpPressed = false; // consume press
    }

    #endregion

    #region Combat Inputs

    private void AttackInput()
    {
        // Gamepad — мгновенно
        if (powerAttackGamepadPressed)
        {
            combatHandler.PerformPowerAttack();
            powerAttackGamepadPressed = false;
            return;
        }

        // Keyboard + Mouse: мощная атака по удержанию
        if (chargeHeld && attackPressed)
        {
            
            combatHandler.PerformPowerAttack();
            attackPressed = false;
            return;
        }

        // Обычная атака
        if (attackPressed)
        {

            if (throwHeld)
                combatHandler.ThrowWeapon();
            else
                combatHandler.PerformAttack();

            attackPressed = false;
        }
    }

    private void EmitInput()
    {
        if (isEmitPressed)
        {
            combatHandler.PerformEmit();
            isEmitPressed = false;  
        }
    }

    private void PushInput()
    {
        if (isPushPressed)
        {
            combatHandler.PerformPush();   

        }

        isPushPressed = false;  
    }


    private void BlockInput()
    {
        if (blockHeld)
        {
            if (throwHeld)
                combatHandler.ThrowShield();
            else
                combatHandler.PerformBlock();
        }
        else
        {
            combatHandler.CancelBlock();
        }

    }


    private void LockOnTargetInput()
    {
        if (lockOnTargetPressed)
        {
            targetLock.HandleSetTarget();
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
            locomotionHandler.Interact();
            interactPressed = false;
        }
    }
    #endregion


}