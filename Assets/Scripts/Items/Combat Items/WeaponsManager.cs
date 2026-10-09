using System;
using System.Collections.Generic;
using UnityEngine;


public class WeaponsManager : MonoBehaviour
{
    [Header("All registered combat items")]
    //public List<CombatItemInstance> instances = new List<CombatItemInstance>();
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