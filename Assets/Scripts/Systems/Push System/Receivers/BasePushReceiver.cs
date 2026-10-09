using System;
using UnityEngine;

public abstract class BasePushReceiver : MonoBehaviour, IPushable
{
    protected CharacterType characterType;
    protected Transform self;
    public CharacterType CharacterType() => characterType;

    public abstract void CancelPush();

    public abstract void GetPushed(PushDirection dir, IAttackSource source);

    public abstract void TrackPush();

    public Transform Origin() => self;


    public bool IsPushed { get;  set; }

    public event Action<IAttackSource> PushReceived;

    protected void InvokePushReceived(IAttackSource source)
    {
        PushReceived?.Invoke(source);
    }
}
