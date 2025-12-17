using UnityEngine;

public class PlayerMotor : MonoBehaviour, ICharacterAnimator
{

    #region Inspector Variables

    [Header("camera rotation")]
    [Tooltip("Rotation speed of the character")]
    public float rotationSpeed = 16f;
    [Tooltip("Rotate with the Camera forward when standing idle")]
    public bool rotateWithCamera = false;

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

    #endregion

    #region Components

    internal Animator animator;
    internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
    internal PhysicsMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;         // create PhysicMaterial for the Rigidbody
    internal CapsuleCollider _capsuleCollider;                                          // access CapsuleCollider information

    #endregion

    #region Internal Variables

    internal PlayerStats playerStats;
    internal PlayerStatsModifier playerStatsModifer;
    
    internal float moveSpeed;                           // set the current moveSpeed for the MoveCharacter method
    internal float verticalVelocity;                    // set the vertical velocity of the rigidbody
    internal float colliderRadius, colliderHeight;      // storage capsule collider extra information        
    internal float heightReached;                       // max height that character reached in air;
    internal float jumpCounter;                         // used to count the routine to reset the jump
    internal RaycastHit groundHit;                      // raycast to hit the ground 

    protected Transform rotateTarget;                    // used as a generic reference for the camera.transform
    internal Vector3 input;                             // generate raw input for the controller
    internal Vector3 colliderCenter;                    // storage the center of the capsule collider info                
    internal Vector3 inputSmooth;                       // generate smooth input based on the inputSmooth value       
    internal Vector3 moveDirection;

    private float attackSlow = 1f;// used to know the direction you're moving 

    #endregion

    #region ICharacterAnimator
    public Vector3 GetInverseTransformDirection() => transform.InverseTransformDirection(moveDirection);
    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float AnimationSmooth { get => animationSmooth; set => animationSmooth = value; }
    public float InputMagnitude { get ; set ; }
    public float VerticalSpeed { get ; set ; }
    public float HorizontalSpeed { get; set; }
    public float GroundDistance { get; set; }
    public bool IsLockedOnTarget { get; set; }
    public bool IsDodging { get; set; }   
    public bool StopMove { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsJumping { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsAttacking { get; set ; }
    public bool IsWeaponed { get ; set ; }
    public int AttackIndex { get; set; }
    public int WeaponIndex { get; set; }
    public bool IsShieldRaised { get; set; }
    public bool IsDamaged { get; set; }
    public float BalancePenalty { get; set; }
    public bool IsDead { get; set; }
    #endregion

    public void Init(PlayerControllerServiceProvider service)
    {
        animator = service.animator;
        playerStats = service.stats;
        playerStatsModifer = service.statsModifier;

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

        IsGrounded = true;
        IsSprinting = true;
    }

    public virtual void UpdateMotor()
    {
        CheckGround();
        CheckSlopeLimit();
        ControlJumpBehaviour();
        AirControl();
    }


    private void OnAnimatorMove()
    {

        if (animator.applyRootMotion)
        {
            _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);
            _rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);
        }

    }

    #region Locomotion

    public virtual void UpdateMoveDirection()
    {
        moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
    }

    public virtual void ControlLocomotionType()
    {
        SetControllerMoveSpeed(playerStats);
        MoveCharacter(moveDirection);
    }

    public virtual void SetControllerMoveSpeed(CharacterStats stats)
    {
        moveSpeed = Mathf.Lerp(moveSpeed, IsSprinting ? stats.runningSpeed : stats.walkSpeed, movementSmooth * Time.deltaTime);
    }

    public virtual void MoveCharacter(Vector3 _direction)
    {
        // calculate input smooth
        inputSmooth = Vector3.Lerp(inputSmooth, input, (movementSmooth) * Time.deltaTime);

        if (!IsGrounded || IsJumping) return;

        _direction.y = 0;
        _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
        _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
        // limit the input
        if (_direction.magnitude > 1f)
            _direction.Normalize();


        Vector3 targetPosition = _rigidbody.position + FinalDirection(_direction);
        Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

        bool useVerticalVelocity = true;
        if (useVerticalVelocity) targetVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = targetVelocity;
    }

    private Vector3 FinalDirection(Vector3 _direction)
    {
        // если атакуем или ранены — плавно уменьшаем скорость
        float target = (StopMove || IsAttacking || IsDamaged || IsDodging) ? 0f : 1f;
        attackSlow = Mathf.Lerp(attackSlow, target, Time.deltaTime * 10f);

        return _direction * (moveSpeed * attackSlow) * Time.deltaTime;
    }

    #endregion

    #region Rotation
    public virtual void ControlRotationType()
    {
        Vector3 dir = (rotateWithCamera && input == Vector3.zero) && rotateTarget ? rotateTarget.forward : moveDirection;
        RotateToDirection(dir);
    }

    public virtual void RotateToDirection(Vector3 direction)
    {
        if (IsDamaged) return;
       
        if (rotateTarget != null)
        {
            RotateToTarget();
            return;
        }

        // обычная логика если нет lock-on
        RotateToDirection(direction, rotationSpeed);
    }


    public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
    {

        if (!jumpAndRotate && !IsGrounded) return;
        direction.y = 0f;
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, .1f);
        Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
        transform.rotation = _newRotation;
    }

    private void RotateToTarget()
    {
        Vector3 lookDir = rotateTarget.position - transform.position;
        lookDir.y = 0; // чтобы не задирал голову

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
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
                    StopMove = true;
                    return;
                }
            }
        }
        StopMove = false;
    }

    #endregion

    #region Jump Methods

    protected virtual void ControlJumpBehaviour()
    {
        if (!IsJumping) return;

        jumpCounter -= Time.deltaTime;
        if (jumpCounter <= 0)
        {
            jumpCounter = 0;
            IsJumping = false;
        }
        // apply extra force to the jump height   
        var vel = _rigidbody.linearVelocity;
        vel.y = playerStats.jumpHeight;
        _rigidbody.linearVelocity = vel;
    }

    public virtual void AirControl()
    {
        if (IsGrounded && !IsJumping) return;
        if (transform.position.y > heightReached) heightReached = transform.position.y;
        inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * Time.deltaTime);

        if (jumpWithRigidbodyForce && !IsGrounded)
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

        if (GroundDistance <= groundMinDistance)
        {
            IsGrounded = true;
            if (!IsJumping && GroundDistance > 0.05f)
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);

            heightReached = transform.position.y;
        }
        else
        {
            if (GroundDistance >= groundMaxDistance)
            {
                // set IsGrounded to false 
                IsGrounded = false;
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
        _capsuleCollider.material = (IsGrounded && GroundAngle() <= slopeLimit + 1) ? frictionPhysics : slippyPhysics;

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
            GroundDistance = (float)System.Math.Round(dist, 2);
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