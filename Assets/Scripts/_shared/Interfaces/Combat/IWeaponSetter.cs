using System.Collections.Generic;
using UnityEngine;

public interface IWeaponSetter
{

    /// <summary>
    /// Изначальное оружие
    /// </summary>
    public IWeapon DefaultWeapon { get; set; }

    /// <summary>
    /// Текущее оружие 
    /// </summary>
    public IWeapon CurrentWeapon { get; set; }

    /// <summary>
    /// Щит
    /// </summary>
    public IShield ShieldWeapon { get; set; }


    /// <summary>
    /// Сброс оружия. CurrentWeapon = DefaultWeapon
    /// </summary>
    //public abstract void ResetCombatItem(CombatItem item);

    public Transform GetRightHand();

    /// <summary>
    /// Используется для назначения положения оружия в левой руке (в основном для щита)
    /// </summary>
    /// <returns></returns>
    public Transform GetLeftHand();

}
