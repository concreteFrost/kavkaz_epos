using UnityEngine;

public class AiFallController : BaseFallController
{

    IRagdollController ragdollController;

    public void Init(IRagdollController ragdollController, IDamagable damagable, Transform self)
    {
        this.ragdollController = ragdollController;
        this.damagable = damagable;
        this.self = self;
    }

    private void Update()
    {
        TrackFall();
    }

    protected override void TrackFall()
    {
        if (damagable.IsDead) return;

        if (ragdollController.IsKnockedOut && !wasLastGroundedPositionRegistered)
        {
            wasLastGroundedPositionRegistered = true;
            lastGroundedPosition = self.position;
        }
        else if (!ragdollController.IsBonesMoving(threshold:0.1f) && wasLastGroundedPositionRegistered)
        {
            wasLastGroundedPositionRegistered = false;
            CalculateFallDamage();
        }
    }

    protected override void CalculateFallDamage()
    {

        var fallHeight = lastGroundedPosition.y - self.position.y;


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

           
        }

    }
}
