using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public ItemSO itemSO;
    [Min(1)] public int quantity = 1;
    public string instanceId;
    public bool isEquiped;
    [Range(0f, 100f)] public float durability = 100f;
    [SerializeField, Range(1, 10)] protected int weaponLevel = 1;

    public int WeaponLevel
    {
        get => weaponLevel;
        set => weaponLevel = Mathf.Clamp(value, 1, 10);
    }

    // Loot and reward definitions produce typed copies for their inventories.
    public ItemData CreateInstance()
    {
        ItemData copy;
        if (GetType() != typeof(ItemData))
            copy = (ItemData)MemberwiseClone();
        else
        {
            copy = itemSO switch
            {
                ShieldSO _ => new ShieldData(),
                CombatItemSO _ => new WeaponData(),
                SpellProjectileSO _ => new SpellData(),
                ConsumableItemSO _ => new ConsumableData(),
                _ => new ItemData()
            };
            copy.itemSO = itemSO;
            copy.quantity = quantity;
            copy.durability = durability;
            copy.weaponLevel = weaponLevel;
        }
        copy.instanceId = System.Guid.NewGuid().ToString();
        copy.isEquiped = false;
        return copy;
    }

}

[System.Serializable]
public class CombatItemData : ItemData
{


}

[System.Serializable]
public class WeaponData : CombatItemData , IItemStats
{
    public float GetWeaponDamageWithLevel(int level)
    {
        float baseDamage = (itemSO as WeaponSO).GetBaseDamage();
        float multiplier = WeaponUpgradeFormula.GetDamageMultiplier(level);

        return Mathf.Round(baseDamage * multiplier * 10f) / 10f;
    }

    public void Upgrade()
    {
        if (!CanUpgrade())
        {
            Debug.Log("weapon is on max level");
            return;
        }

        WeaponLevel++;
    }

    public void Downgrade()
    {
        if (!CanDowngrade())
        {
            Debug.Log("weapon level is on minimum");
            return;
        }

        WeaponLevel--;
    }

    public bool CanUpgrade() => WeaponLevel < 10;

    public bool CanDowngrade() => WeaponLevel > 1;

    /// <summary>
    /// Посколько оружие динамическое то ему нужны актуальные данные
    /// </summary>
    /// <returns></returns>
    public  List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("weapon level", weaponLevel, ItemStatFormatType.flat),
        new ItemStat("base damage",   Mathf.Round(GetWeaponDamageWithLevel(WeaponLevel) * 10f) / 10f, ItemStatFormatType.flat),
        new ItemStat("durability", durability, ItemStatFormatType.flat)
    };

}

[System.Serializable]
public class ShieldData : CombatItemData, IItemStats {

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    { 
        new ItemStat("durability", durability, ItemStatFormatType.flat)
    };
}
[System.Serializable]
public class SpellData: ItemData, IItemStats
{
    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("base damage", (itemSO as SpellProjectileSO).GetBaseDamage(), ItemStatFormatType.flat),

    };
}

[System.Serializable]
public class ConsumableData: ItemData
{

}

