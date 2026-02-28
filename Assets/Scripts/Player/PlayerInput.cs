using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputReader reader;

    private PlayerLocomotionActionHandler locomotion;
    private PlayerCombatActionHandler combat;
    private PlayerQuickSlotActionHandler quickSlots;
    private PlayerTargetLock targetLock;
    private PlayerUIManager ui;
    private PlayerAnimatorController animator;

    public static Action<GameState> PlayerModeChanged;

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

        reader = new PlayerInputReader();
        reader.Init();

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
        reader.controls.Disable();
    }

    public void EnableInput()
    {
        reader.controls.Enable();    
    }

    private void SwitchToGameInput()
    {
        reader.controls.UI.Disable();
        reader.controls.Player.Enable();
    }

    private void SwitchToUiInput()
    {
        reader.controls.Player.Disable();
        reader.controls.UI.Enable();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(reader.Move.x, 0, reader.Move.y);
        locomotion.MoveAndRotate(moveDir);

        locomotion.Sprint(reader.SprintHeld);

        if (reader.JumpPressed)
        {
            locomotion.HandleJumpOrDodge(reader.Move);
            reader.Consume(ref reader.JumpPressed);
        }
    }

    private void HandleCombat()
    {
        if (reader.PowerAttackGamepadPressed)
        {
            combat.PerformPowerAttack();
            reader.Consume(ref reader.PowerAttackGamepadPressed);
            return;
        }

        if (reader.ChargeHeld && reader.AttackPressed)
        {
            combat.PerformPowerAttack();
            reader.Consume(ref reader.AttackPressed);
            return;
        }

        if (reader.AttackPressed)
        {
            if (reader.ThrowHeld)
                combat.ThrowWeapon();
            else
                combat.PerformAttack();

            reader.Consume(ref reader.AttackPressed);
        }

        if (reader.EmitPressed)
        {
            combat.PerformEmit();
            reader.Consume(ref reader.EmitPressed);
        }

        if (reader.PushPressed)
        {
            combat.PerformPush();
            reader.Consume(ref reader.PushPressed);
        }

        if (reader.BlockHeld)
        {
            if (reader.ThrowHeld)
                combat.ThrowShield();
            else
                combat.PerformBlock();
        }
        else
        {
            combat.CancelBlock();
        }

        if (reader.LockPressed)
        {
            targetLock.HandleSetTarget();
            reader.Consume(ref reader.LockPressed);
        }

        targetLock.SwitchTarget(reader.Look.x);

        if (reader.SpellScroll != 0)
        {
            quickSlots.ChangeSpell(reader.SpellScroll > 0 ? 1 : -1);
            reader.ResetSpellScroll();
        }
    }

    private void HandleInteraction()
    {
        if (reader.InteractPressed)
        {
            locomotion.Interact();
            reader.Consume(ref reader.InteractPressed);
        }
    }

    private void HandleInventory()
    {
        if (!reader.InventoryPressed) return;

        ui.ToggleInventoryPanel(true);
        SwitchToUiInput();
        reader.Consume(ref reader.InventoryPressed);
    }

    private void HandleCloseUI()
    {
        if (!reader.SwitchToGamePressed) return;

        ui.CloseAllPanels();
        SwitchToGameInput();
        reader.Consume(ref reader.SwitchToGamePressed);
    }
}