using UnityEngine;

public class DefenceCollider : MonoBehaviour
{
    Collider col;
    IDamagable Owner; //для получении информации о владельце
    IShield Shield;

    float currentDefenceBonus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void SetOwner(IDamagable source)
    {
        Owner =source;
    }

    public void ResetOwner()
    {
        Owner = null;
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

    public void CalculateDamage(float incoming)
    {

        Shield.ReduceDurability(Shield.ShieldData().breakdownPenalty);
        float defence = currentDefenceBonus != 0 ? currentDefenceBonus : 1f;
        float result = defence * incoming;

        if (Owner != null) {
            Owner.TakeDamage(result);
        }

       
       
    }


}
