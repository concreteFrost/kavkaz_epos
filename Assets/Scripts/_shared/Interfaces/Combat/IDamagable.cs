
using System;
using UnityEngine;


public interface IDamagable
{
    CharacterType CharacterType { get; set; } 
    abstract void TakeDamage(DamageData damageData, Transform source=null);   
    bool IsDead { get; set; }
    bool IsDamaged { get; set; }
    bool IsKnockedOut {  get; set; }
    Transform GetAimTransform();
    Transform GetOrigin();
    event Action<Transform> DamageTaken;


}
