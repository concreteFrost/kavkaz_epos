using UnityEngine;


public class AnimatorParameters
{

    public const float walkSpeed = 0.5f;
    public const float runningSpeed = 1f;
    public const float sprintSpeed = 1.5f;
    
    public const float transitionSpeed = 0.2f;

    public const int motionLayer = 0;
    public const int armedLayer = 1;
    public const int combatLayer = 2;
    public const int climbLayer = 5;
    public const int damageLayer = 7;

    public const string getUpState= "Get Up";
    public const string getUpFromBellyState = "Get Up From Belly";
    public const string lookAroundState = "Look Around Start";

    public const string getUpClip = "Getup";
    public const string getUpFromBellyClip = "Getup_from_belly";

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
    public static int DodgeX = Animator.StringToHash("DodgeX");
    public static int DodgeY = Animator.StringToHash("DodgeY");

}
