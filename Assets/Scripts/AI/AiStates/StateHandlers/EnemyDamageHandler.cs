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
        if (attackSource == null) return;

        var tracker = context.stateTracker;
        //idle, patrol
        tracker.Interrupt(attackSource.position); 

        //combat

        tracker.RegisterDamage();
    }


}
