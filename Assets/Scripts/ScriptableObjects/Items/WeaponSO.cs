using UnityEngine;

public enum WeaponType
{
    melee =0,
    sword =1,
}

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Systems/Items/Weapons/WeaponSO")]
public class WeaponSO : ItemSO 
{
    [Tooltip("Определяет может ли игрок брать другое оружие поверх этого")]
    public bool canOverride = false;

    [Header("Урон")]
    [Tooltip("Урон от удара")]
    [SerializeField] private float damageAmount;

    [Tooltip("Урон по балансу")]
    [SerializeField] private float balancePenalty;

    [Header("Поломка")]
    [Tooltip("Сколько снимать от состояния при ударе")]
    [SerializeField] private float breakdownPenalty;

    [Header("Набор атак")]
    public AttackSO attackSet;

    public float GetHealthDamage()
    {
        return damageAmount;    
    }

    public float GetBalanceDamage()
    {
        if (balancePenalty == 0) return 0.001f;

        return balancePenalty;  
    }

    public float GetBreakdownPenalty() {
        if (breakdownPenalty == 0) return 0.001f;

        return breakdownPenalty;    
    }
}
