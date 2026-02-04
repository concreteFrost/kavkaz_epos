using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;
    private HumanoidAIDamageController damageController;
    private HumanoidAIPushReceiver pushable;

    public EnemyIdleHandler idleHandler;
    public EnemyPatrolHandler patrolHandler;
    public EnemyChaseHandler chaseHandler;  
    public EnemyPassiveInterruptionHandler passiveInterruptionTracker;
    public EnemyCombatHandler combatHandler; 
    public EnemyWaitForTargetHandler waitForTargetHandler;
    public EnemyStrafeHandler strafeHandler;    

    public void Init(HumanoidAIDamageController damageController,HumanoidAIPushReceiver pushable, HumanoidStats statsInfo)
    {
        this.damageController = damageController;
        this.pushable = pushable;

        idleHandler = new EnemyIdleHandler(stats);
        patrolHandler = new EnemyPatrolHandler(stats);
        chaseHandler = new EnemyChaseHandler(stats);    

        combatHandler = new EnemyCombatHandler(stats, statsInfo);
        this.damageController.DamageTaken += combatHandler.OnDamageTaken;

        waitForTargetHandler = new EnemyWaitForTargetHandler(stats);
        this.damageController.DamageTaken += waitForTargetHandler.OnDamageTaken;

        strafeHandler = new EnemyStrafeHandler(stats);
        this.damageController.DamageTaken += strafeHandler.OnDamageTaken;

        passiveInterruptionTracker = new EnemyPassiveInterruptionHandler();
        this.damageController.DamageTaken += passiveInterruptionTracker.OnDamageTaken;

        this.pushable.PushReceived += passiveInterruptionTracker.OnDamageTaken;

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
        pushable.PushReceived -= passiveInterruptionTracker.OnDamageTaken;
    }


}
