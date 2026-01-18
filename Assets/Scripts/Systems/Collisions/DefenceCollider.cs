using UnityEngine;

public struct DefenceOutcome
{
    public float healthDamage;
    public float balanceDamage;

    public DefenceOutcome(float healthDamage, float balanceDamage)
    {
        this.healthDamage = healthDamage;
        this.balanceDamage = balanceDamage;
    }
}

public class DefenceCollider : MonoBehaviour
{
    Collider col;
    IShield Shield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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

    public DefenceOutcome ProcessDamage(float healthDamage, float balanceDamage)
    {
        Shield.ReduceDurability(Shield.ShieldData().breakdownPenalty);

        float finalHealth = healthDamage * Shield.ShieldData().defenceBonus;

        return new DefenceOutcome(finalHealth, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DamageCollider>() != null)
        {
            var damageCollider = other.GetComponent<DamageCollider>();

            damageCollider.attackInterrupted = true;
            
            if(Shield.Owner != null)
            {

                var damagable = Shield.Owner.Damagable;

                if (damagable.IsDamaged) return;

                var outcomeDamage = ProcessDamage(damageCollider.healthDamage, damageCollider.balanceDamage);

                damagable.TakeDamage(outcomeDamage.healthDamage, outcomeDamage.balanceDamage, null);
            }
        }
    }

}
