using System;
using UnityEngine;

public class EnemyNotifier
{
    public static Action<IDamagable> NotifyAboutTarget;

    public void Notify(IDamagable target)
    {
        NotifyAboutTarget?.Invoke(target);  
    }
}
