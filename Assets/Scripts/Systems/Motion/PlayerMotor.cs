using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerMotor : MonoBehaviour, ICharacterMovementAnimData
{
    #region Inspector Variables

    [Header("camera rotation")]
    [Tooltip("Rotation speed of the character")]
    public float rotationSpeed = 8f;

    [Header("animator smooth speed")]
    [Range(1f, 20f)]
    public float movementSmooth = 6f;
    [Range(0f, 1f)]
    public float animationSmooth = 0.2f;

    [Header("- Airborne")]
    [Tooltip("Use the currently Rigidbody Velocity to influence on the Jump Distance")]
    public bool jumpWithRigidbodyForce = false;
    [Tooltip("Rotate or not while airborne")]
    public bool jumpAndRotate = true;

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

    [Header("- Root Motion")]
    [Tooltip("Проверяет дистанцию до обьекта во время использования applyRootMotion")]
    public float distanceToObstacle;

    #endregion

    #region Components

    internal Animator animator;
    internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
    internal PhysicsMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;         // create PhysicMaterial for the Rigidbody
    internal CapsuleCollider _capsuleCollider;                                          // access CapsuleCollider information

    #endregion

    #region Internal Variables

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

    internal bool stopMove;
    internal bool isSprinting;
    internal bool isJumping;
    internal bool isGrounded = true;
    internal bool isLockedOnTarget;
    internal bool isDodging;
    internal bool isHanging;

    internal float attackSlow = 1f;// used to know the direction you're moving 

    #endregion

    #region IPlayerAnimData
    public Vector3 GetInverseTransformDirection() => transform.InverseTransformDirection(moveDirection);
    public Vector3 MoveDirection { get => moveDirection; }
    public float AnimationSmooth { get => animationSmooth; }
    public float InputMagnitude { get => inputMagnitude;  }
    public float VerticalSpeed { get=>verticalSpeed ;  }
    public float HorizontalSpeed { get=>horizontalSpeed; }
    public bool IsLockedOnTarget { get => isLockedOnTarget; set => isLockedOnTarget = value; }
    public float GroundDistance { get=>groundDistance; }
    public bool StopMove { get=>stopMove; }

    public bool ApplyRootMotion { get; set; }

    public bool IsSprinting { get => isSprinting; }
    public bool IsJumping { get => isJumping; }
    public bool IsGrounded { get => isGrounded; }
    public bool IsDodging { get => isDodging; set => isDodging = value; }
    public float DodgeX { get => dodgeX; set => dodgeX = value; }
    public float DodgeY { get => dodgeY; set => dodgeY = value; }
    #endregion

    public void Init(PlayerControllerServiceProvider service)
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

        isGrounded = true;
        isSprinting = true;
    }

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            
            _rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);

            RaycastHit hit;

            //центр игрока
            var center = transform.TransformPoint(colliderCenter);

            //Если есть приграда то игнорировать движение вперед
            if (Physics.Raycast(center,_rigidbody.transform.forward,out hit, distanceToObstacle))
            {
                return;
            }

            _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);

        }

    }

    private void Update()
    {
        moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
    }


    public virtual void UpdateMotor(float jumpHeight)
    {
        CheckGround();
        CheckSlopeLimit();
        ControlJumpBehaviour(jumpHeight);
        AirControl();
    }
    

    #region Locomotion

    /// <summary>
    /// Обновляет анимацию ДВИЖЕНИЯ
    /// </summary>
    public void UpdateAnimatorLocomotion()
    {

        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
        verticalSpeed = relativeInput.z;
        horizontalSpeed = relativeInput.x;

        var newInput = new Vector2(verticalSpeed,horizontalSpeed);

        inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? AnimatorConsts.runningSpeed : AnimatorConsts.walkSpeed);
    }


    public virtual void MoveCharacter(Vector3 direction)
    {
        // сглаживаем ввод
        inputSmooth = Vector3.Lerp(
            inputSmooth,
            input,
            movementSmooth * Time.deltaTime
        );

        direction.y = 0f;
        direction = Vector3.ClampMagnitude(direction, 1f);

        Vector3 velocity = direction * moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;

        _rigidbody.linearVelocity = velocity;
    }

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

    #region Rotation
    public virtual void RotateToDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, 0.1f);
        transform.rotation = Quaternion.LookRotation(desiredForward);
    }

    public virtual void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 lookDir = targetPosition - transform.position;
        lookDir.y = 0f;
        
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }


    #endregion

    #region Slope Check
    public virtual void CheckSlopeLimit()
    {
        if (input.sqrMagnitude < 0.1) return;

        RaycastHit hitinfo;
        var hitAngle = 0f;

        if (Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), transform.position + moveDirection.normalized * (_capsuleCollider.radius + 0.2f), out hitinfo, groundLayer))
        {
            hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

            var targetPoint = hitinfo.point + moveDirection.normalized * _capsuleCollider.radius;
            if ((hitAngle > slopeLimit) && Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), targetPoint, out hitinfo, groundLayer))
            {
                hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                if (hitAngle > slopeLimit && hitAngle < 85f)
                {
                    stopMove = true;
                    return;
                }
            }
        }
        stopMove = false;
    }

    #endregion

    #region Jump Methods

    public void Jump(float jumpTimer)
    {

        jumpCounter = jumpTimer;
        isJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

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

    public virtual void AirControl()
    {
        //return if cant move character
        if (transform.position.y > heightReached) heightReached = transform.position.y;
        inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * Time.deltaTime);

        if (jumpWithRigidbodyForce && !isGrounded)
        {
            _rigidbody.AddForce(moveDirection * airSpeed * Time.deltaTime, ForceMode.VelocityChange);
            return;
        }

        moveDirection.y = 0;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

        Vector3 targetPosition = _rigidbody.position + (moveDirection * airSpeed) * Time.deltaTime;
        Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

        targetVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = Vector3.Lerp(_rigidbody.linearVelocity, targetVelocity, airSmooth * Time.deltaTime);
    }

    #endregion

    #region Ground Check                

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
                // set IsGrounded to false 
                isGrounded = false;
                // check vertical velocity
                verticalVelocity = _rigidbody.linearVelocity.y;
                // apply extra gravity when falling
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

    protected virtual void ControlMaterialPhysics()
    {
        // change the physics material to very slip when not grounded
        _capsuleCollider.material = (isGrounded && GroundAngle() <= slopeLimit + 1) ? frictionPhysics : slippyPhysics;

        if (IsGrounded && input == Vector3.zero)
            _capsuleCollider.material = maxFrictionPhysics;
        else if (IsGrounded && input != Vector3.zero)
            _capsuleCollider.material = frictionPhysics;
        else
            _capsuleCollider.material = slippyPhysics;
    }

    protected virtual void CheckGroundDistance()
    {
        if (_capsuleCollider != null)
        {
            // radius of the SphereCast
            float radius = _capsuleCollider.radius * 0.9f;
            var dist = 10f;
            // ray for RayCast
            Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
            // raycast for check the ground distance
            if (Physics.Raycast(ray2, out groundHit, (colliderHeight / 2) + dist, groundLayer) && !groundHit.collider.isTrigger)
                dist = transform.position.y - groundHit.point.y;
            // sphere cast around the base of the capsule to check the ground distance
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

    public virtual float GroundAngle()
    {
        var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
        return groundAngle;
    }

    public virtual float GroundAngleFromDirection()
    {
        var dir = input.magnitude > 0 ? (transform.right * input.x + transform.forward * input.z).normalized : transform.forward;
        var movementAngle = Vector3.Angle(dir, groundHit.normal) - 90;
        return movementAngle;
    }

    #endregion



}