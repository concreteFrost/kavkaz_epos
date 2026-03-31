using UnityEngine;

public class EnemyNotifierManager : MonoBehaviour
{
    [SerializeField] NotifierDataSO notifierDataSO;
    private float eventListenDistance;

    EnemyNotifierListener listener;
    EnemyNotifier notifier;

    public void Init(Transform self,EnemyFOVController fov)
    {
        notifier = new EnemyNotifier();
        SetEventListenDistance(notifierDataSO);

        listener = new EnemyNotifierListener(self, fov, eventListenDistance);
        EnemyNotifier.NotifyAboutTarget += listener.OnNotify;

    }

    private void OnDisable()
    {
        EnemyNotifier.NotifyAboutTarget -= listener.OnNotify;
    }

    private void SetEventListenDistance(NotifierDataSO notifierDataSO)
    {
        if (notifierDataSO == null)
        {
            Debug.Log("no notifier data found, event listend distance is set to 20f");
            eventListenDistance = 20f;
        }
        else
        {
            eventListenDistance = notifierDataSO.eventListenDistance;
        }
    }

    public void Notify(IDamagable dm)
    {
        notifier.Notify(dm);
    }
}
