
using UnityEngine;

[System.Serializable]   
public class EnemyStrafeHandler
{
    CharacterBehaviourStatsSO stats;

    [Header("Strafe state")]
    [SerializeField] private float timeInStrafeState = 0f;
    [SerializeField] private float maxTimeInStrafeState;


    public EnemyStrafeHandler(CharacterBehaviourStatsSO stats)
    {
        this.stats = stats;
    }

    public void UpdateTimeInStrafeState() => timeInStrafeState += Time.deltaTime;

    public void SetNewMaxInStrafeTime() => maxTimeInStrafeState = Random.Range(stats.minTimeInStrafeState, stats.maxTimeInStrafeState);

    public void ResetStrafeState() => timeInStrafeState = 0f;

    public void InterruptStrafeState() => timeInStrafeState = maxTimeInStrafeState;

    public bool IsStrafeTimeFinished() => timeInStrafeState >= maxTimeInStrafeState;

    public bool IsStrafeTargetFar(float dist) => dist > stats.maxTargetDistanceInStrafe;
  

    public void OnDamageTaken(IAttackSource attackSource)
    {
        InterruptStrafeState(); 
    }

}
