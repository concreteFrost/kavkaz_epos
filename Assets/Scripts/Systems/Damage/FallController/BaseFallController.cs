using UnityEngine;

public abstract class BaseFallController : MonoBehaviour
{

    protected Transform self;

    protected IDamagable damagable;
    protected Vector3 lastGroundedPosition;
    
    protected bool wasLastGroundedPositionRegistered;

    protected const float fallDamageThreshold = 3f;
    protected const float fallDamageMultiplier = 12f;
    protected abstract void TrackFall();
    protected abstract void CalculateFallDamage();
}
