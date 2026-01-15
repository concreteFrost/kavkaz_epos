using UnityEngine;

public interface IHumanoidMovement
{
    bool StopMove { get; set; }
    float InputMagnitude { get; }
    Vector3 MoveDirection { get; }
    float VerticalSpeed { get; }
    float HorizontalSpeed { get; }
    bool BlockRotation { get; set; } 
    bool ApplyRootMotion { get; set; }

    bool IsSprinting { get; set; }

    bool IsStrafing { get; set; }
    
    float AnimationSmooth {  get; }
    public Vector3 GetInverseTransformDirection();
    bool IsGrounded { get; }
    float GroundDistance { get; }
    bool IsJumping { get; }

    public bool IsDodging { get; set; }
    public float DodgeX {  get; set; }
    public float DodgeY { get; set; }
}
