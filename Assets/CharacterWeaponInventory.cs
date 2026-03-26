using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponInventory : QuickAccessInventory
{
    [Header("Starter Set")]
    public CombatInventorySO starterSet;

    private HumanoidWeaponSetter weaponSetter;
    private Dictionary<string, GameObject> weaponPool = new Dictionary<string, GameObject>();
    private WeaponDataBaseSO weaponDataBaseSO;

    public void Init(HumanoidWeaponSetter setter)
    {
        base.BaseInit();
        weaponSetter = setter;
        
        var resources = Resources.Load<WeaponDataBaseSO>("DataBases/DataBase_Weapons");
        weaponDataBaseSO = resources;

        if (starterSet == null) return;

        if (starterSet.initialWeapon != null)
        {
            var weaponSo = starterSet.initialWeapon.GetComponent<Weapon>().WeaponData();
            var weaponData = AddCombatItemToInventory(weaponSo);
            EquipStartingWeapon(weaponData);
        }

        if (starterSet.initialShield != null)
        {
            var shieldSo = starterSet.initialShield.GetComponent<Shield>().ShieldData();
            var shieldData = AddCombatItemToInventory(shieldSo);
            EquipStartingShield(shieldData);
        }

    }

    // возвращаем созданный ItemData
    public ItemData AddCombatItemToInventory(ItemSO weaponSO, float durability = 100f)
    {
        if (weaponSO == null) return null;

        ItemData data = new ItemData
        {
            itemSO = weaponSO,
            quantity = 1,
            instanceId = Guid.NewGuid().ToString(),
            durability = durability
        };

        AddItemToInventory(data);
        return data; // возвращаем созданный экземпляр
    }



    private void EquipStartingWeapon(ItemData data)
    {
        GameObject obj = GetWeaponObject(data);
        var weapon = obj.GetComponent<IWeapon>();

        weaponSetter.SetWeapon(weapon);
    }

    private void EquipStartingShield(ItemData data)
    {
        GameObject obj = GetWeaponObject(data);
        var shield = obj.GetComponent<IShield>();

        weaponSetter.SetShield(shield);
    }



    // Пулл объектов: возвращаем GameObject для экипировки
    public GameObject GetWeaponObject(ItemData data)
    {
        if (!weaponPool.TryGetValue(data.instanceId, out var obj))
        {
            var template = weaponDataBaseSO.Get(data.itemSO.id);
            obj = Instantiate(template);
            var combatItem = obj.GetComponent<CombatItem>();
            combatItem.Init(data);   
            weaponPool[data.instanceId] = obj;
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnWeaponObject(string id)
    {
        if (weaponPool.TryGetValue(id, out var obj))
        {
            obj.SetActive(false);
        }
    }

    public override void UseItem(ItemData data)
    {
        if (data == null || weaponSetter == null) return;

        GameObject obj = GetWeaponObject(data);

        var weapon = obj.GetComponent<IWeapon>();
       
        if (weapon != null)
        {

            weaponSetter.ResetWeapon();
            weaponSetter.SetWeapon(weapon);

            return;
        }

        var shield = obj.GetComponent<IShield>();
        
        if (shield != null)
        {
            weaponSetter.ResetShield();
            weaponSetter.SetShield(shield);
        }
    }
}