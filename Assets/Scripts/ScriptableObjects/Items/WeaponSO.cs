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

    public WeaponType weaponType;

    [Header("Базовый урон")]
    [Tooltip("Сырой урон без мультипликаторов")]
    [SerializeField] private float baseDamage;

    [Header("Поломка")]
    [Tooltip("Сколько снимать от состояния при ударе")]
    [SerializeField] private float breakdownPenalty;

    [Header("Набор атак")]
    public AttackSO attackSet;

    public float GetBreakdownPenalty() {
        if (breakdownPenalty == 0) return 0.001f;

        return breakdownPenalty;    
    }

    public float GetBaseDamage()=> baseDamage;  
}
