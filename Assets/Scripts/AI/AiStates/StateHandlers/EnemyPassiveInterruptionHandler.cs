using UnityEngine;

[System.Serializable]
public class EnemyPassiveInterruptionHandler
{

    Transform self;
    //private EnemyBrain brain;

    //private AIState<EnemyBrainContext> moveToInterruptor;

    EnemyFOVController fOVController;

    public void Init(Transform self, EnemyFOVController fOVController)
    {
        this.self = self;
        this.fOVController = fOVController;

        //moveToInterruptor.Init(brain.GetContext());
    }

    public void OnDamageTaken(IAttackSource attackSource)
    {

        if (attackSource == null) return;

        ReactOnDamage(attackSource);


    }

    /// <summary>
    /// Определяет реакцию на отвлечение. 
    /// </summary>
    /// <param name="selfPosition"></param>
    /// <param name="anim"></param>
    /// <returns></returns>
    public void ReactOnDamage(IAttackSource src)
    {

        if (fOVController.currentTarget != null) return;
        var dmg = fOVController.TryGetDamagable(src.Source());

        if (dmg == null)
        {
            Debug.Log("no damagable interruptor detected");
            return;

        }
        fOVController.SetLockedTarget(dmg);





    }

}
