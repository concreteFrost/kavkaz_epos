using UnityEngine;

public class PlayerFallController : MonoBehaviour
{
    IHumanoidMovement motor;
    IDamagable damagable;

    public bool wasLastGroundedPositionRegistered;
    public Vector3 lastGroundedPosition;

    public float fallDamageThreshold = 4f;
    public float fallDamageMultiplier = 5f;

    public void Init(IHumanoidMovement motor,IDamagable damagable)
    {
        this.motor = motor;
        this.damagable = damagable; 
    }

    private void Update()
    {
        TrackFall();
    }

    private void TrackFall()
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

    private void CalculateFallDamage()
    {

        var fallHeight = lastGroundedPosition.y - transform.position.y;

        if (fallHeight > fallDamageThreshold)
        {
            float damage = (fallHeight - fallDamageThreshold) * fallDamageMultiplier;

            DamageData damageData = new DamageData
            {
                healthDamageMultiplier = damage,
                balanceDamageType = BalanceDamageType.High,
                impactForce = 0,
            };
            
            damagable.TakeDamage(damageData);   

            //playerStatsModifier.StartFallPenalty(penaltyDuration, damage);

        }

    }
}
