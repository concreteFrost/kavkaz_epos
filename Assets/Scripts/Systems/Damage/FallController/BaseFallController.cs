using UnityEngine;
using Zenject;

public abstract class BaseFallController : MonoBehaviour
{

    protected Transform self;

    protected IDamagable damagable;
    protected Vector3 lastGroundedPosition;
    
    protected bool wasLastGroundedPositionRegistered;

    protected const float fallDamageThreshold = 5f;
    protected const float fallDamageMultiplier = 12f;
    protected abstract void TrackFall();
    protected virtual void CalculateFallDamage()
    {
        var fallHeight = lastGroundedPosition.y - self.position.y;

        if (fallHeight > fallDamageThreshold)
        {
            float damage = (fallHeight - fallDamageThreshold) * fallDamageMultiplier;

            DamageData damageData = new DamageData
            {
                damageMultiplier = 0,
                finalDamage = damage,
                balanceDamageType = BalanceDamageType.High,
                impactForce = 0,
                damageSourceType = DamageSourceType.None
            };


            damagable.TakeDamage(damageData);

            //playerStatsModifier.StartFallPenalty(penaltyDuration, damage);

        }
    }

    /// <summary>
    /// ѕредотвар€щает смерть при телепортации
    /// </summary>
    /// <param name="pos"></param>
    public void ResetLastGroundedPosition(Vector3 pos)
    {
        lastGroundedPosition = pos;
       
    }


}
