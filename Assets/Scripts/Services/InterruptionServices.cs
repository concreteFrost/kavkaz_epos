using UnityEngine;

public class EnemyInterruptionServices {
    
    public HumanoidAIDamageController damageController;
    public HumanoidAIPushReceiver pushReceiver;

    public EnemyInterruptionServices(HumanoidAIDamageController damageController, HumanoidAIPushReceiver pushReceiver)
    {
        this.damageController = damageController;
        this.pushReceiver = pushReceiver;
    }
}
