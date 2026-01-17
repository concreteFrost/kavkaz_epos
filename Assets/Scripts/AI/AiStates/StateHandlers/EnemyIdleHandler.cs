using UnityEngine;

[System.Serializable]   
public class EnemyIdleHandler
{

    [SerializeField] private float currIdleTime = 0;
    [SerializeField] private float maxIdleTime;

    CharacterBehaviourStatsSO stats;

    public EnemyIdleHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats; 
    }

    public void UpdateCurrentIdleTime() => currIdleTime += Time.deltaTime;

    public void SetMaxIdleTime() =>
        maxIdleTime = Random.Range(
            (float)stats.minIdleStationary,
            (float)stats.maxIdleStationary
        );

    public void ResetIdleState()
    {
        currIdleTime = 0;
    }

    public bool HasIdleTimeFinished() => currIdleTime >= maxIdleTime;
}
