
using System;
using UnityEngine;

public interface IDamagable
{
    public CharacterType CharacterType { get; set; } 
    //public ITargetLockable Lockable { get; set; }
    //public string SourceId();

    public abstract void TakeDamage(DamageData damageData, Transform source=null);   
    public void Die();
    public bool IsDead { get; set; }
    bool IsDamaged { get; set; }
    bool IsKnockedOut {  get; set; }
    BalanceDamageType BalancePenalty { get; set; }

    public Transform GetAimTransform();
    public Transform GetOrigin();

    public event Action<Transform> DamageTaken;
}
