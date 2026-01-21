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


    public void ProcessDamage(float healthDamage, BalanceDamageType balanceDamage, Transform source)
    {

        Shield.ReduceDurability(Shield.ShieldData().breakdownPenalty);

        float finalHealth = healthDamage * Shield.ShieldData().defenceBonus;
        BalanceDamageType finalBalance = BalanceDamageType.Blocked;

        Shield.Owner.Damagable.TakeDamage(finalHealth, finalBalance, source);

    }

   



}
