using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryContextMenuUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Button addToSlotBtn;
    [SerializeField] Button removeFromSlotBtn;
    [SerializeField] Button useBtn;

    RectTransform _rectTransform;
    ItemData currentItem;

    public List<Selectable> allSelectables = new List<Selectable>();

    public Action<ItemData> OnContextMenuClosed; //вызывает фокус на активный предмет в инвентаре
    public Action UpdateQuickSlotsInfo; //обновляет быстрые слоты в основном инвентаре

    QuickAccessInventory quickAccessInventory;
    CharacterConsumeController consumableController;

    public void Init(CharacterConsumeController consumableController)
    {
        this.consumableController = consumableController;   
        allSelectables.AddRange(wrapper.GetComponentsInChildren<Button>());
        _rectTransform = wrapper.GetComponent<RectTransform>();
        SetupAction(addToSlotBtn, AddFromContext);
        SetupAction(removeFromSlotBtn, RemoveFromContext);
        SetupAction(useBtn, UseItemFromContext);

    }

    public void SetCurrentInventory(QuickAccessInventory inv)
    {
        quickAccessInventory = inv;

        SetContextButtons();    
    }

    private void SetContextButtons()
    {
        useBtn.gameObject.SetActive(quickAccessInventory is CharacterConsumableInventory);
    }

    /// <summary>
    /// Добавляет текущий предмет в быстрый слот из контекстного меню.
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

    private void UseItemFromContext()
    {
        if (currentItem == null) return;

        consumableController.StartConsumeFromContext(currentItem);
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }

    /// <summary>
    /// Удаляет текущий предмет из быстрого слота через контекстное меню.
    /// </summary>
    private void RemoveFromContext() => RemoveItem(currentItem);

    /// <summary>
    /// Удаляет указанный предмет из быстрого слота по нажатию на его иконку.
    /// </summary>
    /// <param name="d">Данные предмета, который нужно удалить из быстрых слотов.</param>
    public void RemoveOnItemClick(ItemData d)=> RemoveItem(d);

    /// <summary>
    /// Назначает действие для кнопки контекстного меню.
    /// Перед добавлением нового обработчика удаляет все предыдущие.
    /// После выполнения действия автоматически скрывает меню.
    /// </summary>
    /// <param name="btn">Кнопка, для которой задаётся действие.</param>
    /// <param name="action">Метод, вызываемый при нажатии.</param>
    void SetupAction(Button btn, Action action)
    {
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
            HideContextMenu();
        });
    }

    /// <summary>
    /// Скрывает контекстное меню,
    /// сбрасывает текущий выбранный предмет
    /// и уведомляет подписчиков о закрытии.
    /// </summary>
    public void HideContextMenu()
    {
        OnContextMenuClosed?.Invoke(currentItem);
        wrapper.SetActive(false);
        currentItem = null;
    }

    /// <summary>
    /// Отображает контекстное меню для указанного предмета
    /// в заданной позиции на экране.
    /// Если меню не должно быть показано — скрывает его.
    /// </summary>
    /// <param name="data">Данные предмета, для которого открывается меню.</param>
    /// <param name="position">Позиция отображения меню (локальные координаты).</param>
    public void ShowContextMenu(ItemData data, Vector2 position)
    {
        if (!WillShowContextMenu(data))
        {
            HideContextMenu();
            return;
        }

        currentItem = data;
       

        wrapper.SetActive(true);

        position.y -= 90;
        _rectTransform.localPosition = position;

        UINavigationUtils.ClampVerticalNavigation(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(allSelectables[0].gameObject));
    }

    /// <summary>
    /// Определяет, нужно ли отображать контекстное меню для указанного предмета.
    /// Меню не отображается, если оно уже открыто для того же самого предмета.
    /// </summary>
    /// <param name="data">Данные предмета для проверки.</param>
    /// <returns>
    /// True — если меню следует показать;  
    /// False — если меню уже открыто для этого предмета.
    /// </returns>
    private bool WillShowContextMenu(ItemData data)
    {
        
        if (currentItem != null)
        {
            if (currentItem.itemSO.id == data.itemSO.id)
            {
                return false; // не показываем меню, если тот же предмет
            }
        }
        return true;
    }

}
