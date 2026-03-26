using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatItemInstance
{
    public CombatItemData data;
    public CombatItem item;

    public CombatItemInstance(CombatItemData data, CombatItem item)
    {
        this.data = data;
        this.item = item;
    }

    public CombatItemData SaveData()
    {
        return item.SaveCombatItemData();
    }
}

public class WeaponsManager : MonoBehaviour
{
    [Header("All registered combat items")]
    public List<CombatItemInstance> instances = new List<CombatItemInstance>();
    [SerializeField] private WeaponDataBaseSO weaponDatabase;

    // Инициализация: регистрируем все предметы на сцене
    public void Init()
    {
        //instances.Clear();

        //// Все CombatItem на сцене
        //var children = GetComponentsInChildren<CombatItem>();
        //foreach (var child in children)
        //{
        //    child.Init();
        //    Register(child, true);
        //}

        //CreateStarterItems();
    }

    // Регистрируем предмет
   
}