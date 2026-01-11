using System.Collections.Generic;
using UnityEngine;

public interface IAttackSource
{
    /// <summary>
    /// Назначает источник атаки для предотвращения попадания по самому себе
    /// </summary>
    /// <returns></returns>
    public int SourceId();

    /// <summary>
    /// Указывает на источник атаки
    /// </summary>
    /// <returns></returns>
    public Transform SourcePosition();

    /// <summary>
    /// Цели которые стоит игнорировать. Например дружественные NPC или же цели того же класса
    /// </summary>
    public List<CharacterType> TargetsToIgnore { get; set; }

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
    /// Назначение текущего оружия
    /// </summary>
    /// <param name="w">Целевое оружие</param>
    public abstract void SetWeapon(IWeapon w);

    /// <summary>
    /// Назначение щита
    /// </summary>
    /// <param name="w">Целевой щит</param>
    public abstract void SetShield(IShield w);

    /// <summary>
    /// Сброс оружия. CurrentWeapon = DefaultWeapon
    /// </summary>
    public abstract void ResetWeapon();

    /// <summary>
    /// Сброс щита. ShieldWeapon = null
    /// </summary>
    public abstract void ResetShield();

    /// <summary>
    /// Используется для назначения положения оружия в правой руке
    /// </summary>
    /// <returns></returns>
    public Transform GetRightHand();

    /// <summary>
    /// Используется для назначения положения оружия в левой руке (в основном для щита)
    /// </summary>
    /// <returns></returns>
    public Transform GetLeftHand();

}
