internal interface IPushSource
{
    bool IsPushing { get; set; }

    void PerformPush();
    void CancelPush();

    AnimationInfoSO AnimationData();   
}