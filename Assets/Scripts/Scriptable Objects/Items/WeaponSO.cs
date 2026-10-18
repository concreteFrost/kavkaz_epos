using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponGripType
{
    OneHand = 0,
    TwoHands = 1,
}

public enum WeaponType
{
    None = 0,
    Dagger = 1,
    Saber = 2,
    Spear = 3,
    Sword= 4,
    Axe = 5

}

[CreateAssetMenu(fileName = "Weapon", menuName = ScriptablePaths.ITEMS_PATH + "/Weapons/Weapon")]
public class WeaponSO : CombatItemSO
{
    public override bool IsStackable() => false;

    [Tooltip("Определяет может ли игрок брать другое оружие поверх этого.")]
    public bool canOverride = false;

    [Header("Weapon Type")]

    public WeaponType weaponType;
    [Tooltip("Тип хвата оружия (одноручное,двуручное).")]
    public WeaponGripType gripType;

    [Header("Базовый урон")]
    [Tooltip("Сырой урон без мультипликаторов.")]
    [SerializeField] private float baseDamage;

    [Header("Набор атак")]
    [Tooltip("Набор атак.")]
    public AttackSO attackSet;

    [Header("Idle анимация оружия")]
    [Tooltip("Анимация воспроизводимая в Idle состоянии когда игрок держит оружие в руке.")]
    public AnimationClip idleAnimation;

    [Header("Audio")]
    public EventReference equipEvent;

    [Tooltip("Звук взмаха оружием. Момент воспроизведения задаётся в анимации атаки.")]
    public EventReference swingEvent;
    public float GetBaseDamage() => baseDamage;

}
