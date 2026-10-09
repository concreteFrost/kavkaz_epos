using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponSaveData : InventoryItemSaveData
{
    public float durability;
    public int weaponLevel;
}

public class CharacterWeaponInventory : BaseInventory<CombatItemData>
{
    [Header("Initial Equipment")]
    public GameObject initialWeapon;
    public GameObject initialShield;

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

        if (initialWeapon != null)
        {
            var weaponSo = initialWeapon.GetComponent<Weapon>().WeaponData();
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

        if (initialShield != null)
        {
            var shieldSo = initialShield.GetComponent<Shield>().ShieldData();
            var itemData = new ShieldData()
            {
                instanceId = Guid.NewGuid().ToString(),
                quantity = 1,
                itemSO =shieldSo,
                durability = 100,

            };
           
            AddCombatItemToInventory(itemData);
            EquipItem(itemData);

        }

    }

    /// <summary>
    /// ТОЛЬКО ДЛЯ ТЕСТА
    /// </summary>
    public void AddAllItemsOnStart()
    {
        var allItems = Resources.LoadAll<WeaponSO>("Items/Weapons/");

        foreach (var item in allItems)
        {
            var data = new WeaponData
            {
                itemSO = item,
                quantity = 20,
                durability = 100,
                WeaponLevel = 1
            };

            AddItemToInventory(data);
           

        }
    }


    public override void LoadInventoryData(SaveInventoryData data)
    {
        base.LoadInventoryData(data);

        weaponSetter.ResetAllCombatItems();

        //ВАЖНО: пересобираем items с учетом instanceId и durability
        items = new List<CombatItemData>();

        Dictionary<string, ItemSO> itemsMap = new Dictionary<string, ItemSO>();

        foreach (var item in cachedCombatItems)
            itemsMap[item.id] = item;

        foreach (var saved in data.items)
        {
            if (!itemsMap.TryGetValue(saved.id, out var so))
                continue;

            var parsedWeaponData = saved as WeaponSaveData;

            CombatItemData newItem = so is ShieldSO ? new ShieldData() : new WeaponData();
            newItem.itemSO = so;
            newItem.quantity = parsedWeaponData.quantity;
            newItem.instanceId = parsedWeaponData.instanceId;
            newItem.durability = parsedWeaponData.durability;
            newItem.isEquiped = parsedWeaponData.isEquiped;
            if (newItem is WeaponData loadedWeapon) loadedWeapon.WeaponLevel = parsedWeaponData.weaponLevel;

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
            var combatItem = item;
            var weapon = combatItem as WeaponData;
            if (combatItem == null)
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
                id = combatItem.itemSO.id,
                quantity = combatItem.quantity,
                quickSlotIndex = quickSlotIndex,
                instanceId = combatItem.instanceId,
                isEquiped = combatItem.isEquiped,

                durability = combatItem.durability,
                weaponLevel = weapon != null ? weapon.WeaponLevel : 1
            };

            data.items.Add(weaponSaveData);
        }

        data.currentIndex = currentIndex;

        return data;
    }

    // Пулл объектов: возвращаем GameObject для экипировки
    public ICombatItem GetWeaponObject(CombatItemData data)
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
    public void AddCombatItemToInventory(CombatItemData data)
    {
        if (data.itemSO == null)
            return;

        if (string.IsNullOrEmpty(data.instanceId))
            data.instanceId = Guid.NewGuid().ToString();

        var parsedData = data;

        
        AddItemToInventory(data);
    }


    public void EquipItem(CombatItemData data)
    {
        
        ICombatItem obj = GetWeaponObject(data);
        weaponSetter.HandleSetCombatItem(obj);  
    }

    public void UnequipItem(CombatItemData data)
    {
        if(weaponSetter.CurrentWeapon != weaponSetter.DefaultWeapon)
        {
            ICombatItem obj = GetWeaponObject(data);
        }
           
        weaponSetter.HandleResetCombatItem(data.instanceId);
    }

    public override void UseItem(CombatItemData data)
    {
        EquipItem(data);
    }

    public override void RemoveFromInventory(CombatItemData item)
    {
        base.RemoveFromInventory(item);
        weaponSetter.HandleResetCombatItem(item.instanceId);
    }



    public ItemData GetCurrentWeaponData() => weaponSetter.CurrentWeapon.GetItemData();
    public ItemData GetCurrentShieldData() => weaponSetter.ShieldWeapon != null ? weaponSetter.ShieldWeapon.GetItemData() : null;    

}