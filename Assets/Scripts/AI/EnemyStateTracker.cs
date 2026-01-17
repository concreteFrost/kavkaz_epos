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


    [Header("Wait state")]
    public float waitTimer = 0f;

    [Header("Strafe state")]
    [SerializeField] private float timeInStrafeState = 0f;
    [SerializeField] private float maxTimeInStrafeState;


    public void Init(HumanoidAIDamageController damageController)
    {
        this.damageController = damageController;

        idleHandler = new EnemyIdleHandler(stats);
        patrolHandler = new EnemyPatrolHandler(stats);
        chaseHandler = new EnemyChaseHandler(stats);    

        combatHandler = new EnemyCombatHandler(stats);
        this.damageController.DamageTaken += combatHandler.OnDamageTaken;

        interruptionTracker = new EnemyPassiveInterruptionHandler();
        this.damageController.DamageTaken += interruptionTracker.OnDamageTaken;

    }

    private void OnDisable()
    {
        damageController.DamageTaken -=interruptionTracker.OnDamageTaken;
        damageController.DamageTaken -=combatHandler.OnDamageTaken; 
    }

    public void OnDamageTaken(Transform attackSource)
    {
        //var tracker = context.stateTracker;
        ////combat
        //tracker.RegisterDamage();

        ////wait 
        //tracker.InterruptWait();

        //tracker.InterruptStrafeState();


        //if (attackSource == null) return;

        //idle, patrol
        //interruptionTracker.Interrupt(attackSource.position);

    }
 
    #region Target Wait State
    public void UpdateWaitTimer(bool canReach) => waitTimer = canReach ? 0f : waitTimer + Time.deltaTime;

    public void ResetWaitState()=> waitTimer = 0f;

    public void InterruptWait()=> waitTimer = stats.maxWaitTimer;

    #endregion

    #region Strafe State

    public void UpdateTimeInStrafeState() => timeInStrafeState += Time.deltaTime;

    public void SetNewMaxInStrafeTime() => maxTimeInStrafeState = Random.Range(stats.minTimeInStrafeState, stats.maxTimeInStrafeState);

    public void ResetStrafeState()=> timeInStrafeState = 0f; 

    public void InterruptStrafeState() => timeInStrafeState = maxTimeInStrafeState;

    public bool IsStrafeTimeFinished() => timeInStrafeState >= maxTimeInStrafeState;

    public bool IsStrafeTargetFar(float dist) => dist > stats.maxTargetDistanceInStrafe;


    #endregion


}
