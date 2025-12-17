using UnityEngine;

public interface ICharacterMovementAnimData
{
    float InputMagnitude { get; }
    Vector3 MoveDirection { get; }
    float VerticalSpeed { get; }
    float HorizontalSpeed { get; }
    bool IsSprinting { get; }
    bool StopMove { get; }
    float AnimationSmooth {  get; }
    public bool IsLockedOnTarget {  get; }

    public Vector3 GetInverseTransformDirection();

    bool IsGrounded { get; }
    float GroundDistance { get; }
    bool IsJumping { get; }
}
