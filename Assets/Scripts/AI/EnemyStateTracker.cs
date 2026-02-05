using UnityEngine;

public class EnemyStateTracker : MonoBehaviour
{
    public CharacterBehaviourStatsSO stats;
    
    private HumanoidAIDamageController damageController;

    public EnemyIdleHandler idleHandler;
    public EnemyPatrolHandler patrolHandler;
    public EnemyChaseHandler chaseHandler;  
 
    public EnemyCombatHandler combatHandler; 
    public EnemyWaitForTargetHandler waitForTargetHandler;
    public EnemyStrafeHandler strafeHandler;


    public void Init(EnemyStateTrackerServices services)
    {
    
        idleHandler = new EnemyIdleHandler(stats);
        patrolHandler = new EnemyPatrolHandler(stats);
        chaseHandler = new EnemyChaseHandler(stats);

        this.damageController = services.damageController;

        combatHandler = new EnemyCombatHandler(stats, services.stats);
        this.damageController.DamageTaken += combatHandler.OnDamageTaken;

        waitForTargetHandler = new EnemyWaitForTargetHandler(stats);
        this.damageController.DamageTaken += waitForTargetHandler.OnDamageTaken;

        strafeHandler = new EnemyStrafeHandler(stats);
        this.damageController.DamageTaken += strafeHandler.OnDamageTaken;

     
     
    }


    private void OnDisable()
    {
       
        damageController.DamageTaken -=combatHandler.OnDamageTaken; 
        damageController.DamageTaken -=waitForTargetHandler.OnDamageTaken;
        damageController.DamageTaken -=strafeHandler.OnDamageTaken;
       
    }


}
