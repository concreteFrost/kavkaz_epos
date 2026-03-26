
using UnityEngine;

public interface IWeapon : ICombatItem
{
    /// <summary>
    /// Данные оружия
    /// </summary>
    /// <returns></returns>
    public WeaponSO WeaponData();

    /// <summary>
    /// Включает хитбоксы
    /// </summary>
    public void PerformAttack();

    /// <summary>
    /// Выключает хитбоксы
    /// </summary>
    public void CancelAttack();


    /// <summary>
    /// Выбирает атаку из списка предоставленного WeaponSO
    /// </summary>
    /// <param name="index"></param>
    public void SelectAttack(int index);

    WeaponAttack GetPowerAttack(WeaponAttack attack);

    /// <summary>
    /// Получение данных текущей атаки
    /// </summary>
    /// <returns></returns>
    public WeaponAttack CurrentAttack();

}
