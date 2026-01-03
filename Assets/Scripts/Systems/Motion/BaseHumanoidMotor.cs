using System;
using UnityEngine;
public abstract class BaseHumanoidMotor : MonoBehaviour, IHumanoidMovement
{
    [Header("- Rotation")]
    [Tooltip("Rotation speed of the character")]
    public float rotationSpeed = 8f;
    public float blockedRotationSpeed = 1.5f;

    [Header("animator smooth speed")]
    [Range(1f, 20f)]
    public float movementSmooth = 10f;
    [Range(0f, 1f)]
    public float animationSmooth = 0.2f;

    [Header("- Airborne")]
    [Tooltip("Speed that the character will move while airborne")]
    public float airSpeed = 5f;
    [Tooltip("Smoothness of the direction while airborne")]
    public float airSmooth = 6f;
    [Tooltip("Apply extra gravity when the character is not grounded")]
    public float extraGravity = -10f;
    [HideInInspector]
    public float limitFallVelocity = -15f;

    [Header("- Ground")]
    [Tooltip("Layers that the character can walk on")]
    public LayerMask groundLayer = 1 << 0;
    [Tooltip("Distance to became not grounded")]
    public float groundMinDistance = 0.25f;
    public float groundMaxDistance = 0.5f;

    [Header("- Root Motion")]
    [Tooltip("Проверяет дистанцию до обьекта во время использования applyRootMotion")]
    public float distanceToObstacle = 0.8f;

    #region Components
    internal Animator animator;
    #endregion

    internal float inputMagnitude;
    internal float groundDistance;

    internal float moveSpeed;                           // set the current moveSpeed for the MoveCharacter method
    internal float verticalSpeed;
    internal float horizontalSpeed;
    internal float verticalVelocity;                    // set the vertical velocity of the rigidbody
    internal float colliderRadius, colliderHeight;      // storage capsule collider extra information        
    internal float heightReached;                       // max height that character reached in air;
    internal float jumpCounter;                         // used to count the routine to reset the jump
    internal float dodgeX;
    internal float dodgeY;

    internal RaycastHit groundHit;                      // raycast to hit the ground 

    internal Transform rotateTarget;
    internal Vector3 input;                             // generate raw input for the controller
    internal Vector3 colliderCenter;                    // storage the center of the capsule collider info                
    internal Vector3 inputSmooth;                       // generate smooth input based on the inputSmooth value       
    internal Vector3 moveDirection;

    internal bool stopMove = false;
    public bool isSprinting;
    public bool isJumping;
    internal bool isGrounded = true;
    public bool isLockedOnTarget;
    public bool isDodging;
   
    internal bool isRotationBlocked = false;

    #region IHumanoidMovement Contract
    public bool StopMove { get => stopMove; set => stopMove = value; }  
    public Vector3 GetInverseTransformDirection() => transform.InverseTransformDirection(moveDirection);
    public Vector3 MoveDirection { get => moveDirection; }
    public float AnimationSmooth { get => animationSmooth; }
    public float InputMagnitude { get => inputMagnitude; }
    public float VerticalSpeed { get => verticalSpeed; }
    public float HorizontalSpeed { get => horizontalSpeed; }
    public bool BlockRotation { get => isRotationBlocked; set => isRotationBlocked = value; }
    public bool IsLockedOnTarget { get => isLockedOnTarget; set => isLockedOnTarget = value; }
    public float GroundDistance { get => groundDistance; }
    public bool ApplyRootMotion { get; set; }
    public bool IsSprinting { get => isSprinting; }
    public bool IsJumping { get => isJumping; }
    public bool IsGrounded { get => isGrounded; }
    public bool IsDodging { get => isDodging; set => isDodging = value; }
    public float DodgeX { get => dodgeX; set => dodgeX = value; }
    public float DodgeY { get => dodgeY; set => dodgeY = value; }
    #endregion


    /// <summary>
    /// Обновляет анимацию ДВИЖЕНИЯ
    /// </summary>
    public virtual void UpdateAnimatorLocomotion()
    {
        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
        verticalSpeed = relativeInput.z;
        horizontalSpeed = relativeInput.x;

        var newInput = new Vector2(verticalSpeed, horizontalSpeed);

        inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? AnimatorParameters.runningSpeed : AnimatorParameters.walkSpeed);
    }

    #region Abstract Methods

    public abstract void Init(Animator anim);
    public abstract void UseRootMotion();
    public abstract void UseRootMotionWithObstacles();

    /// <summary>
    /// Основной метод обновления состояния движения персонажа.
    /// Выполняет проверку на земле, управление прыжком и движение в воздухе.
    /// Должен вызываться каждый кадр (обычно в Update или FixedUpdate).
    /// </summary>
    /// <param name="jumpHeight">Высота прыжка, которая будет применена при контроле прыжка.</param>
    public abstract void UpdateMotor(float jumpHeight);

    /// <summary>
    /// Двигает персонаж в заданом направлении
    /// </summary>
    /// <param name="direction">Направление</param>
    public abstract void MoveCharacter(Vector3 direction);

    /// <summary>
    /// Полностью останавливает движение персонажа
    /// </summary>
    public abstract void StopMovement();

    #endregion

    #region Rotation

    /// <summary>
    /// Поворачивает персонажа в обычном состояние по отношению к вводу
    /// </summary>
    /// <param name="direction">Направление</param>
    public virtual void RotateToDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        float finalRotation = isRotationBlocked ? blockedRotationSpeed : rotationSpeed;

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, finalRotation * Time.deltaTime, 0.1f);
        transform.rotation = Quaternion.LookRotation(desiredForward);
    }

    /// <summary>
    /// Поворачивает персонажа относительно цели
    /// </summary>
    /// <param name="targetPosition">Позиция цели</param>
    public virtual void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 lookDir = targetPosition - transform.position;
        lookDir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        float finalRotation = isRotationBlocked ? blockedRotationSpeed : rotationSpeed;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, finalRotation * Time.deltaTime);
    }

    #endregion

    #region Dodge

    /// <summary>
    /// Уклоняет персонажа относительно направления ввода
    /// </summary>
    /// <param name="dir">Направление ввода</param>
    public virtual void Dodge(Vector2 dir)
    {

        isDodging = true;

        float _dodgeX = 0f;
        float _dodgeY = 0f;

        Vector3 relativeInput = GetInverseTransformDirection();

        if (relativeInput.sqrMagnitude < 0.01f)
        {
            // без движения — всегда назад
            _dodgeY = -1f;
        }
        else if (Mathf.Abs(relativeInput.x) > Mathf.Abs(relativeInput.z)) //
        {
            _dodgeX = Mathf.Sign(relativeInput.x);
        }
        else
        {
            _dodgeY = Mathf.Sign(relativeInput.z);
        }

        //statsModifier.ReduceStamina(stats.staminaJumpReducePenalty);

        this.dodgeX = _dodgeX;
        this.dodgeY = _dodgeY;
    }

    #endregion

    #region Jump

    /// <summary>
    /// Осуществляет прыжок персонажа по заданой высоте
    /// </summary>
    /// <param name="jumpTimer">Высота</param>
    public virtual void Jump(float jumpTimer)
    {
        jumpCounter = jumpTimer;
        isJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

    /// <summary>
    /// Управляет поведением прыжка персонажа.
    /// Метод уменьшает таймер прыжка и при необходимости завершает состояние прыжка.
    /// Также задаёт вертикальную скорость Rigidbody для достижения указанной высоты прыжка.
    /// </summary>
    /// <param name="jumpHeight">Высота прыжка, которая будет применена к вертикальной скорости Rigidbody.</param>
    protected abstract void ControlJumpBehaviour(float jumpHeight);

    /// <summary>
    /// Управляет движением персонажа в воздухе (air control).
    /// Метод позволяет изменять горизонтальное направление и скорость персонажа, 
    /// пока он находится в воздухе, плавно интерполируя текущую скорость к целевой.
    /// Также отслеживает максимальную достигнутую высоту прыжка.
    /// </summary>
    public abstract void AirControl();
    #endregion

    #region Ground Check                

    /// <summary>
    /// Проверяет, находится ли персонаж на земле.
    /// Вычисляет дистанцию до земли, корректирует физический материал коллайдера
    /// и применяет дополнительную силу гравитации при необходимости.
    /// Обновляет флаг <c>isGrounded</c> и максимальную достигнутую высоту <c>heightReached</c>.
    /// </summary>
    protected abstract void CheckGround();

    /// <summary>
    /// Контролирует физический материал коллайдера персонажа в зависимости от того,
    /// стоит ли персонаж на земле, угла поверхности и наличия движения.
    /// Меняет материал на скользкий, стандартный или максимальное трение.
    /// </summary>


    /// <summary>
    /// Рассчитывает расстояние до поверхности земли под персонажем.
    /// Использует RayCast и SphereCast для точного определения дистанции.
    /// Обновляет значение <c>groundDistance</c>.
    /// </summary>
    protected abstract void CheckGroundDistance();

    #endregion

}
