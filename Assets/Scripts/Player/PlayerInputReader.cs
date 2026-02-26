using UnityEngine;

public class PlayerInputReader
{
    public PlayerControls controls;

    public Vector2 Move;
    public Vector2 Look;

    public bool JumpPressed;
    public bool SprintHeld;

    public bool AttackPressed;
    public bool PowerAttackGamepadPressed;
    public bool ChargeHeld;
    public bool ThrowHeld;
    public bool BlockHeld;

    public bool EmitPressed;
    public bool PushPressed;
    public bool LockPressed;
    public bool InteractPressed;
    public bool InventoryPressed;

    public float SpellScroll;

    public bool SwitchToGamePressed;

    public void Init()
    {
        controls = new PlayerControls();
        Bind();
        BindUiInput();
        controls.Enable();
    }

    private void Bind()
    {
        controls.Player.Move.performed += c => Move = c.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => Move = Vector2.zero;

        controls.Player.Look.performed += c => Look = c.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => Look = Vector2.zero;

        controls.Player.Jump.performed += _ => JumpPressed = true;

        controls.Player.Sprint.performed += _ => SprintHeld = true;
        controls.Player.Sprint.canceled += _ => SprintHeld = false;

        controls.Player.Attack.performed += _ => AttackPressed = true;
        controls.Player.PowerAttackGamepad.performed += _ => PowerAttackGamepadPressed = true;

        controls.Player.PowerAttackHold.performed += _ => ChargeHeld = true;
        controls.Player.PowerAttackHold.canceled += _ => ChargeHeld = false;

        controls.Player.ThrowItem.performed += _ => ThrowHeld = true;
        controls.Player.ThrowItem.canceled += _ => ThrowHeld = false;

        controls.Player.Block.performed += _ => BlockHeld = true;
        controls.Player.Block.canceled += _ => BlockHeld = false;

        controls.Player.Emit.performed += _ => EmitPressed = true;
        controls.Player.AgressivePush.performed += _ => PushPressed = true;

        controls.Player.LockTarget.performed += _ => LockPressed = true;
        controls.Player.Interaction.performed += _ => InteractPressed = true;
        controls.Player.Inventory.performed += _ => InventoryPressed = true;

        controls.Player.SpellChange.performed += c =>
            SpellScroll = c.ReadValue<float>();
    }

    private void BindUiInput()
    {
        controls.UI.SwitchToGame.performed += _ => SwitchToGamePressed = true;
    }

    public void Consume(ref bool flag) => flag = false;
    public void ResetSpellScroll() => SpellScroll = 0f;
}