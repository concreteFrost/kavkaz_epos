using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;
    
    private HumanoidAIDamageController damageController;
    private HumanoidAIPushReceiver pushReceiver;

    public EnemyIdleHandler idleHandler;
    public EnemyPatrolHandler patrolHandler;
    public EnemyChaseHandler chaseHandler;  
 
    public EnemyCombatHandler combatHandler; 
    public EnemyWaitForTargetHandler waitForTargetHandler;
    public EnemyStrafeHandler strafeHandler;

    public EnemyPassiveInterruptionHandler passiveInterruptionTracker;


    public void Init(EnemyStateTrackerServices services)
    {
    
        idleHandler = new EnemyIdleHandler(stats);
        patrolHandler = new EnemyPatrolHandler(stats);
        chaseHandler = new EnemyChaseHandler(stats);
       

        this.damageController = services.damageController;
        this.pushReceiver = services.pushReceiver;

        combatHandler = new EnemyCombatHandler(stats, services.stats);
        this.damageController.DamageTaken += combatHandler.OnDamageTaken;

        waitForTargetHandler = new EnemyWaitForTargetHandler(stats);
        this.damageController.DamageTaken += waitForTargetHandler.OnDamageTaken;

        strafeHandler = new EnemyStrafeHandler(stats);
        this.damageController.DamageTaken += strafeHandler.OnDamageTaken;

        passiveInterruptionTracker = new EnemyPassiveInterruptionHandler();
        this.damageController.DamageTaken += passiveInterruptionTracker.OnDamageTaken;

        this.pushReceiver.PushReceived += passiveInterruptionTracker.OnDamageTaken;
     
    }

    private void Update()
    {
        passiveInterruptionTracker.HandleInterruptionUpdate(); 
    }

    private void OnDisable()
    {
        damageController.DamageTaken -=passiveInterruptionTracker.OnDamageTaken;
        damageController.DamageTaken -=combatHandler.OnDamageTaken; 
        damageController.DamageTaken -=waitForTargetHandler.OnDamageTaken;
        damageController.DamageTaken -=strafeHandler.OnDamageTaken;
        pushReceiver.PushReceived -= passiveInterruptionTracker.OnDamageTaken;
    }


}
