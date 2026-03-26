using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Weapon Status Effect Item", fileName = "Weapon Status Effect Item")]
public class WeaponModifierItemSO : ConsumableItemSO, IItemStats
{
    [Tooltip("Процент восполнения прочности оружия (в единицах)")]
    [SerializeField] private float durabilityToGain;

    public override bool IsStackable() => true;


    public float GetDurabilityTopUpAmount() => durabilityToGain;

    public  void UseItem(IWeaponSetter ctx)
    {
        if (ctx.CurrentWeapon == null) return;

        ctx.CurrentWeapon.IncreaseDurability(GetDurabilityTopUpAmount());   
    }

    public List<ItemStat> ItemStats() => new List<ItemStat>()
    {
        new ItemStat("weapon repait",GetDurabilityTopUpAmount(), ItemStatFormatType.flat),
       
    };



}





