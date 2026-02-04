public interface IPushSource
{
    void PerformPush();
    void CancelPush();

    AnimationInfoSO AnimationData();   
}