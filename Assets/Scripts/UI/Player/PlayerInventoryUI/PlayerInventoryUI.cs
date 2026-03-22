using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum InventorySection
{
    Magic = 0,
    Consumables = 1,
}

public class PlayerInventoryUI : MonoBehaviour
{
    private CharacterStatsController statsController;  
    private PlayerInventoryContextMenuUI contextMenu;
    private ItemDescriptionPanelUI descriptionPanel;
    private ICombatInventory combatInventory;
   
    [SerializeField] GameObject mainWrapper;
    [SerializeField] GameObject itemCellPrefab;
    [SerializeField] Transform weaponCellsContainer;
    [SerializeField] Transform cellsContainer;
    [SerializeField] Scrollbar scrollSlider;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Button magicSectionBtn;
    [SerializeField] Button consumableSectionBtn;
    //[SerializeField] Button resourcesSectionBtn;


    private List<InventoryItemUI> weaponItems = new List<InventoryItemUI>();

    private List<InventoryItemUI> slotItems = new List<InventoryItemUI>();
    private int totalCellsToInit = 50;

    [SerializeField] Transform quickSlotsContainer;
    private List<InventoryItemUI> quickSlotItems = new List<InventoryItemUI>();

    private QuickAccessInventory currentInventory;
    Dictionary<InventorySection, QuickAccessInventory> inventories;

    GridLayoutGroup grid;

    public bool IsOpened()=>mainWrapper.activeInHierarchy;

    public InventorySection currentSection { get; private set; }


    // для переключения контроллером
    private List<InventorySection> sectionOrder = new List<InventorySection>
{
    InventorySection.Magic,
    InventorySection.Consumables
};

    public void Init(ItemDescriptionPanelUI descriptionPanel, QuickAccessInventory spellInventory,QuickAccessInventory consumableInventory, PlayerInventoryContextMenuUI contextMenu, CharacterStatsController statsController, ICombatInventory combatInventory)
    {
        this.descriptionPanel = descriptionPanel;   
        this.contextMenu = contextMenu;
        this.statsController = statsController; 
        this.combatInventory = combatInventory; 
        contextMenu.OnContextMenuClosed += FocusFirstGridItem;
        contextMenu.UpdateQuickSlotsInfo += GetQuickSlotsInfo;

        grid = cellsContainer.GetComponent<GridLayoutGroup>();

        //динамическое назначение инвентарей
        inventories = new Dictionary<InventorySection, QuickAccessInventory>
    {
        {InventorySection.Magic,spellInventory },
        {InventorySection.Consumables, consumableInventory },

    };

       
        InitInventoryCells();
        InitQuickAccessCells();
        InitWeaponCells();

        BindSectionButtons();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
        contextMenu.OnContextMenuClosed -= FocusFirstGridItem;  
        contextMenu.UpdateQuickSlotsInfo -= GetQuickSlotsInfo; 
    }

    /// <summary>
    /// Управление состоянием видимости инвентаря
    /// </summary>
    /// <param name="isVisible"></param>
    public void ToggleInventory(bool isVisible)
    {
        mainWrapper.SetActive(isVisible);

        if (!isVisible) descriptionPanel.ClearCommonItemInfo();
    }

    private void BindSectionButtons()
    {
        RemoveButtonListeners();    

        magicSectionBtn.onClick.AddListener(()=>GetSection(InventorySection.Magic));
        consumableSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Consumables));
        //resourcesSectionBtn.onClick.AddListener(() => GetSection(InventorySection.Resources));
    }

    private void RemoveButtonListeners()
    {
        magicSectionBtn.onClick.RemoveAllListeners();
        consumableSectionBtn.onClick.RemoveAllListeners();
        //resourcesSectionBtn.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Открывает панель описания предмета
    /// </summary>
    /// <param name="item"></param>
    private void OnItemOutlined(ItemSO item)
    {

        descriptionPanel.ShowPanel(item);
    }

    /// <summary>
    /// Динамически создает QuickSlotItemUI в контейнере
    /// </summary>
    private void InitInventoryCells()
    {
        //создание основной сетки
        for (int i = 0; i < totalCellsToInit; i++)
        {
            GameObject go = Instantiate(itemCellPrefab, cellsContainer);
            InventoryItemUI slotItem = go.GetComponent<InventoryItemUI>();

            slotItem.InitInInventory((item,pos)=>contextMenu.ShowContextMenu(item,pos));
            slotItem.RemoveData();
            slotItem.FitToCell(grid.cellSize);
            slotItem.ItemOutlined += OnItemOutlined;
            slotItems.Add(slotItem);
   
        }

    }

    private void InitWeaponCells()
    {
        for(int i = 0; i < 2; i++)
        {
            GameObject go = Instantiate(itemCellPrefab, weaponCellsContainer);
            InventoryItemUI slotItem = go.GetComponent<InventoryItemUI>();

            slotItem.InitInInventory((item, pos) => contextMenu.ShowContextMenu(item, pos));
            slotItem.RemoveData();
            slotItem.FitToCell(grid.cellSize);
            slotItem.ItemOutlined += OnItemOutlined;
            weaponItems.Add(slotItem);
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

    /// <summary>
    /// Показывает актуально информацию о предметах в быстром доступе
    /// </summary>
    private void GetQuickSlotsInfo()
    {
        //if (!mainWrapper.activeInHierarchy) return;   i

        quickSlotItems.ForEach((s) => s.RemoveData());

        for (int i = 0; i < currentInventory.GetQuickAccessData().Count; i++)
        {
            quickSlotItems[i].UpdateImageDate(currentInventory.GetQuickAccessData()[i],statsController);
        }

    }

    /// <summary>
    /// Показывает актуальную информацию о предметах в иневентаре
    /// </summary>
    private void GetSlotsInfo()
    {
        
        for (int i = 0; i < currentInventory.items.Count; i++)
        {
            slotItems[i].UpdateImageDate(currentInventory.items[i], statsController);
          
        }
    }

    /// <summary>
    /// Показывает актуальную информацию об оружие или щите
    /// </summary>
    private void GetWeaponsInfo()
    {
        if (combatInventory.CurrentWeapon != null)
        {
            var tempWeaponData = new ItemData();
            tempWeaponData.itemSO = combatInventory.CurrentWeapon.WeaponData();
            tempWeaponData.quantity = 1;
            weaponItems[0].UpdateImageDate(tempWeaponData, statsController);
        }
        if(combatInventory.ShieldWeapon != null)
        {
            var tempShieldData = new ItemData();
            tempShieldData.itemSO = combatInventory.ShieldWeapon.ShieldData();
            tempShieldData.quantity = 1;
            weaponItems[1].UpdateImageDate(tempShieldData, statsController);    
        }
            
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

    /// <summary>
    /// Отображает предметы в инвентаре и быстром доступе, которые актуальны для данной секции
    /// </summary>
    /// <param name="section"></param>
    public void GetSection(InventorySection section)
    {

        descriptionPanel.HidePanel();

        currentSection = section;
        currentInventory = inventories[section];

        contextMenu.SetCurrentInventory(currentInventory);  
        contextMenu.HideContextMenu();
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
        UINavigationUtils.SetupGridNavigation(mainGridButtons, 5,weaponButtons);

        // Настраиваем вертикальную панель для оружия с учётом перехода на правую панель
        UINavigationUtils.ClampVerticalNavigation(weaponButtons,mainGridButtons);
        
        FocusFirstGridItem(null);

        SetButtonState(magicSectionBtn, section == InventorySection.Magic);
        SetButtonState(consumableSectionBtn, section == InventorySection.Consumables);

      
    }

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
            foreach(var item in slotItems)
            {
                if(item.GetItem().itemSO.id == data.itemSO.id)
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
