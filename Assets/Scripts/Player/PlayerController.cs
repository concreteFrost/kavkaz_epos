using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMotor locomotion;
    HumanoidCombatController combatController;
    PlayerStatsModifier statsModifier;
    PlayerStats stats;
    PlayerInteract interact;

    PlayerActionGuards actionGuards;

    private void Update()
    {
        UpdateMotor();
    }

    private void FixedUpdate()
    {
        UpdateAnimator();
    }

    public void Init(PlayerStateService provider)
    {
        locomotion = provider.controller;
        statsModifier = provider.statsModifier;
        combatController = provider.combatController;
        stats = provider.stats;
        interact = provider.interact;

        actionGuards = new PlayerActionGuards(locomotion, combatController, stats, statsModifier);


    }

    public void MoveAndRotate(Vector3 dir)
    {
        MoveCharacter(dir);
        RotateCharacter(dir);
    }


    public void UpdateInput(Vector2 input)
    {
        locomotion.input.x = input.x;
        locomotion.input.z = input.y;
    }

    private void UpdateMotor()
    {
        locomotion.UpdateMotor(stats.jumpHeight);
    }

    private void UpdateAnimator()
    {
        locomotion.UpdateAnimatorLocomotion();
    }


    #region Locomotion
    /// <summary>
    /// Наземное движение
    /// </summary>
    /// <param name="dir">Направление ввода</param>
    private void MoveCharacter(Vector3 dir)
    {
        if (!actionGuards.CanMove())
        {
            locomotion.inputSmooth = Vector2.zero;
            return;
        }

        float baseSpeed = locomotion.IsSprinting
            ? stats.runningSpeed
            : stats.walkSpeed;


        locomotion.moveSpeed = Mathf.Lerp(
            locomotion.moveSpeed,
            baseSpeed,
            locomotion.movementSmooth * Time.deltaTime
        );

        locomotion.MoveCharacter(dir);
    }


    /// <summary>
    /// Поворот персонажа в сторону ввода или в сторону цели
    /// </summary>
    /// <param name="input">Направление ввода</param>
    private void RotateCharacter(Vector3 input)
    {

        if (!actionGuards.CanRotate(input)) return;

        
        // lock-on активен → вращаем к цели
        if (locomotion.rotateTarget != null)
        {
         
            locomotion.RotateToTarget(locomotion.rotateTarget.position);
            return;
        }
        // иначе — вращаемся по движению игрока
        locomotion.RotateToDirection(locomotion.MoveDirection);
    }

    public void Dodge(Vector3 dir)
    {
        if (!actionGuards.CanDodge()) return;

        locomotion.Dodge(dir);

    }

    public void Jump()
    {
        if (!actionGuards.CanJump()) return;

        locomotion.Jump(stats.jumpTimer);
        statsModifier.ReduceStamina(stats.staminaJumpReducePenalty);
    }

    public void Sprint(bool sprintHeld)
    {

        locomotion.isSprinting = actionGuards.CanSprint(sprintHeld);

        if (locomotion.isSprinting)
        {
            statsModifier.ReduceStamina(stats.staminaRunReducePenalty);
        }

    }

    #endregion

    #region Target Lock

    public void SetLockTarget(Transform target)
    {
        locomotion.rotateTarget = target;
    }

    public void ResetLockTarget()
    {
        locomotion.rotateTarget = null;
    }

    #endregion

    #region Combat
    public void PerformAttack()
    {
        if (!actionGuards.CanAttack()) return;

        combatController.PerformAttack();

    }
    public void ThrowWeapon()
    {
        if (!actionGuards.CanThrowWeapon()) return;

        combatController.ThrowWeapon();
    }
    public void ThrowShield()
    {
        if (!actionGuards.CanThrowWeapon()) return;

        combatController.ThrowShield();
    }

    public void PerformBlock()
    {
        if (!actionGuards.CanBlock()) return;
        combatController.PerformBlock();
    }

    public void CancelBlock()
    {
        combatController.CancelBlock();
    }

    #endregion

    #region Interaction

    public void Interact()
    {
        //не взаимодействуем если игрок атакует
        if (locomotion.StopMove) return;

        interact.Interact();
    }
    #endregion
}
