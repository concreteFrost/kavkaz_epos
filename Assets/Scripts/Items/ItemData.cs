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
public class WeaponData : ItemData 
{
    public float durability;
    [SerializeField]
    private int weaponLevel = 1;

    public int WeaponLevel
    {
        get => weaponLevel;
        set => weaponLevel = Mathf.Clamp(value, 1, 10);
    }

    /// <summary>
    /// Посколько оружие динамическое то ему нужны актуальные данные
    /// </summary>
    /// <returns></returns>
    //public List<ItemStat> ItemStats() => new List<ItemStat>()
    //{
    //    new ItemStat("base damage", GetWeaponDamageWithLevel(), ItemStatFormatType.flat),
    //    new ItemStat("cost per hit", weaponSO.GetBreakdownPenalty(), ItemStatFormatType.flat)
    //};


}
