using UnityEngine;

public class InterruptionManager : MonoBehaviour
{

    private EnemyPassiveInterruptionHandler passiveInterruptionHandler;
    private IDamagable damageController;
    private IPushable pushReceiver;


    //[SerializeField] private AIState<EnemyBrainContext> moveToInterruptor;
    //[SerializeField] private AIState<EnemyBrainContext> interrupted;


    public void Init(
        Transform self,
        IDamagable damageController,
        IPushable pushReceiver,
        EnemyFOVController fOVController

        )
    {
        this.damageController = damageController;
        this.pushReceiver = pushReceiver;

        passiveInterruptionHandler = new EnemyPassiveInterruptionHandler();
        passiveInterruptionHandler.Init(self, fOVController,damageController);

        this.damageController.DamageTaken += passiveInterruptionHandler.OnDamageTaken;
        this.pushReceiver.PushReceived += passiveInterruptionHandler.OnDamageTaken;

    }


    private void OnDisable()
    {
        damageController.DamageTaken -= passiveInterruptionHandler.OnDamageTaken;
        pushReceiver.PushReceived -= passiveInterruptionHandler.OnDamageTaken;
    }

}
