public interface IPushSource
{
    void PerformPush();
    void CancelPush();
    void TriggerPushAnimation();
    AnimationInfoSO AnimationData();   
}