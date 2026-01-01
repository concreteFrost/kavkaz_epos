using UnityEngine;

public abstract class ProjectileAttackSO : ScriptableObject
{
    public float cooldown = 0.5f;
    public abstract void Execute(IEmitter emitter);

    protected Quaternion SpreadRotation(float maxAngle)
    {
   
        return Quaternion.AngleAxis(
            Random.Range(-maxAngle, maxAngle),
            Random.onUnitSphere
        );
    }

}
