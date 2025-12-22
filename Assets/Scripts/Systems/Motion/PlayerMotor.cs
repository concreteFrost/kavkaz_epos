using UnityEngine;

public class PlayerMotor : HumanoidMotor, ICharacterMovementAnimData
{

    public bool isHighSlope = false;
    public override void Init(HumanoidMotorServices service)
    {
        base.Init(service);
    }

    public override void UpdateMotor(float jumpHeight)
    {
        
        CheckGround();
        ControlJumpBehaviour(jumpHeight);
        AirControl();
        CheckSlopeLimit();  
    }

    public override void MoveCharacter(Vector3 direction)
    {
        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        input.x = direction.x;
        input.z = direction.z;

        // сглаживаем ввод только если можно двигаться
        inputSmooth = Vector3.Lerp(
            inputSmooth,
            input,
            movementSmooth * Time.deltaTime
        );

        // корректное формирование moveDirection
        moveDirection = new Vector3(inputSmooth.x, 0f, inputSmooth.z);

        // горизонтальная скорость
        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        // сохраняем вертикальную скорость
        horizontalVelocity.y = _rigidbody.linearVelocity.y;

        _rigidbody.linearVelocity = horizontalVelocity;
    }

    public void StopMovement()
    {
        // обнуляем вход и сглаженный ввод
        input = Vector3.zero;
        inputSmooth = Vector3.zero;
        moveDirection = Vector3.zero;

        //// горизонтальная скорость
        Vector3 velocity = _rigidbody.linearVelocity;
        velocity.x = 0f;
        velocity.z = 0f;

        //// сохраняем вертикальную скорость (для падения/гравитации)
        _rigidbody.linearVelocity = velocity;
    }


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
                    isHighSlope = true;
                    return;
                }
            }
        }
        isHighSlope = false;
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

        if (isGrounded) return;

        //return if cant move character
        if (transform.position.y > heightReached) heightReached = transform.position.y;


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