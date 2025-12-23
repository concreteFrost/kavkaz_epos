using UnityEngine;

public class PlayerClimbing : MonoBehaviour , IClimber
{
    public ClimbDetector climbDetector;
    
    #region Climbing
    public bool IsClimbing { get; set; }
    public Vector3 ClimbNormal {  get; set; }   

    public void EnterClimb(Vector3 normal)
    {
        IsClimbing = true;
        ClimbNormal = normal;   
    }

    public void ExitClimb()
    {
        IsClimbing = false; 
        ClimbNormal = Vector3.zero; 
    }

    #endregion
}
