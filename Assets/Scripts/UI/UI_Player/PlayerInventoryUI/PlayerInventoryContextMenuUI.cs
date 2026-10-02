using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryContextMenuUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Button addToSlotBtn;
    [SerializeField] Button removeFromSlotBtn;
    [SerializeField] Button useBtn;
    [SerializeField] Button equipBtn;
    [SerializeField] Button destroyBtn;

    RectTransform _rectTransform;
    ItemData currentItem;

    public List<Selectable> allSelectables = new List<Selectable>();

    public Action<ItemData> ContextMenuClosed; //�������� ����� �� �������� ������� � ���������
    public Action UpdateQuickSlotsInfo; //��������� ������� ����� � �������� ���������
    public Action ItemDestroyed;
    public Action ItemEquiped;

    IInventoryUI quickAccessInventory;
    CharacterConsumeController consumableController;


    public void Init(CharacterConsumeController consumableController)
    {
        this.consumableController = consumableController;
        allSelectables.AddRange(wrapper.GetComponentsInChildren<Button>());
        _rectTransform = wrapper.GetComponent<RectTransform>();

        SetupAction(addToSlotBtn, AddFromContext);
        SetupAction(removeFromSlotBtn, RemoveFromContext);
        SetupAction(useBtn, ConsumeItemFromContext);
        SetupAction(equipBtn, EquipItemFromContext);
        SetupAction(destroyBtn, DestroyItemFromContextMenu);


    }

    public void SetCurrentInventory(IInventoryUI inv)
    {
        quickAccessInventory = inv;

    }

    private void SetContextButtons()
    {
        bool isWeaponInventory = quickAccessInventory != null &&
            typeof(CharacterWeaponInventory).IsAssignableFrom(quickAccessInventory.GetType());
        bool isConsumableInventory = quickAccessInventory != null &&
            typeof(PlayerConsumableInventory).IsAssignableFrom(quickAccessInventory.GetType());
        bool currentIsWeapon = currentItem is WeaponData;

        useBtn.gameObject.SetActive(isConsumableInventory);
        equipBtn.gameObject.SetActive(isWeaponInventory && currentIsWeapon && !currentItem.isEquiped);
        addToSlotBtn.gameObject.SetActive(!isWeaponInventory);
        removeFromSlotBtn.gameObject.SetActive(!isWeaponInventory);
    }


    #region Button Actions
    /// <summary>
    /// ��������� ������� ������� � ������� ���� �� ������������ ����.
    /// </summary>
    private void AddFromContext()
    {
        quickAccessInventory.AddToQuickAccess(currentItem);
        UpdateQuickSlotsInfo?.Invoke();
    }

    private void RemoveItem(ItemData item)
    {
        quickAccessInventory.RemoveFromQuickAccess(item);
        UpdateQuickSlotsInfo?.Invoke();
    }

    private void EquipItemFromContext()
    {
        quickAccessInventory.UseItem(currentItem);
        ItemEquiped?.Invoke();
        //GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }


    private void ConsumeItemFromContext()
    {
        consumableController.StartConsumeFromContext(currentItem as ConsumableData);
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }

    private void DestroyItemFromContextMenu()
    {
        quickAccessInventory.RemoveFromInventory(currentItem);

        HideContextMenu(true);
        ItemDestroyed?.Invoke();

    }
    #endregion

    /// <summary>
    /// ������� ������� ������� �� �������� ����� ����� ����������� ����.
    /// </summary>
    private void RemoveFromContext() => RemoveItem(currentItem);

    /// <summary>
    /// ������� ��������� ������� �� �������� ����� �� ������� �� ��� ������.
    /// </summary>
    /// <param name="d">������ ��������, ������� ����� ������� �� ������� ������.</param>
    public void RemoveOnItemClick(ItemData d) => RemoveItem(d);

    /// <summary>
    /// ��������� �������� ��� ������ ������������ ����.
    /// ����� ����������� ������ ����������� ������� ��� ����������.
    /// ����� ���������� �������� ������������� �������� ����.
    /// </summary>
    /// <param name="btn">������, ��� ������� ������� ��������.</param>
    /// <param name="action">�����, ���������� ��� �������.</param>
    void SetupAction(Button btn, Action action)
    {
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
            HideContextMenu(true);
        });
    }

    /// <summary>
    /// �������� ����������� ����,
    /// ���������� ������� ��������� �������
    /// � ���������� ����������� � ��������.
    /// </summary>
    public void HideContextMenu(bool invokeEvent)
    {
        if (invokeEvent)
            ContextMenuClosed?.Invoke(currentItem);

        wrapper.SetActive(false);
        currentItem = null;
    }

    /// <summary>
    /// ���������� ����������� ���� ��� ���������� ��������
    /// � �������� ������� �� ������.
    /// ���� ���� �� ������ ���� �������� � �������� ���.
    /// </summary>
    /// <param name="data">������ ��������, ��� �������� ����������� ����.</param>
    /// <param name="position">������� ����������� ���� (��������� ����������).</param>
    public void ShowContextMenu(ItemData data, Vector2 position)
    {
        if(data.itemSO is KeyItemSO)
        {
            HideContextMenu(true);
            return;

        }
        if (!WillShowContextMenu(data))
        {
            HideContextMenu(true);
            return;
        }

        currentItem = data;

        wrapper.SetActive(true);
        SetContextButtons();

        position.y -= 90;
        _rectTransform.localPosition = position;

        UINavigationUtils.ClampVerticalNavigation(allSelectables);
        var fistActiveSelectable = UINavigationUtils.GetFirstActive(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(fistActiveSelectable));
    }

    /// <summary>
    /// ����������, ����� �� ���������� ����������� ���� ��� ���������� ��������.
    /// ���� �� ������������, ���� ��� ��� ������� ��� ���� �� ������ ��������.
    /// </summary>
    /// <param name="data">������ �������� ��� ��������.</param>
    /// <returns>
    /// True � ���� ���� ������� ��������;  
    /// False � ���� ���� ��� ������� ��� ����� ��������.
    /// </returns>
    private bool WillShowContextMenu(ItemData data)
    {
        if (data == null)
        {
            Debug.Log("data is null");
            return false;
        }

        if (currentItem != null)
        {
            if (currentItem.itemSO.id == data.itemSO.id)
            {
                return false; // �� ���������� ����, ���� ��� �� �������
            }
        }
        return true;
    }

}
