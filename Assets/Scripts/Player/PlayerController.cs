using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMotor locomotion;
    HumanoidCombatController combatController;
    PlayerStatsModifier statsModifier;
    PlayerStats stats;
    PlayerInteract interact;

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
        bool canMove = locomotion.IsGrounded && !locomotion.IsJumping;
        if (!canMove) return;

        float baseSpeed = locomotion.IsSprinting
            ? stats.runningSpeed
            : stats.walkSpeed;

        // ↓ здесь живёт вся логика замедлений
        float targetMultiplier = (locomotion.StopMove || locomotion.isHighSlope) ? 0f : 1f;

        locomotion.attackSlow = Mathf.Lerp(
            locomotion.attackSlow,
            targetMultiplier,
            Time.deltaTime * 10f
        );

        locomotion.moveSpeed = Mathf.Lerp(
            locomotion.moveSpeed,
            baseSpeed * locomotion.attackSlow,
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
        // input нулевой → не вращаемся
        if (input.sqrMagnitude < 0.01f)
            return;

        if (combatController.BlockRotation) return; 

        // нельзя вращаться при атаке, повреждении, или если запрещено вращение в воздухе
        if (!locomotion.jumpAndRotate && !locomotion.IsGrounded)
            return;

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
        if (!locomotion.StopMove && !locomotion.IsDodging && stats.currentStamina > 0)
        {
            locomotion.Dodge(dir);
        }
    }

    public void Jump()
    {
        bool canJump = locomotion.IsGrounded &&
                 locomotion.GroundAngle() < locomotion.slopeLimit &&
                 !locomotion.IsJumping &&
                 !locomotion.StopMove &&
                 stats.currentStamina > 0;

        locomotion.isJumping = canJump;

        if (locomotion.isJumping)
        {
            locomotion.Jump(stats.jumpTimer);
            statsModifier.ReduceStamina(stats.staminaJumpReducePenalty);
        }
    }

    public void Sprint(bool sprintHeld)
    {
        bool isMoving = locomotion.input.sqrMagnitude > 0.1f;
        bool hasStamina = stats.currentStamina > 0;
        Vector3 localDir = transform.InverseTransformDirection(locomotion.MoveDirection);
        bool isMovingForward = localDir.z > 0.1f;

        bool canSprint =
                sprintHeld &&
                locomotion.IsGrounded &&
                !combatController.IsAttacking &&
                !statsModifier.IsDamaged &&
                isMoving &&
                isMovingForward &&
                hasStamina;

        locomotion.isSprinting = canSprint;

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
        if (!locomotion.IsJumping && locomotion.IsGrounded && !locomotion.IsDodging && stats.currentStamina > 0)
        {
            combatController.PerformAttack();
        }
    }
    public void ThrowWeapon()
    {
        combatController.ThrowWeapon();
    }
    public void ThrowShield()
    {
        combatController.ThrowShield();
    }

    public void PerformBlock()
    {
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
