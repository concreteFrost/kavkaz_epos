using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]   
public class EnemyWaitForTargetHandler
{
    public CharacterBehaviourStatsSO stats;

    [SerializeField] private float waitTimer = 0f;
    [SerializeField] private float nextAttackTimer = 0f;
    [SerializeField] private float maxAttackTimer;


    [Header("Передвижение во время ожидания")]
    [SerializeField] private float repositionTimer;
    [SerializeField] private float nextRepositionTime;


    public EnemyWaitForTargetHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    public void UpdateWaitTimer(bool canReach) => waitTimer = canReach ? 0f : waitTimer + Time.deltaTime;

    public void ResetWaitState()
    {
        waitTimer = 0f;
        ResetDistanceAttackTimer();
        ResetRepositionTimer();
    }

    private void InterruptWait() => waitTimer = stats.maxWaitTimer;

    public bool HasWaitTimerExceeded()=> waitTimer >= stats.maxWaitTimer;

    public void OnDamageTaken(Transform attackSource)
    {
        InterruptWait();
    }


    #region Distance Attack 
    public void ResetDistanceAttackTimer()
    {
        nextAttackTimer = 0f;
        maxAttackTimer = Random.Range(3, 6);
    }
    public bool CanAttack()
    {
        
        nextAttackTimer += Time.deltaTime;

        if(nextAttackTimer >= maxAttackTimer)
        {
            ResetDistanceAttackTimer();
            return true;
        }

        return false;

    }

    #region Move While Waiting
    public void ResetRepositionTimer()
    {
        repositionTimer = 0f;
        nextRepositionTime = Random.Range(
            stats.minRepositionCooldown,
            stats.maxRepositionCooldown);
    }

    public bool ShouldReposition()
    {
        repositionTimer += Time.deltaTime;

        if (repositionTimer < nextRepositionTime)
            return false;

        ResetRepositionTimer();

        return Random.value <= stats.willMoveChance;
    }
    #endregion



    #endregion
}
