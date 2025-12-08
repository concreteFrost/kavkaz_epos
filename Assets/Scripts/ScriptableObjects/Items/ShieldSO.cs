using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSO", menuName = "Scriptable Systems/Items/Shields/ShieldSO")]
public class ShieldSO : ItemSO
{

    [Tooltip("Степень защиты")]
    public float defenceBonus;

    [Tooltip("Ущерб от защиты")]
    public float breakdownPenalty;
}
