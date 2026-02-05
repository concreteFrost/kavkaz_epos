using UnityEngine;

public class PlayerClimbing : MonoBehaviour, IClimber
{
    public ClimbDetector climbDetector;
    private PlayerMotor motor;
    private PlayerActionGuards actionGuards;
    Animator animator;

    #region Climbing
    public bool IsClimbing { get; set; }

    public void Init(PlayerMotor motor, PlayerActionGuards actionGuards, Animator animator)
    {
        this.motor = motor;
        this.actionGuards = actionGuards;
        this.animator = animator;
    }

    public void TryToClimb()
    {
        if (climbDetector.TryGetClimbable(out var surface, out var hit))
        {
            EnterClimb(hit.normal);
        }
    }

    public void EnterClimb(Vector3 normal)
    {
        actionGuards.SetMode(PlayerMode.Climbing);
        animator.CrossFade("Climb Up", AnimatorParameters.transitionSpeed);
        IsClimbing = true;
        motor.AttachTo(normal);
    }

    public void ExitClimb()
    {
        
        IsClimbing = false; 
        motor.Detach();
        actionGuards.SetMode(PlayerMode.Locomotion);
    }

    #endregion
}
