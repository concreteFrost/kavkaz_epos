using UnityEngine;


[CreateAssetMenu(fileName = "ShieldSO", menuName = "Scriptable Systems/Items/Shields/ShieldSO")]
public class ShieldSO : ItemSO
{

    [Space(10)]
    [Header("Защита")]
    [Tooltip("Степень защиты")]
    public float defenceBonus;

    [Tooltip("Поглощение утраты баланса")]
    public float balanceBlockFactor;

    [Space(10)]
    [Header("Поломка")]
    [Tooltip("Ущерб от защиты")]
    public float breakdownPenalty;

    public float GetDefenceBonus()
    {
        if (defenceBonus <= 0) return 0.01f;

        return defenceBonus;
    }

    public float GetBalanceBlockFactor()
    {
        if (balanceBlockFactor <= 0) return 0.01f;

        return balanceBlockFactor;  
    }

    public float GetBreakdownPenalty()
    {
        if (breakdownPenalty <= 0) return 0.01f;

        return breakdownPenalty;
    }

}
