using UnityEngine;

public class EnemyDamageHandler
{
    EnemyBrainContext context;

    public EnemyDamageHandler(EnemyBrainContext context, float duration = 1f)
    {
        this.context = context;
        context.damageController.DamageTaken += OnDamageTaken;
    }

    public void Dispose()
    {
        context.damageController.DamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(Transform attackSource)
    {
        var tracker = context.stateTracker;
        //combat
        tracker.RegisterDamage();

        //wait 
        tracker.InterruptWait();

        tracker.InterruptStrafeState(); 

       
        if (attackSource == null) return;

        //idle, patrol
        tracker.Interrupt(attackSource.position); 
 
    }


}
