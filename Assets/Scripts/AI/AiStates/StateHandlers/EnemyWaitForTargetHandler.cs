using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]   
public class EnemyWaitForTargetHandler
{
    public CharacterBehaviourStatsSO stats;

    [SerializeField] private float waitTimer = 0f;

    public EnemyWaitForTargetHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    public void UpdateWaitTimer(bool canReach) => waitTimer = canReach ? 0f : waitTimer + Time.deltaTime;

    public void ResetWaitState() => waitTimer = 0f;

    private void InterruptWait() => waitTimer = stats.maxWaitTimer;

    public bool HasWaitTimerExceeded()=> waitTimer >= stats.maxWaitTimer;

    public void OnDamageTaken(Transform attackSource)
    {
        InterruptWait();
    }

}
