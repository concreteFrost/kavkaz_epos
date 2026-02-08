using UnityEngine;

public class EnemyNotifierListener 
{

    protected EnemyFOVController fov;
    Transform self;

    private float listenDistance;

    public EnemyNotifierListener(Transform self, EnemyFOVController fov, float listenDistance)
    {

        this.self = self;
        this.fov = fov;
        this.listenDistance = listenDistance;
    }

    public void OnNotify(IDamagable target)
    {
       
        //if (stateMachine.CurrentState == null) return;
        if (fov.currentTarget != null)
        {     
            return;
        }
        
        float distance = Vector3.Distance(self.position, target.GetOrigin().position);

        if (distance > listenDistance) return;

        fov.SetLockedTarget(target);

       
    }
}
