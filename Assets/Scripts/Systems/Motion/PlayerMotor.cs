using UnityEngine;

public class PlayerMotor : HumanoidMotor, IHumanoidMovementAnimData
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

    #endregion

    #region Jump Methods

    public override void Jump(float jumpTimer)
    {
        base.Jump(jumpTimer);   
    }

    protected override void ControlJumpBehaviour(float jumpHeight)
    {
       base.ControlJumpBehaviour(jumpHeight);   
    }

    public override void AirControl()
    {
      base.AirControl();    
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
                    isHighSlope = true;
                    return;
                }
            }
        }
        isHighSlope = false;
    }

    #endregion

}