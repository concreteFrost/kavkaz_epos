using UnityEngine;

public class PlayerPushReceiver : BasePushReceiver
{
    public override void CancelPush()
    {
        IsPushed = false;
        //throw new System.NotImplementedException();
    }

    public override void GetPushed(PushDirection dir, IAttackSource source)
    {
        //throw new System.NotImplementedException();
    }

    public override void TrackPush()
    {
        //throw new System.NotImplementedException();
    }
}
