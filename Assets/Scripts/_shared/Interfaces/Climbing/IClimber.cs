using UnityEngine;
public interface IClimber
{
    bool IsClimbing { get; set; }
    void TryToClimb();
    void EnterClimb(Vector3 normal);
    void ExitClimb();


}
