using UnityEngine;

public interface ICharacterAnimator
{
    public Vector3 GetInverseTransformDirection();

    //movement
    public float InputMagnitude { get; set; }
    public Vector3 MoveDirection { get; set; }
    public float VerticalSpeed {  get; set; }   
    public float HorizontalSpeed { get; set; }
    public bool IsSprinting { get; set; }
    public bool StopMove { get; set; }
    //animation
    public float AnimationSmooth {  get; set; }
    //air
    public bool IsGrounded { get; set; }
    public float GroundDistance {  get; set; }
    public bool IsJumping { get; set; }
    //rotation
    public bool IsLockedOnTarget {  get; set; }
    //combat
    public bool IsAttacking {  get; set; }
    public bool IsWeaponed {  get; set; }
    public int AttackIndex {  get; set; }
    public int WeaponIndex {  get; set; }
    public bool IsShieldRaised { get; set; }
    public bool IsDodging { get; set; }
    //damage
    public bool IsDamaged { get; set; }
    public float BalancePenalty { get; set; }
    public bool IsDead { get; set; }

}
