using UnityEngine;

public class PlayerFallController : BaseFallController
{
    IHumanoidMovement motor;

    public void Init(IHumanoidMovement motor,IDamagable damageController, Transform self)
    {
        this.motor = motor;
        this.damagable = damageController;
        this.self = self;

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
            lastGroundedPosition = self.position;
        }
        if (motor.IsGrounded && wasLastGroundedPositionRegistered)
        {
            wasLastGroundedPositionRegistered = false;
            CalculateFallDamage();
        }
    }

}
