using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputReader input;

    private PlayerLocomotionActionHandler locomotion;
    private PlayerCombatActionHandler combat;
    private PlayerQuickSlotActionHandler quickSlots;
    private PlayerTargetLock targetLock;
    private PlayerUIManager ui;
    private PlayerAnimatorController animator;

    public static Action<GameState> PlayerModeChanged;

    private void Awake()
    {
        input = new PlayerInputReader();
        input.Init();
    }

    public void Init(
        PlayerLocomotionActionHandler locomotion,
        PlayerCombatActionHandler combatHandler,
        PlayerQuickSlotActionHandler quickSlotHandler,
        PlayerAnimatorController animatorController,
        PlayerTargetLock targetLock,
        PlayerUIManager uiManager)
    {
        this.locomotion = locomotion;
        this.combat = combatHandler;
        this.quickSlots = quickSlotHandler;
        this.animator = animatorController;
        this.targetLock = targetLock;
        this.ui = uiManager;

        SwitchToGameInput();
    }

    private void Update()
    {
        HandleMovement();
        HandleCombat();
        HandleInteraction();
        HandleInventory();
        HandleCloseUI();
    }

    private void FixedUpdate()
    {
        animator.UpdateAnimatorParameters();
    }

    public void DisableInput()
    {
        input.controls.Disable();
    }

    public void EnableInput()
    {
        input.controls.Enable();    
    }

    private void SwitchToGameInput()
    {
        input.controls.UI.Disable();
        input.controls.Player.Enable();
    }

    private void SwitchToUiInput()
    {
        input.controls.Player.Disable();
        input.controls.UI.Enable();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(input.Move.x, 0, input.Move.y);
        locomotion.MoveAndRotate(moveDir);

        locomotion.Sprint(input.SprintHeld);

        if (input.JumpPressed)
        {
            locomotion.HandleJumpOrDodge(input.Move);
            input.Consume(ref input.JumpPressed);
        }
    }

    private void HandleCombat()
    {
        if (input.PowerAttackGamepadPressed)
        {
            combat.PerformPowerAttack();
            input.Consume(ref input.PowerAttackGamepadPressed);
            return;
        }

        if (input.ChargeHeld && input.AttackPressed)
        {
            combat.PerformPowerAttack();
            input.Consume(ref input.AttackPressed);
            return;
        }

        if (input.AttackPressed)
        {
            if (input.ThrowHeld)
                combat.ThrowWeapon();
            else
                combat.PerformAttack();

            input.Consume(ref input.AttackPressed);
        }

        if (input.EmitPressed)
        {
            combat.PerformEmit();
            input.Consume(ref input.EmitPressed);
        }

        if (input.PushPressed)
        {
            combat.PerformPush();
            input.Consume(ref input.PushPressed);
        }

        if (input.BlockHeld)
        {
            if (input.ThrowHeld)
                combat.ThrowShield();
            else
                combat.PerformBlock();
        }
        else
        {
            combat.CancelBlock();
        }

        if (input.LockPressed)
        {
            targetLock.HandleSetTarget();
            input.Consume(ref input.LockPressed);
        }

        targetLock.SwitchTarget(input.Look.x);

        if (input.SpellScroll != 0)
        {
            quickSlots.ChangeSpell(input.SpellScroll > 0 ? 1 : -1);
            input.ResetSpellScroll();
        }
    }

    private void HandleInteraction()
    {
        if (input.InteractPressed)
        {
            locomotion.Interact();
            input.Consume(ref input.InteractPressed);
        }
    }

    private void HandleInventory()
    {
        if (!input.InventoryPressed) return;

        ui.ToggleInventoryPanel(true);
        SwitchToUiInput();
        input.Consume(ref input.InventoryPressed);
    }

    private void HandleCloseUI()
    {
        if (!input.SwitchToGamePressed) return;

        ui.CloseAllPanels();
        SwitchToGameInput();
        input.Consume(ref input.SwitchToGamePressed);
    }
}