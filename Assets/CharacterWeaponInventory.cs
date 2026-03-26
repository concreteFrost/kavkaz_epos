using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponInventory : QuickAccessInventory
{
    [Header("Starter Set")]
    public CombatInventorySO starterSet;

    private HumanoidWeaponSetter weaponSetter;
    private Dictionary<string, GameObject> weaponPool = new Dictionary<string, GameObject>();

    public void Init(HumanoidWeaponSetter setter)
    {
        base.BaseInit();
        weaponSetter = setter;

        if (starterSet == null) return;


        if (starterSet.initialWeapon != null)
        {
            var weaponSo = starterSet.initialWeapon.GetComponent<Weapon>().WeaponData();
            AddWeaponToInventory(weaponSo);
            EquipStartingWeapon(weaponSo);
        }
           
        if (starterSet.initialShield != null)
        {
            var shieldSo = starterSet.initialShield.GetComponent<Shield>().ShieldData();    
            AddShieldToInventory(shieldSo);
            EquipStartingShield(shieldSo);
        }
           
    }

    public void AddWeaponToInventory(ItemSO weaponSO)
    {
        if (weaponSO == null) return;

        ItemData data = new ItemData { itemSO = weaponSO, quantity = 1 };
        AddItemToInventory(data);
    }

    public void AddShieldToInventory(ItemSO shieldSO)
    {
        if (shieldSO == null) return;

        ItemData data = new ItemData { itemSO = shieldSO, quantity = 1 };
        AddItemToInventory(data);
       
    }

    private void EquipStartingWeapon(ItemSO weaponSO)
    {
        GameObject obj = GetWeaponObject(weaponSO.id);
        var weapon = obj.GetComponent<IWeapon>();

        weaponSetter.SetWeapon(weapon);
    }

    private void EquipStartingShield(ItemSO shieldSO)
    {
        GameObject obj = GetWeaponObject(shieldSO.id);
        var shield = obj.GetComponent<IShield>();

        weaponSetter.SetShield(shield);
    }



    // Пулл объектов: возвращаем GameObject для экипировки
    public GameObject GetWeaponObject(string id)
    {
        if (!weaponPool.TryGetValue(id, out var obj))
        {
            var weapons = Resources.Load<WeaponDataBaseSO>("DataBases/DataBase_Weapons");
           
            var targetWeapon = weapons.Get(id);
            obj = Instantiate(targetWeapon);
            var combatItem = obj.GetComponent<CombatItem>();
            combatItem.Init();
            weaponPool[id] = obj;
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

        GameObject obj = GetWeaponObject(data.itemSO.id);

        var weapon = obj.GetComponent<IWeapon>();
        if (weapon != null)
        {
            weaponSetter.SetWeapon(weapon);
            return;
        }

        var shield = obj.GetComponent<IShield>();
        if (shield != null)
        {
            weaponSetter.SetShield(shield);
        }
    }
}