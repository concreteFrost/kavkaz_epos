
using UnityEngine;

public abstract class CharacterAnimator : MonoBehaviour 
{
    protected Animator animator;

    public void Init(Animator animator)
    {
        this.animator = animator;    
    }

    public abstract void UpdateAnimator(ICharacterAnimator IAnim);

    public abstract void SetAnimatorMoveSpeed(ICharacterAnimator IAnim);
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
    public static int IsAttacking = Animator.StringToHash("IsAttacking");
    public static int AttackIndex = Animator.StringToHash("AttackIndex");
    public static int WeaponType = Animator.StringToHash("WeaponType");
    public static int HasShield = Animator.StringToHash("HasShield");
    public static int IsShieldRaised = Animator.StringToHash("IsShieldRaised");
    public static int IsDamaged = Animator.StringToHash("IsDamaged");
    public static int BalancePenalty = Animator.StringToHash("BalancePenalty");
    public static int IsDead = Animator.StringToHash("IsDead");
    public static int IsDodging = Animator.StringToHash("IsDodging");
}

