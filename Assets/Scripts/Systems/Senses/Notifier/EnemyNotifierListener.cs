using UnityEngine;

public class EnemyNotifierListener 
{

    protected EnemyFOVController fov;
    Transform self;

    private float listenDistance;
    IDamagable damageController;

    public EnemyNotifierListener(Transform self,IDamagable damageController, EnemyFOVController fov, float listenDistance)
    {
       
        this.self = self;
        this.damageController = damageController;   
        this.fov = fov;
        this.listenDistance = listenDistance;
    }

    public void OnNotify(IDamagable target)
    {
        if (damageController.IsDead) return;

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
