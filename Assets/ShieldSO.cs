using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSO", menuName = "Scriptable Systems/Items/Shields/ShieldSO")]
public class ShieldSO : ItemSO
{
    [Tooltip("Определяет порог поломки")]
    public float breakdownThreshold;

    [Tooltip("Степень защиты")]
    public float defenceBonus;
}
