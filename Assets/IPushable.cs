using UnityEngine;

public enum PushDirection
{
    Back = 0, 
    Forward = 1   
}
public interface IPushable
{
    bool IsPushed { get; set; }
    CharacterType CharacterType();
    void GetPushed(PushDirection dir, Vector3 aimingSpot);

    void CancelPush();

    void TrackPush();

    Transform Origin();
}
