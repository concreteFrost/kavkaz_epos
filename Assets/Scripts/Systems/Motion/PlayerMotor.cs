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