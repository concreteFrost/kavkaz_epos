using UnityEngine;

public class PlayerClimbing : MonoBehaviour, IClimber
{
    public ClimbDetector climbDetector;
    private PlayerMotor motor;
    private PlayerActionGuards actionGuards;
    private PlayerAnimatorController animatorController;

    #region Climbing
    public bool IsClimbing { get; set; }

    public void Init(PlayerMotor motor, PlayerActionGuards actionGuards, PlayerAnimatorController animatorController)
    {
       
        this.motor = motor;
        this.actionGuards = actionGuards;
        this.animatorController = animatorController;
       
    }

    public void TryToClimb()
    {
        if (climbDetector.TryGetClimbable(out var surface, out var hit))
        {
            //Debug.Log(hit.normal);
            //var allowedVector1= new Vector3(0,0,1);
            //var allowedVector2 = new Vector3(0,0,-1);

            //if (hit.normal != allowedVector1 && hit.normal != allowedVector2)
            //    return;

            EnterClimb(hit.normal);
        }
    }

    public void EnterClimb(Vector3 normal)
    {
        actionGuards.SetMode(PlayerMode.Climbing);
        animatorController.PlayClipCrossFade(AnimatorParameters.climbUpClip);
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