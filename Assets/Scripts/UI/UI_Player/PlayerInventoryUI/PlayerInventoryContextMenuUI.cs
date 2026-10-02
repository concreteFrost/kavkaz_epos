using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Управляет контекстным меню выбранного предмета инвентаря.</summary>
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

    public Action<ItemData> ContextMenuClosed; 
    public Action UpdateQuickSlotsInfo; 
    public Action ItemDestroyed;
    public Action ItemEquiped;

    IInventoryUI quickAccessInventory;
    CharacterConsumeController consumableController;


    /// <summary>Инициализирует меню и связывает его кнопки с действиями.</summary>
    /// <param name="consumableController">Контроллер для использования расходуемых предметов.</param>
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

    /// <summary>Устанавливает инвентарь для быстрого доступа и выполнения действий с предметами.</summary>
    /// <param name="inv">Инвентарь, связанный с этим контекстным меню.</param>
    public void SetCurrentInventory(IInventoryUI inv)
    {
        quickAccessInventory = inv;

    }

    private void SetContextButtons(ItemSO item)
    {


        useBtn.gameObject.SetActive(item.CanUse());
        equipBtn.gameObject.SetActive(item.CanEquip());
        addToSlotBtn.gameObject.SetActive(item.CanAddToSlot());
        removeFromSlotBtn.gameObject.SetActive(item.CanRemoveFromSlot());
        destroyBtn.gameObject.SetActive(item.CanDestroy());
    }


    #region Button Actions
    /// <summary>Добавляет текущий предмет в быстрый доступ и обновляет интерфейс быстрых слотов.</summary>
    private void AddFromContext()
    {
        quickAccessInventory.AddToQuickAccess(currentItem);
        UpdateQuickSlotsInfo?.Invoke();
    }

    /// <summary>Удаляет предмет из быстрого доступа и обновляет интерфейс быстрых слотов.</summary>
    /// <param name="item">Предмет, который нужно удалить из быстрого доступа.</param>
    private void RemoveItem(ItemData item)
    {
        quickAccessInventory.RemoveFromQuickAccess(item);
        UpdateQuickSlotsInfo?.Invoke();
    }

    /// <summary>Передаёт текущий предмет назначенному инвентарю и вызывает событие экипировки.</summary>
    private void EquipItemFromContext()
    {
        quickAccessInventory.UseItem(currentItem);
        ItemEquiped?.Invoke();
        //GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }


    /// <summary>Начинает использование текущего расходуемого предмета и переключает состояние игры на Game.</summary>
    private void ConsumeItemFromContext()
    {
        consumableController.StartConsumeFromContext(currentItem as ConsumableData);
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }

    /// <summary>Удаляет текущий предмет из инвентаря, закрывает меню и вызывает событие удаления предмета.</summary>
    private void DestroyItemFromContextMenu()
    {
        quickAccessInventory.RemoveFromInventory(currentItem);

        HideContextMenu(true);
        ItemDestroyed?.Invoke();

    }
    #endregion

    /// <summary>Удаляет текущий предмет из быстрого доступа.</summary>
    private void RemoveFromContext() => RemoveItem(currentItem);

    /// <summary>Удаляет нажатый предмет из быстрого доступа.</summary>
    /// <param name="d">Предмет для удаления.</param>
    public void RemoveOnItemClick(ItemData d) => RemoveItem(d);

    /// <summary>Очищает прежние обработчики кнопки, назначает указанное действие и закрывает меню после нажатия.</summary>
    /// <param name="btn">Кнопка, обработчики которой будут заменены.</param>
    /// <param name="action">Действие, которое нужно выполнить при нажатии кнопки.</param>
    void SetupAction(Button btn, Action action)
    {
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
            HideContextMenu(true);
        });
    }

    /// <summary>Скрывает меню, сбрасывает текущий предмет и при необходимости уведомляет подписчиков.</summary>
    /// <param name="invokeEvent">Нужно ли вызывать событие закрытия меню.</param>
    public void HideContextMenu(bool invokeEvent)
    {
        if (invokeEvent)
            ContextMenuClosed?.Invoke(currentItem);

        wrapper.SetActive(false);
        currentItem = null;
    }

    /// <summary>Показывает меню для допустимого предмета в указанной локальной позиции; иначе закрывает меню.</summary>
    /// <param name="data">Предмет, для которого нужно показать контекстное меню.</param>
    /// <param name="position">Позиция меню в локальных координатах интерфейса.</param>
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
        SetContextButtons(data.itemSO);

        position.y -= 90;
        _rectTransform.localPosition = position;

        UINavigationUtils.ClampVerticalNavigation(allSelectables);
        var fistActiveSelectable = UINavigationUtils.GetFirstActive(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(fistActiveSelectable));
    }

    /// <summary>Проверяет, можно ли открыть меню для переданного предмета.</summary>
    /// <param name="data">Предмет для проверки.</param>
    /// <returns>True, если предмет не равен null и его ItemSO имеет другой ID, чем ItemSO текущего предмета; иначе false.</returns>
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
                return false; 
            }
        }
        return true;
    }

}
