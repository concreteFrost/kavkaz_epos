using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum InventorySection
{
    Weapons = 0,
    Magic = 1,
    Consumables = 2,
}

public class PlayerInventoryUI : MonoBehaviour
{
    private CharacterStatsController statsController;
    private PlayerInventoryContextMenuUI contextMenu;
    private ItemDescriptionPanelUI descriptionPanel;

    [SerializeField] GameObject mainWrapper;
    [SerializeField] GameObject itemCellPrefab;

    [SerializeField] Transform weaponCellsContainer;
    [SerializeField] Transform cellsContainer;

    [SerializeField] Scrollbar scrollSlider;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Button magicSectionBtn;
    [SerializeField] Button consumableSectionBtn;
    [SerializeField] Button weaponsSectionBtn;

    private PlayerConsumableInventory consumableInventory;
    private CharacterSpellInventory spellInventory;
    private CharacterWeaponInventory weaponInventory;

    private List<InventoryItemUI> weaponItems = new List<InventoryItemUI>();
    private List<InventoryItemUI> slotItems = new List<InventoryItemUI>();
    private int totalCellsToInit = 70;

    [SerializeField] Transform quickSlotsContainer;
    private List<InventoryItemUI> quickSlotItems = new List<InventoryItemUI>();

    private QuickAccessInventory currentInventory;
    Dictionary<InventorySection, QuickAccessInventory> inventories;

    private GridLayoutGroup grid;
    public bool IsOpened() => mainWrapper.activeInHierarchy;

    public InventorySection currentSection { get; private set; }

    // для переключения контроллером
    private List<InventorySection> sectionOrder = new List<InventorySection>
{
    InventorySection.Weapons,
    InventorySection.Magic,
    InventorySection.Consumables
};

    public void Init(ItemDescriptionPanelUI descriptionPanel,
        CharacterWeaponInventory weaponInventory,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory,
        PlayerInventoryContextMenuUI contextMenu,
        CharacterStatsController statsController)
    {
        this.descriptionPanel = descriptionPanel;
        this.contextMenu = contextMenu;
        this.statsController = statsController;

        this.consumableInventory = consumableInventory;
        this.weaponInventory = weaponInventory;
        this.spellInventory = spellInventory;

        contextMenu.ContextMenuClosed += OnContextMenuClosed;
        contextMenu.UpdateQuickSlotsInfo += OnQuickSlotsInfoUpdate;
        contextMenu.ItemDestroyed += OnItemDestroyed;
        contextMenu.ItemEquiped += OnItemEquipped;

        grid = cellsContainer.GetComponent<GridLayoutGroup>();

        //динамическое назначение инвентарей
        inventories = new Dictionary<InventorySection, QuickAccessInventory>
    {
        {InventorySection.Weapons, this.weaponInventory },
        {InventorySection.Magic,this.spellInventory },
        {InventorySection.Consumables, this.consumableInventory },
    };


        InitInventoryCells();
        InitQuickAccessCells();
        InitWeaponCells();

        BindSectionButtons();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
        contextMenu.ContextMenuClosed -= OnContextMenuClosed;
        contextMenu.UpdateQuickSlotsInfo -= OnQuickSlotsInfoUpdate;
        contextMenu.ItemDestroyed -= OnItemDestroyed;
        contextMenu.ItemEquiped -= OnItemEquipped;
    }

    /// <summary>
    /// Управление состоянием видимости инвентаря
    /// </summary>
    /// <param name="isVisible"></param>
    public void ToggleInventory(bool isVisible)
    {
        mainWrapper.SetActive(isVisible);
        descriptionPanel.ClearCommonItemInfo();

    }

    #region Buttons Event Binders/Removals
    /// <summary>
    /// Привязывает обработчики событий клика к кнопкам разделов для переключения между разделами инвентаря.
    /// </summary>
    private void BindSectionButtons()
    {
        RemoveButtonListeners();

        magicSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Magic));
        consumableSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Consumables));
        weaponsSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Weapons));
        //resourcesSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Resources));
    }

    /// <summary>
    /// Удаляет все обработчики событий клика для кнопок в разделах магии, расходных материалов и оружия.
    /// </summary>
    private void RemoveButtonListeners()
    {
        magicSectionBtn.onClick.RemoveAllListeners();
        consumableSectionBtn.onClick.RemoveAllListeners();
        weaponsSectionBtn.onClick.RemoveAllListeners();
        //resourcesSectionBtn.onClick.RemoveAllListeners();
    }
    #endregion

    #region Main Grid And Quick Slots Init
    /// <summary>
    /// Создает новый элемент ячейки инвентаря как дочерний элемент указанного контейнера.
    /// </summary>
    /// <param name="container">Родительский элемент ячейки</param>
    /// <param name="action">Действия по нажатие на ячейку</param>
    /// <returns></returns>
    private InventoryItemUI InstantiateInventoryCell(Transform container, Action<ItemData, Vector2> action)
    {
        GameObject go = Instantiate(itemCellPrefab, container);
        InventoryItemUI slotItem = go.GetComponent<InventoryItemUI>();

        slotItem.InitInInventory(action);
        slotItem.RemoveData();
        slotItem.FitToCell(grid.cellSize);
        slotItem.ItemOutlined += OnItemOutlined;

        return slotItem;
    }

    /// <summary>
    /// Динамически создает QuickSlotItemUI в контейнере
    /// </summary>
    private void InitInventoryCells()
    {
        //создание основной сетки
        for (int i = 0; i < totalCellsToInit; i++)
        {
            var newCell = InstantiateInventoryCell(cellsContainer, (item, pos) => contextMenu.ShowContextMenu(item, pos));
            slotItems.Add(newCell);
        }

    }
    /// <summary>
    /// Создает иконки для экипированых оружейных слотов
    /// </summary>
    private void InitWeaponCells()
    {
        for (int i = 0; i < 2; i++)
        {
            var newCell = InstantiateInventoryCell(weaponCellsContainer, (item, pos) => HandleUnequipItem(item, pos));
            weaponItems.Add(newCell);
        }
    }

    /// <summary>
    /// Динамически создает сетку быстрого доступа
    /// </summary>
    private void InitQuickAccessCells()
    {
        //создание сетки быстрого доступа
        for (int i = 0; i < QuickAccessInventory.QUICK_SLOTS_COUNT; i++)
        {
            GameObject go = Instantiate(itemCellPrefab, quickSlotsContainer);
            var data = go.GetComponent<InventoryItemUI>();
            data.InitInInventory((item, pos) => contextMenu.RemoveOnItemClick(item));

            quickSlotItems.Add(data);
        }
    }
    #endregion

    #region Grid And Quick Slots Redrawing Methods
    /// <summary>
    /// Отображает предметы в инвентаре и быстром доступе, которые актуальны для данной секции
    /// </summary>
    /// <param name="section"></param>
    public void GetSection(InventorySection section)
    {

        //descriptionPanel.HidePanel();

        currentSection = section;
        currentInventory = inventories[section];

        contextMenu.SetCurrentInventory(currentInventory);
        contextMenu.HideContextMenu(false);

        ClearCellsData();

        GetWeaponsInfo();
        GetSlotsInfo();
        GetQuickSlotsInfo();

        Canvas.ForceUpdateCanvases();          // важно
        scrollRect.verticalNormalizedPosition = 1f;

        // Преобразуем QuickSlotItemUI в Button
        var mainGridButtons = slotItems.Select(s => s.GetComponent<Selectable>()).ToList();
        // Настраиваем сеточную навигацию
        var weaponButtons = weaponCellsContainer.GetComponentsInChildren<Selectable>().ToList();
        UINavigationUtils.SetupGridNavigation(mainGridButtons, 5, weaponButtons);

        // Настраиваем вертикальную панель для оружия с учётом перехода на правую панель
        UINavigationUtils.ClampVerticalNavigation(weaponButtons, mainGridButtons);

        FocusFirstGridItem(null);

        SetButtonState(magicSectionBtn, section == InventorySection.Magic);
        SetButtonState(consumableSectionBtn, section == InventorySection.Consumables);
        SetButtonState(weaponsSectionBtn, section == InventorySection.Weapons); 
    }

    /// <summary>
    /// Показывает актуально информацию о предметах в быстром доступе
    /// </summary>
    private void GetQuickSlotsInfo()
    {
        quickSlotItems.ForEach((s) => s.RemoveData());

        for (int i = 0; i < currentInventory.GetQuickAccessData().Count; i++)
        {
            quickSlotItems[i].UpdateImageDate(currentInventory.GetQuickAccessData()[i], statsController);
        }
    }

    /// <summary>
    /// Показывает актуальную информацию о предметах в иневентаре
    /// </summary>
    private void GetSlotsInfo()
    {
        slotItems.ForEach(s => s.RemoveData());

        int count = Mathf.Min(currentInventory.items.Count, slotItems.Count);

        for (int i = 0; i < count; i++)
        {
            slotItems[i].UpdateImageDate(currentInventory.items[i], statsController);
        }
    }


    /// <summary>
    /// Показывает актуальную информацию об оружие или щите
    /// </summary>
    private void GetWeaponsInfo()
    {
        weaponItems[0].UpdateImageDate(weaponInventory.GetCurrentWeaponData(), statsController);
        weaponItems[1].UpdateImageDate(weaponInventory.GetCurrentShieldData(), statsController);
    }

    /// <summary>
    /// Очищает неактульную информацию о предметах в инвентаре и быстром доступе
    /// </summary>
    private void ClearCellsData()
    {
        slotItems.ForEach((s) => s.RemoveData());
        quickSlotItems.ForEach((s) => s.RemoveData());
        weaponItems.ForEach((s) => s.RemoveData());
    }
    #endregion

    #region Events Listeners

    private void OnItemEquipped() => GetWeaponsInfo();

    /// <summary>
    /// Открывает панель описания предмета
    /// </summary>
    /// <param name="item"></param>
    private void OnItemOutlined(ItemSO item)
    {

        if (item == null)
        {
            descriptionPanel.HidePanel();
            return;
        }

        contextMenu.HideContextMenu(false);
        descriptionPanel.ShowPanel(item);
    }

    /// <summary>
    /// Обновляет информацию о быстрых слотах, слотах и ​​оружии при уничтожении предмета.
    /// </summary>
    private void OnItemDestroyed()
    {
        if (currentInventory.items.Count == 0)
        {
            descriptionPanel.HidePanel();
        }

        GetQuickSlotsInfo();
        GetSlotsInfo();
        GetWeaponsInfo();
    }

    /// <summary>
    /// Удаляет указанный предмет из инвентаря оружия и обновляет информацию об оружии.
    /// </summary>
    /// <param name="data">Элемент для снятия</param>
    /// <param name="pos">Позиция элемента сетки</param>
    private void HandleUnequipItem(ItemData data, Vector2 pos)
    {
        
        weaponInventory.UnequipItem(data);
        GetWeaponsInfo();
    }

    /// <summary>
    /// Фокусируется на первом элементе сетки при закрытии контекстного меню
    /// </summary>
    /// <param name="data">.</param>
    private void OnContextMenuClosed(ItemData data)=>FocusFirstGridItem(data);

    private void OnQuickSlotsInfoUpdate() => GetQuickSlotsInfo();

    #endregion

    #region UI Visual Helpers

    /// <summary>
    /// Подсвечивает актуальную кнопку секции инвентаря
    /// </summary>
    /// <param name="button"></param>
    /// <param name="active"></param>
    private void SetButtonState(Button button, bool active)
    {
        var colors = button.colors;

        if (active)
        {
            button.image.color = colors.selectedColor; // или свой цвет
        }
        else
        {
            button.image.color = colors.normalColor;
        }
    }


    /// <summary>
    /// Выбирает первый предмет в основной сетке предметов
    /// </summary>
    /// <param name="data"></param>
    private void FocusFirstGridItem(ItemData data)
    {
        if (slotItems.Count == 0) return;

        GameObject toSelect = slotItems[0].gameObject;

        if (data != null)
        {
            foreach (var item in slotItems)
            {
                if (item.GetItem().itemSO.id == data.itemSO.id)
                {
                    toSelect = item.gameObject;
                    break;
                }
            }
        }


        StartCoroutine(UINavigationUtils.SelectWithDelay(toSelect));

    }
    #endregion

    #region Gamepad Input Handlers

    /// <summary>
    /// Меняет секции инвентаря по нажатию кнопок (L1,R1)
    /// </summary>
    /// <param name="direction"></param>
    public void SwitchSectionOnInputChange(int direction)
    {
        int currentIndex = sectionOrder.IndexOf(currentSection);
        if (currentIndex == -1) return;

        int newIndex = currentIndex + direction;

        // clamp
        newIndex = (newIndex + sectionOrder.Count) % sectionOrder.Count;

        if (newIndex == currentIndex) return;

        GetSection(sectionOrder[newIndex]);
    }


    /// <summary>
    /// Листает сетку инвентаря (L2, R2)
    /// </summary>
    /// <param name="val"></param>
    internal void RedSliderValue(float val)
    {

        float result = val >= 0 ? -0.25f : 0.25f;
        scrollSlider.value += result;

        //scrollSlider.value += c.ReadValue<Vector2>().;
    }

    #endregion
}
