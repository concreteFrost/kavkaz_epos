using UnityEngine;

public class InterruptionManager : MonoBehaviour
{

    [HideInInspector] public EnemyPassiveInterruptionHandler passiveInterruptionHandler;
    private HumanoidAIDamageController damageController;
    private HumanoidAIPushReceiver pushReceiver;

    public void Init(EnemyInterruptionServices services)
    {
        this.damageController = services.damageController;
        this.pushReceiver = services.pushReceiver;

        passiveInterruptionHandler = new EnemyPassiveInterruptionHandler();
        
        this.damageController.DamageTaken += passiveInterruptionHandler.OnDamageTaken;
        this.pushReceiver.PushReceived += passiveInterruptionHandler.OnDamageTaken;

    }

    private void Update()
    {
        passiveInterruptionHandler.HandleInterruptionUpdate();
    }

    private void OnDisable()
    {
        damageController.DamageTaken -= passiveInterruptionHandler.OnDamageTaken;
        pushReceiver.PushReceived -= passiveInterruptionHandler.OnDamageTaken;
    }

}
