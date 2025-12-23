using UnityEngine;
public interface IClimber
{
    bool IsClimbing { get; set; }
    Vector3 ClimbNormal { get; set; } // используется для подтягивания персонажа вперед по оси z  
    void EnterClimb(Vector3 normal);
    void ExitClimb();

}
