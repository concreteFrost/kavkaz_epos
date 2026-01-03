using UnityEngine;

public class HumanoidAIAnimator : BaseHumanoidAnimator
{
    IHumanoidMovement movement;

    public void Init(IHumanoidMovement movement, Animator anim)
    {
        this.movement = movement;   
        this.animator = anim;
    }

    public override void UpdateAnimatorParameters()
    {
        if (animator == null || !animator.enabled)
        {
            Debug.Log("no animator found");
            return;
        }

        UpdateLocomotionState(movement);
    }
}
