
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMotor locomotion;
    PlayerCombatController combatController;
    PlayerStatsModifier statsModifier;
    PlayerStats stats;
    PlayerTargetLock targetLock;
    PlayerInteract interact;

    private void OnEnable()
    {
        PlayerTargetLock.OnTargetLost += OnTargetLost;
    }

    private void OnDisable()
    {
        PlayerTargetLock.OnTargetLost -= OnTargetLost;  
    }

    public void Init(PlayerStateServiceProvider provider)
    {
        locomotion = provider.controller;
        statsModifier = provider.statsModifier;
        combatController = provider.combatController;
        stats = provider.stats;
        targetLock = provider.targetLock;
        interact = provider.interact;
    }


    public void UpdateInput(Vector2 input)
    {
        locomotion.input.x = input.x;
        locomotion.input.z = input.y;
    }

    public void UpdateMotor()
    {
        locomotion.UpdateMotor(stats.jumpHeight);
    }

    public void UpdateAnimator()
    {
        locomotion.UpdateAnimatorLocomotion();
    }


    #region Locomotion
    /// <summary>
    /// Наземное движение
    /// </summary>
    /// <param name="dir">Направление ввода</param>
    public void MoveCharacter(Vector3 dir)
    {
        bool canMove = locomotion.IsGrounded && !locomotion.IsJumping;
        if (!canMove) return;

        float baseSpeed = locomotion.IsSprinting
            ? stats.runningSpeed
            : stats.walkSpeed;

        // ↓ здесь живёт вся логика замедлений
        float targetMultiplier =
            (locomotion.StopMove ||
             combatController.IsAttacking ||
             statsModifier.IsDamaged ||
            locomotion.IsDodging)
            ? 0f
            : 1f;

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
    public virtual void RotateCharacter(Vector3 input)
    {
        // input нулевой → не вращаемся
        if (input.sqrMagnitude < 0.01f && !combatController.IsAttacking)
            return;
        // нельзя вращаться при атаке, повреждении, или если запрещено вращение в воздухе
        if (statsModifier.IsDamaged || (!locomotion.jumpAndRotate && !locomotion.IsGrounded))
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

    /// <summary>
    /// Контроль прыжка или отскока (если есть цель)
    /// </summary>
    /// <param name="dir">Направление ввода</param>
    public void HandleJumpOrDodge(Vector3 dir)
    {
        if (locomotion.IsLockedOnTarget)
        {
            Dodge(dir);
            return;

        }
        Jump();

    }

    private void Dodge(Vector3 dir)
    {
        if (!combatController.IsAttacking && !locomotion.IsDodging && stats.currentStamina > 0)
        {
            locomotion.Dodge(dir);

        }
    }

    private void Jump()
    {
        bool canJump = locomotion.IsGrounded &&
                 locomotion.GroundAngle() < locomotion.slopeLimit &&
                 !locomotion.IsJumping &&
                 !combatController.IsAttacking &&
                 !statsModifier.IsDamaged &&
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

    /// <summary>
    /// Следит за потерей цели
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    private void OnTargetLost(Transform target, Transform source)
    {
        var ownTarget = target.GetInstanceID() == targetLock.currentTarget.GetInstanceID()
            && source.GetInstanceID() == transform.GetInstanceID();
        if (ownTarget)
        {
            ResetLockTarget();
        }
            
    }

    public void SetLockTarget()
    {
        if(targetLock.currentTarget != null)
        {
            ResetLockTarget();
            return;

        }
        var target = targetLock.GetLockedTarget();

        if (target == null) return;

        locomotion.rotateTarget = target;
        locomotion.IsLockedOnTarget = true;
    }

    public void ResetLockTarget()
    {
       
        targetLock.ResetLockTarget();
        locomotion.rotateTarget = null;
        locomotion.IsLockedOnTarget = false;
    }

    /// <summary>
    /// Смена фокуса цели
    /// </summary>
    /// <param name="mouseX">Ось Х (мышь или стик)</param>
    public void SwitchTarget(float mouseX)
    {
        if (locomotion.rotateTarget == null)
            return;

        var closest = targetLock.SwitchTarget(mouseX);

        if (closest != null)
        {
            locomotion.rotateTarget = closest;
            locomotion.IsLockedOnTarget = true;
        }
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
        if (combatController.isAttacking) return;

        interact.Interact();        
    }
    #endregion
}
