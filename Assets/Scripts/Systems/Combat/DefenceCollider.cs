using UnityEngine;

public class DefenceCollider : MonoBehaviour
{
    Collider col;
    IShield Shield;

    float currentDefenceBonus;

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

    public float CalculateDamage(float healthDamage, float balanceDamage)
    {
        // Прочность щита уменьшается
        Shield.ReduceDurability(Shield.ShieldData().breakdownPenalty);

        // Коэффициент защиты щита
        float defence = currentDefenceBonus != 0 ? currentDefenceBonus : 1f;

        // --- УРОН ---

        // Урон после щита
        float finalDamage = healthDamage * defence;

        return finalDamage; 
    }

}
