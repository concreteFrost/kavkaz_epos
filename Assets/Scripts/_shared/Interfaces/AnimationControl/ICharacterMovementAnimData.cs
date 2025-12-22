using UnityEngine;

public interface ICharacterMovementAnimData
{
    float InputMagnitude { get; }
    Vector3 MoveDirection { get; }
    float VerticalSpeed { get; }
    float HorizontalSpeed { get; }
    bool BlockRotation { get; set; } 
    bool ApplyRootMotion { get; set; }

    bool IsSprinting { get; }
    bool StopMove { get; set; }
    float AnimationSmooth {  get; }
    public bool IsLockedOnTarget { get; set; }

    public Vector3 GetInverseTransformDirection();

    bool IsGrounded { get; }
    float GroundDistance { get; }
    bool IsJumping { get; }

    public bool IsDodging { get; set; }
    public float DodgeX {  get; set; }
    public float DodgeY { get; set; }
}
