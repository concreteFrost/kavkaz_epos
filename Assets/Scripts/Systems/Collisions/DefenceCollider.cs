using System.Collections.Generic;
using UnityEngine;


public class DefenceCollider : MonoBehaviour
{
    Collider col;
    public IShield Shield;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }


    public void SetShieldData(IShield shield)
    {
        Shield = shield;
    }

    public void EnableCollider()
    {
    
        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
    }


    public void ProcessDamage(DamageData damageData, Transform source)
    {

        Shield.ReduceDurability(Shield.ShieldData().breakdownPenalty);

        damageData.healthDamageMultiplier = damageData.healthDamageMultiplier * Shield.ShieldData().defenceBonus;
        damageData.balanceDamageType = BalanceDamageType.Blocked;

        Shield.Owner.Damagable.TakeDamage(damageData, source);

    }

   



}
