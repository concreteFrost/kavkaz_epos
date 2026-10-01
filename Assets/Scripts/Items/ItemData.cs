using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public ItemSO itemSO;
    public int quantity;

    public string instanceId;

    public bool isEquiped;

}

[System.Serializable]
public class WeaponData : ItemData , IItemStats
{
    public float durability;
    [SerializeField]
    private int weaponLevel = 1;

    public int WeaponLevel
    {
        get => weaponLevel;
        set => weaponLevel = Mathf.Clamp(value, 1, 10);
    }

    public float GetWeaponDamageWithLevel()
    {
        var baseWeaponDamage = (itemSO as WeaponSO).GetBaseDamage();

        float weaponMultiplier =
        WeaponUpgradeFormula.GetDamageMultiplier(weaponLevel);

        return baseWeaponDamage *= weaponMultiplier;
    }

    /// <summary>
    /// Посколько оружие динамическое то ему нужны актуальные данные
    /// </summary>
    /// <returns></returns>
    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("weapon level", weaponLevel, ItemStatFormatType.flat),
        new ItemStat("base damage",   Mathf.Round(GetWeaponDamageWithLevel() * 10f) / 10f, ItemStatFormatType.flat),
        new ItemStat("durability", durability, ItemStatFormatType.flat)
    };

}

public class SpellData: ItemData, IItemStats
{
    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("base damage", (itemSO as SpellProjectileSO).GetBaseDamage(), ItemStatFormatType.flat),

    };
}
