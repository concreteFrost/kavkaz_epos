
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
    /// Выкидывает оружие от владельца по направлению вперед 
    /// </summary>
    /// <param name="from">владелец</param>
    /// <param name="force">сила броска</param>
    public void ThrowWeapon(Transform from, float force);


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
