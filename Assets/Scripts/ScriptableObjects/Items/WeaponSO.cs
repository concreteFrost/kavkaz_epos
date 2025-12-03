using UnityEngine;

public enum WeaponType
{
    melee =0,
    sword =1,
}

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Systems/Items/Weapons/WeaponSO")]
public class WeaponSO : ItemSO 
{
    public bool canOverride = false;

    [Tooltip("Урон от удара")]
    public float damageAmount;

    [Tooltip("Определяет порог поломки")]
    public float breakdownThreshold;

    public AttackSO attackSet;


}
