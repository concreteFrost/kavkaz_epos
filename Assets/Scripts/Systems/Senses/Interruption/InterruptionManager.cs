using UnityEngine;

public class InterruptionManager : MonoBehaviour
{

    [HideInInspector] public EnemyPassiveInterruptionHandler passiveInterruptionHandler;
    private IDamagable damageController;
    private IPushable pushReceiver;

    public void Init(
        IDamagable damageController,
        IPushable pushReceiver

        )
    {
        this.damageController =damageController;
        this.pushReceiver = pushReceiver;

        passiveInterruptionHandler = new EnemyPassiveInterruptionHandler();
        
        this.damageController.DamageTaken += passiveInterruptionHandler.OnDamageTaken;
        this.pushReceiver.PushReceived += passiveInterruptionHandler.OnDamageTaken;

    }

    private void Update()
    {

        passiveInterruptionHandler.HandleInterruptionUpdate();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        damageController.DamageTaken -= passiveInterruptionHandler.OnDamageTaken;
        pushReceiver.PushReceived -= passiveInterruptionHandler.OnDamageTaken;
    }

}
