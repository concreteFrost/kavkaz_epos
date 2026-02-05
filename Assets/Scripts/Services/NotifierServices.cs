using UnityEngine;

public class EnemyNotifierServices
{
    public Transform self;
    public EnemyFOVController fov;

    public EnemyNotifierServices(Transform self, EnemyFOVController fov)
    {
        this.self = self;
        this.fov = fov;
    }
}
