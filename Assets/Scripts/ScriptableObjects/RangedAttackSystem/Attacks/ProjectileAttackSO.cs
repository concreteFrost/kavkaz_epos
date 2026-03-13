using UnityEngine;

public abstract class ProjectileAttackSO : ScriptableObject
{
    public abstract void Execute(IEmitter emitter, int amount,float spawnDelay);

}
