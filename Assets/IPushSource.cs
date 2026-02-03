internal interface IPushSource
{
    void PerformPush();
    void CancelPush();

    AnimationInfoSO AnimationData();   
}