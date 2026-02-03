using UnityEngine;

public abstract class PushReceiver : MonoBehaviour, IPushable
{
    protected CharacterType characterType;
    protected Transform self;
    public CharacterType CharacterType() => characterType;

    public abstract void CancelPush();

    public abstract void GetPushed(PushDirection dir, Vector3 aimingSpot);

    public abstract void TrackPush();

    public Transform Origin() => self;


    public bool IsPushed { get;  set; }  
}
