using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;
    private HumanoidAIDamageController damageController;

    public EnemyIdleHandler idleHandler;
    public EnemyPatrolHandler patrolHandler;
    public EnemyChaseHandler chaseHandler;  
    public EnemyPassiveInterruptionHandler passiveInterruptionTracker;
    public EnemyCombatHandler combatHandler; 
    public EnemyWaitForTargetHandler waitForTargetHandler;
    public EnemyStrafeHandler strafeHandler;    

    public void Init(HumanoidAIDamageController damageController, HumanoidStats statsInfo)
    {
        this.damageController = damageController;

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
    }


}
