using System;
using System.Collections.Generic;
using UnityEngine;

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
            var itemData = new ItemData()
            {
                instanceId = Guid.NewGuid().ToString(),
                quantity = 1,
                itemSO = weaponSo,

            };

            AddCombatItemToInventory(itemData);
            EquipWeapon(itemData);
        }

        if (starterSet.initialShield != null)
        {
            var shieldSo = starterSet.initialShield.GetComponent<Shield>().ShieldData();
            var itemData = new ItemData()
            {
                instanceId = Guid.NewGuid().ToString(),
                quantity = 1,
                itemSO =shieldSo,

            };
           
            AddCombatItemToInventory(itemData);
            EquipShield(itemData);

        }

    }

    public override void LoadInventoryData(SaveInventoryData data)
    {
        base.LoadInventoryData(data);

        if (data == null) return;

        //ВАЖНО: пересобираем items с учетом instanceId и durability
        items = new List<ItemData>();

        Dictionary<string, ItemSO> itemsMap = new Dictionary<string, ItemSO>();

        foreach (var item in cachedCombatItems)
            itemsMap[item.id] = item;

        foreach (var saved in data.items)
        {
            if (!itemsMap.TryGetValue(saved.id, out var so))
                continue;

            var newItem = new ItemData()
            {
                itemSO = so,
                quantity = saved.quantity,

                instanceId = saved.instanceId,   
                durability = saved.durability,   
                isEquiped = saved.isEquiped     
            };

            items.Add(newItem);

            // Если предмет был экипирован — восстанавливаем
            if (newItem.isEquiped)
            {
                if (so is WeaponSO)
                    EquipWeapon(newItem);

                if (so is ShieldSO)
                    EquipShield(newItem);
            }
        }

        Notify();
    }

    // возвращаем созданный ItemData
    public void AddCombatItemToInventory(ItemData data, float durability = 100f)
    {
        if (data.itemSO == null)
            return;

        if (data.instanceId == null)
            data.instanceId = Guid.NewGuid().ToString();   

        data.durability = durability;   
        
        AddItemToInventory(data);
    }



    private void EquipWeapon(ItemData data)
    {
        weaponSetter.ResetWeapon();

        ICombatItem obj = GetWeaponObject(data);
        weaponSetter.SetWeapon(obj as IWeapon);
    }

    private void EquipShield(ItemData data)
    {
        weaponSetter.ResetShield();

        ICombatItem obj = GetWeaponObject(data);
        weaponSetter.SetShield(obj as IShield);
    }



    // Пулл объектов: возвращаем GameObject для экипировки
    public ICombatItem GetWeaponObject(ItemData data)
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

    public override void UseItem(ItemData data)
    {
        if (data == null || weaponSetter == null) return;

        if (data.itemSO is WeaponSO)
        {
            EquipWeapon(data);
            return;
        }

        if(data.itemSO is ShieldSO)
        {
            EquipShield(data);
        }
           
    }


}