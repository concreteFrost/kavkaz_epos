using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeStationUI : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private WeaponUpgradeCardUI cardPrefab;
    [SerializeField] private Transform cardsParent;

    private readonly List<WeaponUpgradeCardUI> cardPool = new();

    private WeaponUpgradeStation station;
    private IPlayerInteractor interactor;

    #region Selectable
    private List<Selectable> selectablePanels = new List<Selectable>();

    private Selectable currentSelected;

    public Selectable CurrentSelected { get => currentSelected; set => currentSelected = value; }

    #endregion

    internal void Close()
    {
        station = null;
        interactor = null;

        foreach (var card in cardPool)
            card.gameObject.SetActive(false);

        wrapper.SetActive(false);
    }

    internal void Show(
        WeaponUpgradeStation station,
        IPlayerInteractor interactor)
    {
        this.station = station;
        this.interactor = interactor;

        PopulateCards();
        wrapper.SetActive(true);
    }

    private void PopulateCards(int preferredSelectionIndex = 0)
    {
        if (station == null || interactor?.WeaponInventory == null)
        {
            HideUnusedCards(0);
            return;
        }

        int cardIndex = 0;
        selectablePanels.Clear();

        //достаём карты из пула
        foreach (CombatItemData combatData in interactor.WeaponInventory.items)
        {
            if (combatData is not WeaponData data) continue;
            //показываем только оружие
            if (data == null || data.itemSO is not WeaponSO)
                continue;

            if (data.WeaponLevel == 10)
                continue;

            WeaponUpgradeCardUI card = GetOrCreateCard(cardIndex);

            var materialInfo = station.GetNextTier(data.WeaponLevel);

            var materialCount = station.GetMaterialCount(
                interactor.ConsumableInventory,
                materialInfo.targetItem.id);

            CardTierData info = new CardTierData(
                data,
                data.itemSO.itemName,
                data.WeaponLevel,
                data.WeaponLevel + 1,
                data.GetWeaponDamageWithLevel(data.WeaponLevel),
                data.GetWeaponDamageWithLevel(data.WeaponLevel + 1),
                materialInfo.targetItem.itemName,
                materialInfo.requiredQuantity,
                materialCount,
                materialInfo.price,
                interactor.MoneyManager.CurrentBalance);

            card.SetCardData(info, HandleUpgradeRequested);
            card.ToggleButton(station.CanUpgrade(
                data,
                interactor.MoneyManager,
                interactor.ConsumableInventory));
            card.gameObject.SetActive(true);

            cardIndex++;
            selectablePanels.Add(card.GetComponentInChildren<Selectable>());
        }


        //скрываем остальные карты
        HideUnusedCards(cardIndex);
        HighlightAt(preferredSelectionIndex);
        
    }

    private void HideUnusedCards(int firstUnusedIndex)
    {
        for (int i = firstUnusedIndex; i < cardPool.Count; i++)
            cardPool[i].gameObject.SetActive(false);
    }

    private WeaponUpgradeCardUI GetOrCreateCard(int index)
    {
        while (cardPool.Count <= index)
        {
            WeaponUpgradeCardUI card = Instantiate(cardPrefab, cardsParent);
            card.gameObject.SetActive(false);
            cardPool.Add(card);
        }

        return cardPool[index];
    }

    private void HandleUpgradeRequested(WeaponData data)
    {
        if (station == null || interactor == null || data == null)
            return;

        int selectedIndex = cardPool.FindIndex(card =>
            card != null && card.gameObject.activeSelf && card.Data == data);

        station.TryUpgrade(
            data,
            interactor.MoneyManager,
            interactor.ConsumableInventory);

        PopulateCards(Mathf.Max(selectedIndex, 0));
    }

    #region Gamepad Panels Interaction
    private void HighlightAt(int index)
    {
        UINavigationUtils.ClampVerticalNavigation(selectablePanels);
        if (selectablePanels.Count == 0) return;

        index = Mathf.Clamp(index, 0, selectablePanels.Count - 1);
        currentSelected = selectablePanels[index];
        StartCoroutine(UINavigationUtils.SelectWithDelay(currentSelected.gameObject));
    }



    #endregion
}
