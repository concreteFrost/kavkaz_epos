using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;
    private HumanoidAIDamageController damageController;

    public EnemyIdleHandler idleHandler;
    public EnemyPatrolHandler patrolHandler;
    public EnemyChaseHandler chaseHandler;  
    public EnemyPassiveInterruptionHandler interruptionTracker;
    public EnemyCombatHandler combatHandler; 
    public EnemyWaitForTargetHandler waitForTargetHandler;
    public EnemyStrafeHandler strafeHandler;    

    public void Init(HumanoidAIDamageController damageController)
    {
        this.damageController = damageController;

        idleHandler = new EnemyIdleHandler(stats);
        patrolHandler = new EnemyPatrolHandler(stats);
        chaseHandler = new EnemyChaseHandler(stats);    

        combatHandler = new EnemyCombatHandler(stats);
        this.damageController.DamageTaken += combatHandler.OnDamageTaken;

        waitForTargetHandler = new EnemyWaitForTargetHandler(stats);
        this.damageController.DamageTaken += waitForTargetHandler.OnDamageTaken;

        strafeHandler = new EnemyStrafeHandler(stats);
        this.damageController.DamageTaken += strafeHandler.OnDamageTaken;

        interruptionTracker = new EnemyPassiveInterruptionHandler();
        this.damageController.DamageTaken += interruptionTracker.OnDamageTaken;

    }

    private void OnDisable()
    {
        damageController.DamageTaken -=interruptionTracker.OnDamageTaken;
        damageController.DamageTaken -=combatHandler.OnDamageTaken; 
        damageController.DamageTaken -=waitForTargetHandler.OnDamageTaken;
        damageController.DamageTaken -=strafeHandler.OnDamageTaken;
    }


}
