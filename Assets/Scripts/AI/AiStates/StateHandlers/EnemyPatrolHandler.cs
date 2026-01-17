using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class EnemyPatrolHandler
{
    CharacterBehaviourStatsSO stats;

    [SerializeField] private int currWalks = 0;

    public EnemyPatrolHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    public bool HasReachedMaxWalks() => currWalks > stats.maxPatrolAttempts;

    public void IncrementWalks() => currWalks++;

    public void ResetPatrol() => currWalks = 0;

    public float GetMaxPatrolRadius() => stats.maxDestiantionRadius;
}
