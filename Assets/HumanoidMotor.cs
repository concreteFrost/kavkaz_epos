using System;
using UnityEngine;
public abstract class HumanoidMotor : MonoBehaviour, ICharacterMovementAnimData
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

    [Header("- Root Motion")]
    [Tooltip("Проверяет дистанцию до обьекта во время использования applyRootMotion")]
    public float distanceToObstacle;

    #region Components

    internal Animator animator;
    internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
    internal PhysicsMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;         // create PhysicMaterial for the Rigidbody
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
    internal bool stopMove;


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
    public bool StopMove { get => stopMove; set => stopMove = value; }
    public bool ApplyRootMotion { get; set; }
    public bool IsSprinting { get => isSprinting; }
    public bool IsJumping { get => isJumping; }
    public bool IsGrounded { get => isGrounded; }
    public bool IsDodging { get => isDodging; set => isDodging = value; }
    public float DodgeX { get => dodgeX; set => dodgeX = value; }
    public float DodgeY { get => dodgeY; set => dodgeY = value; }
    #endregion

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {

            _rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);

            RaycastHit hit;

            //центр игрока
            var center = transform.TransformPoint(colliderCenter);

            //Если есть приграда то игнорировать движение вперед
            if (Physics.Raycast(center, _rigidbody.transform.forward, out hit, distanceToObstacle))
            {
                return;
            }

            _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);

        }

    }

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

        isGrounded = true;
        isSprinting = true;
    }

    #region Abstract Methods

    public abstract void UpdateMotor(float jumpHeight);
    public abstract void MoveCharacter(Vector3 direction);

    #endregion

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



    #region Dodge
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

        float finalRotation = isRotationBlocked ? blockedRotationSpeed : rotationSpeed;

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, finalRotation * Time.deltaTime, 0.1f);
        transform.rotation = Quaternion.LookRotation(desiredForward);
    }

    public virtual void RotateToTarget(Vector3 targetPosition )
    {
        Vector3 lookDir = targetPosition - transform.position;
        lookDir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        float finalRotation = isRotationBlocked ? blockedRotationSpeed : rotationSpeed;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, finalRotation * Time.deltaTime);
    }


    #endregion

   



   



}
