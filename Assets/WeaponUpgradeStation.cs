using UnityEngine;
using System;

public class WeaponUpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private WeaponUpgradeSettingsSO settingsSO;


    #region IInteractable Contract
    public bool HasInteracted { get; set; }

    public string ActionText() => "Взаимодействовать";

    public bool CanInteract() => true;

    public string InteractionName() => "Кузница";

    public ItemInteractionType InteractType() => ItemInteractionType.Item;

    #endregion

    public static Action<WeaponUpgradeStation, IPlayerInteractor> WeaponUpgradeStationInterated;

   
    public void Interact(IInteractor picker)
    {
        if (picker is not IPlayerInteractor playerInteractor)
        {
            return;
        }

       
        //GameStateManager.GameStateChanged?.Invoke(GameState.WeaponUpgrader);
        WeaponUpgradeStationInterated?.Invoke(this, playerInteractor);
    }

    public bool CanUpgrade(
        WeaponData targetWeapon,
        PlayerMoneyManager moneyManager,
        PlayerConsumableInventory consumableInventory)
    {
        int weaponLevel = targetWeapon.WeaponLevel;

        if (weaponLevel >= 10)
            return false;

        WeaponUpgradeTier nextTier = GetNextTier(weaponLevel);

        if (nextTier == null
            || nextTier.targetItem == null
            || nextTier.requiredQuantity <= 0
            || nextTier.price <= 0)
            return false;

        int materialCount = GetMaterialCount(consumableInventory, nextTier.targetItem.id);

        return moneyManager.CurrentBalance >= nextTier.price
            && materialCount >= nextTier.requiredQuantity;
    }

    public bool TryUpgrade(
        WeaponData weaponData,
        PlayerMoneyManager moneyManager,
        PlayerConsumableInventory consumableInventory)
    {
        if (weaponData == null || moneyManager == null || consumableInventory == null)
            return false;

       
        if (!CanUpgrade(weaponData, moneyManager, consumableInventory))
            return false;

        WeaponUpgradeTier nextTier = GetNextTier(weaponData.WeaponLevel);
        if (nextTier == null || nextTier.targetItem == null)
            return false;

        // CanUpgrade проверяет наличие обоих ресурсов; TrySpendMoney повторно
        // подтверждает, что кошелёк согласен выполнить списание.
        if (!moneyManager.TrySpendMoney(nextTier.price))
            return false;

        if (!consumableInventory.TryConsumeItem(nextTier.targetItem.id, nextTier.requiredQuantity))
            return false;

        weaponData.Upgrade();
        return true;
    }

    public int GetMaterialCount(PlayerConsumableInventory inventory, string itemId)
    {
        ConsumableData material = inventory.items.Find(item =>
            item != null && item.itemSO != null && item.itemSO.id == itemId);

        return material?.quantity ?? 0;
    }

    public WeaponUpgradeTier GetNextTier(int currentLevel)
    {
        if (settingsSO == null || settingsSO.tiers == null)
        {
            Debug.Log("settingSO were not assigned");
            return null;
        }

        var targetLevel = currentLevel + 1;

        var match = settingsSO.tiers.Find((x) => x.targetLevel == targetLevel);

        if(match == null)
        {
            Debug.Log("no matching levels for this weapon");
            return null;
        }

        return match;
    }

}
