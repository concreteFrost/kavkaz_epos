using UnityEngine;

public abstract class ProjectileAttackSO : ScriptableObject
{
    public abstract void Execute(IEmitter emitter);

    protected Quaternion SpreadRotation(float maxAngle)
    {
   
        return Quaternion.AngleAxis(
            Random.Range(-maxAngle, maxAngle),
            Random.onUnitSphere
        );
    }

}
