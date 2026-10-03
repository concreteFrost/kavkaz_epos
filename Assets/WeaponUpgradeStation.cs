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
        if(picker is not IPlayerInteractor)
        {
            return;
        }

       
        //GameStateManager.GameStateChanged?.Invoke(GameState.WeaponUpgrader);
        WeaponUpgradeStationInterated?.Invoke(this, picker as IPlayerInteractor);
    }


    public void TryUpgrade(Weapon targetWeapon, PlayerMoneyManager moneyManager)
    {
        if(targetWeapon == null)
        {
            Debug.Log("target weapon is not assigned");
            return;
        }

        if(moneyManager == null)
        {
            Debug.Log("money manager is not assigned");
            return;
        }

        int weaponLevel = targetWeapon.GetItemData().WeaponLevel;

        if(weaponLevel >= 10)
        {
            Debug.Log("weapon level is on maximum");
            return;
        }

        var nextTier = GetNextTier(weaponLevel);

        if (nextTier == null) return;

        if (!HasEnoughMoney(moneyManager.CurrentBalance, nextTier.price))
        {
            Debug.Log("not enough money");
            return;
        }

        moneyManager.TrySpendMoney(nextTier.price);
        targetWeapon.GetItemData().Upgrade();


    }

    private bool HasEnoughMoney(float inWallet, float targetAmount) => inWallet >= targetAmount;

    private WeaponUpgradeTier GetNextTier(int currentLevel)
    {
        if(settingsSO == null)
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
