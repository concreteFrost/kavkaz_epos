using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    OneHand = 0,
    TwoHands = 1,
}

[CreateAssetMenu(fileName = "Weapon", menuName = ScriptablePaths.ITEMS_PATH + "/Weapons/Weapon")]
public class WeaponSO : CombatItemSO, IItemStats
{
    public override bool IsStackable() => false;

    [Tooltip("Определяет может ли игрок брать другое оружие поверх этого.")]
    public bool canOverride = false;

    [Header("Weapon Type")]
    [Tooltip("Тип оружия (одноручное,двуручное).")]
    public WeaponType weaponType;

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
    public float GetBaseDamage() => baseDamage;

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("base damage", GetBaseDamage(), ItemStatFormatType.flat),
        new ItemStat("cost per hit", GetBreakdownPenalty(), ItemStatFormatType.flat)   
    };



}
