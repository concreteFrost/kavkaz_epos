using UnityEngine;

[System.Serializable]
public class EnemyChaseHandler
{

    CharacterBehaviourStatsSO stats;

    [SerializeField] private float lostTargetTimer;
    [SerializeField] private float cantReachTimer;

    public EnemyChaseHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    #region Target Chase State
    public void ResetChaseState()
    {
        lostTargetTimer = 0f;
        cantReachTimer = 0f;
    }

    public void UpdateLostTargetTimer(bool isVisible) => lostTargetTimer = isVisible ? 0f : lostTargetTimer + Time.deltaTime;
    public void UpdateCantReachTimer(bool canReach) => cantReachTimer = canReach ? 0f : cantReachTimer + Time.deltaTime;

    public bool HasCantReachTimerExceeded() => cantReachTimer > stats.maxCantReachTimer;
    public bool HasLostTargetTimerExceeded() => lostTargetTimer > stats.maxLostTargetTimer;

    public bool IsTargetFar(float dist) => dist >= stats.maxChaseDistance;

    public bool IsCloseToAttack(float dist) => dist <= stats.distanceToStop;

    #endregion
}
