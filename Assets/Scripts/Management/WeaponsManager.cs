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
    public void Register(CombatItem item, bool isStaticItem)
    {
        // Генерация уникального ID
        var id = isStaticItem ? item.GetComponent<UniqueId>().uniqueId : Guid.NewGuid().ToString();

        // Создаём данные
        var data = new CombatItemData
        {
            initialPosition = new float[] { item.InitialPosition.x, item.InitialPosition.y, item.InitialPosition.z },
            currentPosition = new float[] { item.InitialPosition.x, item.InitialPosition.y, item.InitialPosition.z },
            initialRotation = new float[] {item.InitialRotation.x, item.InitialRotation.y, item.InitialRotation.z },    
            currentRotation = new float[] { item.InitialRotation.x, item.InitialRotation.y, item.InitialRotation.z },
            itemSOid = item.GetDataId(),
            itemInstanceId = id,
            breakdownThreshold = 100f,
            ownerId = item.Owner != null ? item.Owner.CollectorId() : null,
            isStaticItem = isStaticItem
        };

        // Привязываем данные к объекту
        item.SetCombatItemData(data);

        // Добавляем в список
        instances.Add(new CombatItemInstance(data, item));
    }

    // Создание стартовых предметов для новой игры
    public void CreateStarterItems()
    {
        var inventories = FindObjectsByType<BaseCombatInventory>(FindObjectsSortMode.None);

        foreach (var inventory in inventories)
        {
            var starterSet = inventory.starterSet;
            if (starterSet == null) continue;

            // Стартовое оружие
            if (starterSet.initialWeapon != null)
            {
                var go = Instantiate(starterSet.initialWeapon);
                var weapon = go.GetComponent<Weapon>();
                weapon.Init();
                weapon.AssignToOwner(inventory.Collector);
                weapon.SetBreakdownEnabled(inventory.enableWeponBreakdown);
                inventory.SetWeapon(weapon);

                Register(weapon, false);
            }
            else
            {
                inventory.SetWeapon(null);
            }

            // Стартовый щит
            if (starterSet.initialShield != null)
            {
                var go = Instantiate(starterSet.initialShield);
                var shield = go.GetComponent<Shield>();
                shield.Init();
                shield.AssignToOwner(inventory.Collector);
                shield.SetBreakdownEnabled(inventory.enableWeponBreakdown);
                inventory.SetShield(shield);

                Register(shield, false);
            }
            else
            {
                inventory.SetShield(null);
            }
        }
    }

    // Сохраняем данные всех предметов
    public List<CombatItemData> SaveCombatItemData()
    {
        var list = new List<CombatItemData>();

        foreach (var instance in instances)
        {
            list.Add(instance.SaveData());
        }

        return list;
    }

    public void LoadItemData(List<CombatItemData> savedItems)
    {
        var inventories = FindObjectsByType<BaseCombatInventory>(FindObjectsSortMode.None);

        // 1. Удаляем все динамические предметы (не сценные)
        for (int i = instances.Count - 1; i >= 0; i--)
        {
            var inst = instances[i];
            if (!inst.data.isStaticItem)
            {
                Destroy(inst.item.gameObject);
                instances.RemoveAt(i);
            }
        }

        // 2. Восстанавливаем / создаём предметы из сейва
        foreach (var data in savedItems)
        {
            // Если уже есть на сцене (сценный предмет)
            var existing = instances.Find(x => x.data.itemInstanceId == data.itemInstanceId);

            CombatItem item;

            if (existing != null)
            {
                item = existing.item;
                item.LoadData(data);
            }
            else
            {
                // Динамический предмет = создаём
                var prefab = weaponDatabase.Get(data.itemSOid);
                if (prefab == null)
                {
                    Debug.LogWarning($"Item prefab not found for SO ID {data.itemSOid}");
                    continue;
                }

                item = Instantiate(prefab).GetComponent<CombatItem>();
                item.Init();
                item.LoadData(data);
                Register(item, false);
            }

            // 3. Назначаем владельца
            if (data.ownerId != null)
            {
                foreach (var inventory in inventories)
                {
                    if (inventory.Collector.CollectorId() == data.ownerId)
                    {
                        // Сбрасываем текущее оружие/щит
                        if (item is IWeapon)
                            inventory.ResetWeapon();
                        else if (item is IShield)
                            inventory.ResetShield();

                        // Назначаем предмет владельцу
                        item.AssignToOwner(inventory.Collector);
                        break; // нашли владельца — выходим из цикла
                    }
                }
            }
        }
    }
}