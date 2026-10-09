using UnityEngine;

public interface IHumanoidMovement
{
    bool StopMove { get; set; }
    float InputMagnitude { get; }
    float VerticalSpeed { get; }
    float HorizontalSpeed { get; }
    bool BlockRotation { get; set; } 
    bool IsSprinting { get; set; }
    bool IsStrafing { get; set; }
    
    float AnimationSmooth {  get; }
    bool IsGrounded { get; set; }
    float GroundDistance { get; }
    bool IsJumping { get; }

    bool IsTurning { get; set; }    
    public bool IsDodging { get; set; }
    public float DodgeX {  get; set; }
    public float DodgeY { get; set; }

}
