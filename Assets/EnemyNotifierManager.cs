using UnityEngine;

public class EnemyNotifierManager : MonoBehaviour
{
    EnemyNotifierListener listener;
    EnemyNotifier notifier = new EnemyNotifier();

    public void Init(Transform self, EnemyFOVController fOVController, HumanoidStats stats)
    {
        listener = new EnemyNotifierListener(self, fOVController, stats.statsSO.eventListenDistance);

        EnemyNotifier.NotifyAboutTarget += listener.OnNotify;

    }

    private void OnDisable()
    {
        EnemyNotifier.NotifyAboutTarget -= listener.OnNotify;   
    }

    public void Notify(IDamagable dm)
    {
        notifier.Notify(dm);
    }
}
