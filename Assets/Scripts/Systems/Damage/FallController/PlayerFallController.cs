using UnityEngine;

public class PlayerFallController : BaseFallController
{
    IHumanoidMovement motor;

    public void Init(IHumanoidMovement motor,IDamagable damageController)
    {
        this.motor = motor;
        this.damagable = damageController;

    }

    private void Update()
    {
        TrackFall();
    }

    protected override void TrackFall()
    {
       
        if(damagable.IsDead) return;

        if (!motor.IsGrounded && !wasLastGroundedPositionRegistered)
        {
            wasLastGroundedPositionRegistered = true;
            lastGroundedPosition = transform.position;
        }
        if (motor.IsGrounded && wasLastGroundedPositionRegistered)
        {
            wasLastGroundedPositionRegistered = false;
            CalculateFallDamage();
        }
    }

    protected override void CalculateFallDamage()
    {

        var fallHeight = lastGroundedPosition.y - transform.position.y;

        if (fallHeight > fallDamageThreshold)
        {
            float damage = (fallHeight - fallDamageThreshold) * fallDamageMultiplier;

            DamageData damageData = new DamageData
            {
                damageMultiplier = damage,
                balanceDamageType = BalanceDamageType.High,
                impactForce = 0,
            };
            
            damagable.TakeDamage(damageData);   

            //playerStatsModifier.StartFallPenalty(penaltyDuration, damage);

        }

    }
}
