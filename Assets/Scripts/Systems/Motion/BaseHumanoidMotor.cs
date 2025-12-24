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
    [Tooltip("Max angle to walk")]
    [Range(30, 80)] public float slopeLimit = 45f;
    internal bool isHighSlope = false; //предотвращает движение если угол наклона выше

    [Header("- Root Motion")]
    [Tooltip("Проверяет дистанцию до обьекта во время использования applyRootMotion")]
    public float distanceToObstacle;

    #region Components

    internal Animator animator;
    internal PhysicsMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics, hangPhysics;         // create PhysicMaterial for the Rigidbody
    internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
    internal CapsuleCollider _capsuleCollider;                                          // access CapsuleCollider information

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

    internal bool isSprinting;
    internal bool isJumping;
    internal bool isGrounded = true;
    internal bool isLockedOnTarget;
    internal bool isDodging;
    internal bool isHanging;
    internal bool isRotationBlocked = false;

    #region ICharacterAnimData
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

    public virtual void Init(HumanoidMotorServices service)
    {
        animator = service.animator;

        animator.updateMode = AnimatorUpdateMode.Fixed;

        // slides the character through walls and edges
        frictionPhysics = new PhysicsMaterial();
        frictionPhysics.name = "frictionPhysics";
        frictionPhysics.staticFriction = .25f;
        frictionPhysics.dynamicFriction = .25f;
        frictionPhysics.frictionCombine = PhysicsMaterialCombine.Multiply;

        // prevents the collider from slipping on ramps
        maxFrictionPhysics = new PhysicsMaterial();
        maxFrictionPhysics.name = "maxFrictionPhysics";
        maxFrictionPhysics.staticFriction = 1f;
        maxFrictionPhysics.dynamicFriction = 1f;
        maxFrictionPhysics.frictionCombine = PhysicsMaterialCombine.Maximum;

        // air physics 
        slippyPhysics = new PhysicsMaterial();
        slippyPhysics.name = "slippyPhysics";
        slippyPhysics.staticFriction = 0f;
        slippyPhysics.dynamicFriction = 0f;
        slippyPhysics.frictionCombine = PhysicsMaterialCombine.Minimum;

        // rigidbody info
        _rigidbody = GetComponent<Rigidbody>();
        // capsule collider info
        _capsuleCollider = GetComponent<CapsuleCollider>();

        // save your collider preferences 
        colliderCenter = GetComponent<CapsuleCollider>().center;
        colliderRadius = GetComponent<CapsuleCollider>().radius;
        colliderHeight = GetComponent<CapsuleCollider>().height;

        _rigidbody.WakeUp();
    }


    public void UseRootMotion()
    {
        _rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);
        _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);
       
    }

    public void UseRootMotionWithObstacles()
    {
        _rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);

        RaycastHit hit;

        //центр игрока
        var center = transform.TransformPoint(colliderCenter);

        //Если есть приграда то игнорировать движение вперед
        if (!Physics.Raycast(center, _rigidbody.transform.forward, out hit, distanceToObstacle))
        {
            _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);
        }
           
    }


    /// <summary>
    /// Обновляет анимацию ДВИЖЕНИЯ
    /// </summary>
    public void UpdateAnimatorLocomotion()
    {
        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
        verticalSpeed = relativeInput.z;
        horizontalSpeed = relativeInput.x;

        var newInput = new Vector2(verticalSpeed, horizontalSpeed);

        inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? AnimatorConsts.runningSpeed : AnimatorConsts.walkSpeed);
    }

    #region Abstract Methods

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
    public void Dodge(Vector2 dir)
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
    protected virtual void ControlJumpBehaviour(float jumpHeight)
    {
        if (!isJumping) return;

        jumpCounter -= Time.deltaTime;
        if (jumpCounter <= 0)
        {
            jumpCounter = 0;
            isJumping = false;
        }
        // apply extra force to the jump height   
        var vel = _rigidbody.linearVelocity;
        vel.y = jumpHeight;
        _rigidbody.linearVelocity = vel;
    }

    /// <summary>
    /// Управляет движением персонажа в воздухе (air control).
    /// Метод позволяет изменять горизонтальное направление и скорость персонажа, 
    /// пока он находится в воздухе, плавно интерполируя текущую скорость к целевой.
    /// Также отслеживает максимальную достигнутую высоту прыжка.
    /// </summary>
    public virtual void AirControl()
    {
        if (isGrounded) return;

        // обновляем максимальную высоту прыжка
        if (transform.position.y > heightReached) heightReached = transform.position.y;

        // нормализуем направление движения по горизонтали
        moveDirection.y = 0;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

        // рассчитываем целевую позицию и скорость
        Vector3 targetPosition = _rigidbody.position + (moveDirection * airSpeed) * Time.deltaTime;
        Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

        // сохраняем вертикальную скорость и плавно применяем целевую скорость
        targetVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = Vector3.Lerp(_rigidbody.linearVelocity, targetVelocity, airSmooth * Time.deltaTime);
    }


    #endregion

    #region Ground Check                

    /// <summary>
    /// Проверяет, находится ли персонаж на земле.
    /// Вычисляет дистанцию до земли, корректирует физический материал коллайдера
    /// и применяет дополнительную силу гравитации при необходимости.
    /// Обновляет флаг <c>isGrounded</c> и максимальную достигнутую высоту <c>heightReached</c>.
    /// </summary>
    protected virtual void CheckGround()
    {
        CheckGroundDistance();
        ControlMaterialPhysics();

        if (groundDistance <= groundMinDistance)
        {
            isGrounded = true;
            if (!IsJumping && groundDistance > 0.05f)
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);

            heightReached = transform.position.y;
        }
        else
        {
            if (GroundDistance >= groundMaxDistance)
            {
                isGrounded = false;
                verticalVelocity = _rigidbody.linearVelocity.y;
                if (!IsJumping)
                {
                    _rigidbody.AddForce(transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            else if (!IsJumping)
            {
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
            }
        }
    }

    /// <summary>
    /// Контролирует физический материал коллайдера персонажа в зависимости от того,
    /// стоит ли персонаж на земле, угла поверхности и наличия движения.
    /// Меняет материал на скользкий, стандартный или максимальное трение.
    /// </summary>
    protected virtual void ControlMaterialPhysics()
    {
        _capsuleCollider.material = (isGrounded && GroundAngle() <= slopeLimit + 1) ? frictionPhysics : slippyPhysics;

        if (IsGrounded && input == Vector3.zero)
            _capsuleCollider.material = maxFrictionPhysics;
        else if (IsGrounded && input != Vector3.zero)
            _capsuleCollider.material = frictionPhysics;
        else
            _capsuleCollider.material = slippyPhysics;
    }

    /// <summary>
    /// Рассчитывает расстояние до поверхности земли под персонажем.
    /// Использует RayCast и SphereCast для точного определения дистанции.
    /// Обновляет значение <c>groundDistance</c>.
    /// </summary>
    protected virtual void CheckGroundDistance()
    {
        if (_capsuleCollider != null)
        {
            float radius = _capsuleCollider.radius * 0.9f;
            var dist = 10f;

            Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);

            if (Physics.Raycast(ray2, out groundHit, (colliderHeight / 2) + dist, groundLayer) && !groundHit.collider.isTrigger)
                dist = transform.position.y - groundHit.point.y;

            if (dist >= groundMinDistance)
            {
                Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                Ray ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out groundHit, _capsuleCollider.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
                {
                    Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
                    float newDist = transform.position.y - groundHit.point.y;
                    if (dist > newDist) dist = newDist;
                }
            }
            groundDistance = (float)System.Math.Round(dist, 2);
        }
    }

    /// <summary>
    /// Возвращает угол наклона поверхности под персонажем в градусах.
    /// Рассчитывается как угол между нормалью поверхности и вектором вверх.
    /// </summary>
    /// <returns>Угол наклона поверхности под персонажем в градусах.</returns>
    public virtual float GroundAngle()
    {
        var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
        return groundAngle;
    }

    /// <summary>
    /// Возвращает угол наклона поверхности относительно направления движения персонажа.
    /// Полезно для расчета скольжения по склону или корректировки движения.
    /// </summary>
    /// <returns>Угол между направлением движения и нормалью поверхности, смещённый на 90 градусов.</returns>
    public virtual float GroundAngleFromDirection()
    {
        var dir = input.magnitude > 0 ? (transform.right * input.x + transform.forward * input.z).normalized : transform.forward;
        var movementAngle = Vector3.Angle(dir, groundHit.normal) - 90;
        return movementAngle;
    }

    #endregion

}
