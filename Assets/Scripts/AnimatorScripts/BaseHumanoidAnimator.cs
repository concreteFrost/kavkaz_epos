
using UnityEngine;

public abstract class BaseHumanoidAnimator 
{
    protected Animator animator;
    public abstract void UpdateAnimatorParameters();
}

public static partial class AnimatorParameters
{
    public static int InputHorizontal = Animator.StringToHash("InputHorizontal");
    public static int InputVertical = Animator.StringToHash("InputVertical");
    public static int InputMagnitude = Animator.StringToHash("InputMagnitude");
    public static int IsGrounded = Animator.StringToHash("IsGrounded");
    public static int IsStrafing = Animator.StringToHash("IsStrafing");
    public static int IsSprinting = Animator.StringToHash("IsSprinting");
    public static int GroundDistance = Animator.StringToHash("GroundDistance");
    public static int IsWeaponed = Animator.StringToHash("IsWeaponed");
    public static int IsShieldRaised = Animator.StringToHash("IsShieldRaised");
    public static int IsDamaged = Animator.StringToHash("IsDamaged");
    public static int BalancePenalty = Animator.StringToHash("BalancePenalty");
    public static int IsDead = Animator.StringToHash("IsDead");
    public static int IsDodging = Animator.StringToHash("IsDodging");
    public static int IsThrowingWeapon = Animator.StringToHash("IsThrowingWeapon");
    public static int DodgeX = Animator.StringToHash("DodgeX");
    public static int DodgeY = Animator.StringToHash("DodgeY");
    public static int IsClimbing = Animator.StringToHash("IsClimbing");
    public static int Attack = Animator.StringToHash("Attack");
}
