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
        else if (!ragdollController.IsBonesMoving() && wasLastGroundedPositionRegistered)
        {
            Debug.Log("bones not moving");
            wasLastGroundedPositionRegistered = false;
            CalculateFallDamage();
        }
    }

   
}
