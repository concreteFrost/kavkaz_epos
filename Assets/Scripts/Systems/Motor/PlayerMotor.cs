using UnityEngine;

public class PlayerMotor : BaseHumanoidMotor
{
    internal PhysicsMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics, hangPhysics;         // create PhysicMaterial for the Rigidbody
    internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
    internal CapsuleCollider _capsuleCollider;                                          // access CapsuleCollider information

    [Tooltip("Максимальный угол поверхности для ходьбы")]
    [Range(30, 80)] public float slopeLimit = 45f;
    internal bool isHighSlope = false; //предотвращает движение если угол наклона выше

    public void Init(BaseHumanoidAnimatorController animatorController)
    {
        this.animator = animatorController;   
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

    public override void UseRootMotion()
    {
        Debug.Log("using root motion");
        //_rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);
        //_rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);
    }

    public override void UseRootMotionWithObstacles()
    {

        Debug.Log("using root motion with obstacles");
        //_rigidbody.MoveRotation(animator.deltaRotation * _rigidbody.rotation);

        //RaycastHit hit;

        ////центр игрока
        //var center = transform.TransformPoint(colliderCenter);

        ////Если есть приграда то игнорировать движение вперед
        //if (!Physics.Raycast(center, _rigidbody.transform.forward, out hit, distanceToObstacle))
        //{
        //    _rigidbody.MovePosition(_rigidbody.position + animator.deltaPosition);
        //}
    }

    public override void UpdateMotor(float jumpHeight)
    {
        
        CheckGround();
        ControlJumpBehaviour(jumpHeight);
        AirControl();
    }

    #region Movement

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

    public override void StopMovement()
    {
       
        // обнуляем вход и сглаженный ввод
        //input = Vector3.zero;
        inputSmooth = Vector3.zero;
        moveDirection = Vector3.zero;

        //// горизонтальная скорость
        Vector3 velocity = _rigidbody.linearVelocity;
        velocity.x = 0f;
        velocity.z = 0f;

        //// сохраняем вертикальную скорость (для падения/гравитации)
        _rigidbody.linearVelocity = velocity;
    }

    public override void SetStrafe(bool isStrafing)
    {
        base.isStrafing = isStrafing;   
    }


    #endregion

    #region Jump Methods

    public override void Jump(float jumpTimer)
    {
        base.Jump(jumpTimer);
    }

    protected override void ControlJumpBehaviour(float jumpHeight)
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

    public override void AirControl()
    {
        if (isGrounded) return;

        Vector3 input = moveDirection;
        input.y = 0;

        if (input.sqrMagnitude < 0.01f) return;

        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 horizontalVel = new Vector3(velocity.x, 0, velocity.z);

        float projectedSpeed = Vector3.Dot(horizontalVel, input);

        float addSpeed = airSpeed - projectedSpeed;
        if (addSpeed <= 0) return;

        float accelSpeed = airAcceleration * Time.deltaTime;
        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        _rigidbody.AddForce(input * accelSpeed, ForceMode.VelocityChange);
    }
    #endregion

    #region GroundCheck
    protected override void CheckGround()
    {
        CheckGroundDistance();

        if (groundDistance <= groundMinDistance)
        {
            isGrounded = true;
            //if (!IsJumping && groundDistance > 0.05f)
              if (!IsJumping && groundDistance > 0.1f)
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
        ControlMaterialPhysics();
    }

    protected virtual void ControlMaterialPhysics()
    {
        if (!IsGrounded || groundHit.collider == null || GroundAngle() > slopeLimit)
        {
            _capsuleCollider.material = slippyPhysics;
            return;
        }

        _capsuleCollider.material = input == Vector3.zero ? maxFrictionPhysics : frictionPhysics;
    }

    protected override void CheckGroundDistance()
    {

        groundDistance = groundMaxDistance;
        groundHit = default;

        if (_capsuleCollider != null)
        {
            float radius = _capsuleCollider.radius * 0.9f;
            var dist = 10f;

            Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);

            if (Physics.Raycast(ray2, out var rayHit, (colliderHeight / 2) + dist,
                groundLayer, QueryTriggerInteraction.Ignore) && Vector3.Angle(rayHit.normal, Vector3.up) <= slopeLimit)
            {
                groundHit = rayHit;
                dist = transform.position.y - rayHit.point.y;
            }

            if (dist >= groundMinDistance)
            {
                Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                Ray ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out var sphereHit, _capsuleCollider.radius + groundMaxDistance,
                    groundLayer, QueryTriggerInteraction.Ignore))
                {
                    // Уточняем нормаль у края, не теряя результат при промахе луча.
                    if (Physics.Linecast(sphereHit.point + (Vector3.up * 0.1f), sphereHit.point + Vector3.down * 0.15f,
                        out var surfaceHit, groundLayer, QueryTriggerInteraction.Ignore))
                    {
                        sphereHit = surfaceHit;
                    }

                    float newDist = transform.position.y - sphereHit.point.y;
                    if (Vector3.Angle(sphereHit.normal, Vector3.up) <= slopeLimit && dist > newDist)
                    {
                        groundHit = sphereHit;
                        dist = newDist;
                    }
                }
            }
            groundDistance = (float)System.Math.Round(dist, 2);
        }
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

    #endregion

    #region Slope Check
    public virtual void CheckSlopeLimit(Vector3 direction)
    {
        isHighSlope = false;
        direction.y = 0f;
        if (direction.sqrMagnitude <= Mathf.Epsilon) return;

        direction.Normalize();

        RaycastHit hitinfo;
        var hitAngle = 0f;

        if (Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), transform.position + direction * (_capsuleCollider.radius + 0.2f), out hitinfo, groundLayer, QueryTriggerInteraction.Ignore))
        {
            hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

            var targetPoint = hitinfo.point + direction * _capsuleCollider.radius;
            if ((hitAngle > slopeLimit) && Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), targetPoint, out hitinfo, groundLayer, QueryTriggerInteraction.Ignore))
            {
                hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                if (hitAngle > slopeLimit)
                {
                    isHighSlope = true;
                    return;
                }
            }
        }
        isHighSlope = false;
    }

    #endregion

    #region Dodge
    public override void Dodge(Vector2 dir)
    {
        base.Dodge(dir);
    }
    #endregion

    #region Attach

    /// <summary>
    /// Прикрепляет тело к цели
    /// </summary>
    /// <param name="normal"></param>
    public void AttachTo(Vector3 normal)
    {

        //поворачиваем игрока в сторону стены
        Quaternion targetRotation = Quaternion.LookRotation(-normal, Vector3.up);
        transform.rotation = targetRotation;
        isGrounded = true;

        Vector3 finalPosition = transform.position;

        transform.position = finalPosition;

        // Отключаем гравитацию, чтобы Root Motion сам управлял движением
        _rigidbody.useGravity = false;
        _rigidbody.Sleep();
    }


    /// <summary>
    /// Открепляет тело от цели и возобновляет поведение rigidbody
    /// </summary>
    public void Detach()
    {
        _rigidbody.WakeUp();
        _rigidbody.useGravity = true;
        //_rigidbody.useGravity = false;
    }
    #endregion

}
