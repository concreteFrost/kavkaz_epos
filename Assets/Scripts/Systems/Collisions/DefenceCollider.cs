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

        Debug.Log(finalHealth);

        return new DefenceOutcome(finalHealth, 0f);
    }

}
