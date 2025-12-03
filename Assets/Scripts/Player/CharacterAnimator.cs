
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;

    public void Init(Animator anim)
    {
        animator = anim;    
    }

    public void UpdateAnimator(ICharacterAnimator IAnim)
    {
        
        if (animator == null || !animator.enabled) return;

        animator.SetBool(AnimatorParameters.IsSprinting, IAnim.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, IAnim.IsGrounded);
        animator.SetFloat(AnimatorParameters.GroundDistance, IAnim.GroundDistance);
        animator.SetFloat(AnimatorParameters.InputVertical, IAnim.StopMove ? 0 : IAnim.VerticalSpeed, IAnim.AnimationSmooth, Time.deltaTime);
        animator.SetFloat(AnimatorParameters.InputMagnitude, IAnim.StopMove ? 0f :  IAnim.InputMagnitude, IAnim.AnimationSmooth, Time.deltaTime);

        //animator.SetInteger(AnimatorParameters.AttackIndex)

        //combat control
        animator.SetBool(AnimatorParameters.IsWeaponed, IAnim.IsWeaponed);
        animator.SetBool(AnimatorParameters.IsAttacking, IAnim.IsAttacking);
        animator.SetInteger(AnimatorParameters.AttackIndex, IAnim.AttackIndex);
        animator.SetInteger(AnimatorParameters.WeaponType, IAnim.WeaponIndex);

    }

    public void SetAnimatorMoveSpeed(ICharacterAnimator IAnim)
    {
        Vector3 relativeInput = transform.InverseTransformDirection(IAnim.MoveDirection);
        IAnim.VerticalSpeed = relativeInput.z;
        IAnim.HorizontalSpeed = relativeInput.x;

        var newInput = new Vector2(IAnim.VerticalSpeed, IAnim.HorizontalSpeed);

        //if (isAttacking) //предотвращаем анимацию ходьбы при ударе
        //{
        //    inputMagnitude = Mathf.Lerp(inputMagnitude, 0f, Time.deltaTime * animationStopSmooth);
        //    return;
        //}


        IAnim.InputMagnitude = Mathf.Clamp(newInput.magnitude, 0, IAnim.IsSprinting ? AnimatorConsts.runningSpeed : AnimatorConsts.walkSpeed);
       
    }
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
}

