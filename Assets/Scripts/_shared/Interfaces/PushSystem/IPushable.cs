using System;
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
    void GetPushed(PushDirection dir, Transform source);

    void CancelPush();

    void TrackPush();

    Transform Origin();

    event Action<Transform> PushReceived;
}
