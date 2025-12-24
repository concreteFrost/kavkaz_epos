using UnityEngine;

public class PlayerClimbing : MonoBehaviour, IClimber
{
    public ClimbDetector climbDetector;
    private PlayerMotor motor;
    private PlayerActionGuards actionGuards;

    #region Climbing
    public bool IsClimbing { get; set; }

    public void Init(PlayerMotor motor, PlayerActionGuards actionGuards)
    {
        this.motor = motor;
        this.actionGuards = actionGuards;
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
