using UnityEngine;
using System;

public class PlayerGameInput : MonoBehaviour
{
    public PlayerInputReader reader;
    private PlayerLocomotionActionHandler locomotion;
    private PlayerCombatActionHandler combat;
    private PlayerQuickSlotActionHandler quickSlots;
    private PlayerTargetLock targetLock;
    private PlayerAnimatorController animator;

    public void Init(
        PlayerInputReader reader,
        PlayerLocomotionActionHandler locomotion,
        PlayerCombatActionHandler combatHandler,
        PlayerQuickSlotActionHandler quickSlotHandler,
        PlayerAnimatorController animatorController,
        PlayerTargetLock targetLock
        )
    {
        this.reader = reader;
        this.locomotion = locomotion;
        this.combat = combatHandler;
        this.quickSlots = quickSlotHandler;
        this.animator = animatorController;
        this.targetLock = targetLock;
    }


    private void Update()
    {
        HandleMovement();
        HandleCombat();
        HandleInteraction();
        HandleOpenInventory();

    }

    private void FixedUpdate()
    {
        animator.UpdateAnimatorParameters();
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

    private void HandleOpenInventory()
    {
        if (!reader.InventoryPressed) return;

        //ui.ToggleInventoryPanel(true);
       
        reader.Consume(ref reader.InventoryPressed);
        GameStateManager.GameStateChanged?.Invoke(GameState.Inventory);
    }

}
