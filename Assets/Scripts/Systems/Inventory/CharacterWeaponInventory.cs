using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponSaveData : InventoryItemSaveData
{
    public float durability;
    public int weaponLevel;
}

public class CharacterWeaponInventory : QuickAccessInventory
{
    [Header("Starter Set")]
    public CombatInventorySO starterSet;

    private HumanoidWeaponSetter weaponSetter;
    private Dictionary<string, ICombatItem> weaponPool = new Dictionary<string, ICombatItem>();
    private WeaponDataBaseSO weaponDataBaseSO;
    private List<CombatItemSO> cachedCombatItems;

    public void Init(HumanoidWeaponSetter setter)
    {
        base.BaseInit();
        weaponSetter = setter;
        
        var resources = Resources.Load<WeaponDataBaseSO>("DataBases/DataBase_Weapons");
        cachedCombatItems = new List<CombatItemSO>(resources.GetAllWeapons());  
        weaponDataBaseSO = resources;

        if (starterSet == null) return;

        if (starterSet.initialWeapon != null)
        {
            var weaponSo = starterSet.initialWeapon.GetComponent<Weapon>().WeaponData();
            var itemData = new WeaponData()
            {
                instanceId = Guid.NewGuid().ToString(),
                quantity = 1,
                itemSO = weaponSo,
                durability = 100,
                WeaponLevel = 1

            };

            AddCombatItemToInventory(itemData);
            EquipItem(itemData);
        }

        if (starterSet.initialShield != null)
        {
            var shieldSo = starterSet.initialShield.GetComponent<Shield>().ShieldData();
            var itemData = new WeaponData()
            {
                instanceId = Guid.NewGuid().ToString(),
                quantity = 1,
                itemSO =shieldSo,
                durability = 100,
                WeaponLevel = 1

            };
           
            AddCombatItemToInventory(itemData);
            EquipItem(itemData);

        }

    }

    public override void LoadInventoryData(SaveInventoryData data)
    {
        base.LoadInventoryData(data);

        weaponSetter.ResetAllCombatItems();

        //ВАЖНО: пересобираем items с учетом instanceId и durability
        items = new List<ItemData>();

        Dictionary<string, ItemSO> itemsMap = new Dictionary<string, ItemSO>();

        foreach (var item in cachedCombatItems)
            itemsMap[item.id] = item;

        foreach (var saved in data.items)
        {
            if (!itemsMap.TryGetValue(saved.id, out var so))
                continue;

            var parsedWeaponData = saved as WeaponSaveData;

            var newItem = new WeaponData()
            {
                itemSO = so,
                quantity = parsedWeaponData.quantity,
                instanceId = parsedWeaponData.instanceId,   
                durability = parsedWeaponData.durability,   
                isEquiped = parsedWeaponData.isEquiped,
                WeaponLevel = parsedWeaponData.weaponLevel
            };

            items.Add(newItem);

            // Если предмет был экипирован — восстанавливаем
            if (newItem.isEquiped)
            {
                EquipItem(newItem);
            }
        }

        Notify();
    }

    public override SaveInventoryData SaveInventoryData()
    {
        var data = new SaveInventoryData();

        data.items = new List<InventoryItemSaveData>();

        foreach (var item in items)
        {
            var weapon = item as WeaponData;
            if (weapon == null)
                continue;

            int quickSlotIndex = -1;

            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (quickSlots[i] == item)
                {
                    quickSlotIndex = i;
                    break;
                }
            }

            var weaponSaveData = new WeaponSaveData
            {
                id = weapon.itemSO.id,
                quantity = weapon.quantity,
                quickSlotIndex = quickSlotIndex,
                instanceId = weapon.instanceId,
                isEquiped = weapon.isEquiped,

                durability = weapon.durability,
                weaponLevel = weapon.WeaponLevel
            };

            data.items.Add(weaponSaveData);
        }

        data.currentIndex = currentIndex;

        return data;
    }

    // Пулл объектов: возвращаем GameObject для экипировки
    public ICombatItem GetWeaponObject(WeaponData data)
    {
        if (!weaponPool.TryGetValue(data.instanceId, out var obj))
        {
            var template = weaponDataBaseSO.Get(data.itemSO.id);
            GameObject go = Instantiate(template);
            var combatItem = go.GetComponent<CombatItem>();
            combatItem.Init(data);

            obj = combatItem;

            weaponPool[data.instanceId] = obj;
        }

        return obj;
    }

    // возвращаем созданный ItemData
    public void AddCombatItemToInventory(ItemData data, float durability = 100f)
    {
        if (data.itemSO == null)
            return;

        if (data.instanceId == null)
            data.instanceId = Guid.NewGuid().ToString();

        var parsedData = data as WeaponData;
        parsedData.durability = durability;

        
        AddItemToInventory(data);
    }


    public void EquipItem(WeaponData data)
    {
        
        ICombatItem obj = GetWeaponObject(data);
        weaponSetter.HandleSetCombatItem(obj);  
    }

    public void UnequipItem(WeaponData data)
    {
        if(weaponSetter.CurrentWeapon != weaponSetter.DefaultWeapon)
        {
            ICombatItem obj = GetWeaponObject(data);
        }
           
        weaponSetter.HandleResetCombatItem(data.instanceId);
    }

    public override void UseItem(ItemData data)
    {
        var weaponData = data as WeaponData;
         EquipItem(weaponData);
    }

    public override void RemoveFromInventory(ItemData item)
    {
        base.RemoveFromInventory(item);
        weaponSetter.HandleResetCombatItem(item.instanceId);
    }



    public ItemData GetCurrentWeaponData() => weaponSetter.CurrentWeapon.GetItemData();
    public ItemData GetCurrentShieldData() => weaponSetter.ShieldWeapon != null ? weaponSetter.ShieldWeapon.GetItemData() : null;    

}