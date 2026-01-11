
using System;
using UnityEngine;

public interface IDamagable
{
    public CharacterType CharacterType { get; set; } 
    //public ITargetLockable Lockable { get; set; }
    //public string SourceId();

    public abstract void TakeDamage(float damage, float balanceDamage, Transform source=null);   
    public void Die();
    public bool IsDead();
    bool IsDamaged { get; set; }
    float BalancePenalty { get; set; }

    public Transform GetAimTransform();
    public Transform GetOrigin();

    public event Action<Transform> DamageTaken;
}
