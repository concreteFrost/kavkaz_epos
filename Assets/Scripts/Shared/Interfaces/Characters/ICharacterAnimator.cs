using UnityEngine;

public interface ICharacterAnimator
{

    public float InputMagnitude { get; set; }

    public Vector3 MoveDirection { get; set; }

    public float VerticalSpeed {  get; set; }   

    public float HorizontalSpeed { get; set; }

    public float AnimationSmooth {  get; set; }
    public float GroundDistance {  get; set; }  

    public bool StopMove {  get; set; } 
    public bool IsSprinting { get; set; }   

    public bool IsJumping { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsAttacking {  get; set; }
    public bool IsWeaponed {  get; set; }

    public int AttackIndex {  get; set; }
    public int WeaponIndex {  get; set; }

    public bool IsShieldRaised { get; set; }




}
