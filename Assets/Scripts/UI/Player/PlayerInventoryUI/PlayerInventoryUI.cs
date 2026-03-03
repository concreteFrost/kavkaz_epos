using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum InventorySection
{
    Magic = 0,
    Resources = 1,
    Consumables = 2,
}

public class PlayerInventoryUI : MonoBehaviour
{
    private CharacterStatsController statsController;  
    private PlayerInventoryContextMenuUI contextMenu;

    [SerializeField] GameObject mainWrapper;
    [SerializeField] GameObject itemCellPrefab;
    [SerializeField] Transform cellsContainer;
    [SerializeField] Scrollbar scrollSlider;
    [SerializeField] ScrollRect scrollRect;

    private List<InventoryItemUI> slotItems = new List<InventoryItemUI>();
    private int totalCellsToInit = 30;

    [SerializeField] Transform quickSlotsContainer;
    private List<InventoryItemUI> quickSlotItems = new List<InventoryItemUI>();

    private QuickAccessInventory currentInventory;
    Dictionary<InventorySection, QuickAccessInventory> inventories;

    GridLayoutGroup grid;

    public InventorySection currentSection { get; private set; }

    public void Init(QuickAccessInventory spellInventory, PlayerInventoryContextMenuUI contextMenu, CharacterStatsController statsController)
    {
        this.contextMenu = contextMenu;
        this.statsController = statsController; 
        contextMenu.OnContextMenuClosed += FocusFirstGridItem;
        contextMenu.UpdateQuickSlotsInfo += GetQuickSlotsInfo;

        grid = cellsContainer.GetComponent<GridLayoutGroup>();

        //динамическое назначение инвентарей
        inventories = new Dictionary<InventorySection, QuickAccessInventory>
    {
        { InventorySection.Magic,spellInventory },

    };


        InitInventoryCells();
        InitQuickAccessCells();
    }

    private void OnDisable()
    {
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
            slotItems.Add(slotItem);
   
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

        List<ItemData> itemsToFill = new List<ItemData>();

        switch (currentSection)
        {
            case InventorySection.Magic:
                itemsToFill =currentInventory.GetQuickAccessData();
                break;
        }

        for (int i = 0; i < itemsToFill.Count; i++)
        {
            quickSlotItems[i].UpdateImageDate(itemsToFill[i],statsController);
        }

    }

    /// <summary>
    /// Показывает актуальную информацию о предметах в иневентаре
    /// </summary>
    private void GetSlotsInfo()
    {
        List<ItemData> itemsToFill = new List<ItemData>();

        switch (currentSection)
        {
            case InventorySection.Magic:
                itemsToFill.AddRange(currentInventory.items);
                break;
        }

        if (itemsToFill.Count == 0) return;

        for (int i = 0; i < itemsToFill.Count; i++)
        {
            slotItems[i].UpdateImageDate(itemsToFill[i],statsController );

          
        }
    }

    /// <summary>
    /// Очищает неактульную информацию о предметах в инвентаре и быстром доступе
    /// </summary>
    private void ClearCellsData()
    {
        slotItems.ForEach((s) => s.RemoveData());
        quickSlotItems.ForEach((s) => s.RemoveData());
    }

    /// <summary>
    /// Отображает предметы в инвентаре и быстром доступе, которые актуальны для данной секции
    /// </summary>
    /// <param name="section"></param>
    public void GetSection(InventorySection section)
    {

        currentSection = section;
        currentInventory = inventories[section];

        ClearCellsData();

        GetSlotsInfo();
        GetQuickSlotsInfo();

        Canvas.ForceUpdateCanvases();          // важно
        scrollRect.verticalNormalizedPosition = 1f;

        // Преобразуем QuickSlotItemUI в Button
        var buttons = slotItems.Select(s => s.GetComponent<Button>()).ToList();
        // Настраиваем сеточную навигацию
        UINavigationUtils.SetupGridNavigation(buttons, 5);
        FocusFirstGridItem(null);

      
    }


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

    internal void RedSliderValue(float val)
    {
       
        float result = val >= 0 ? -0.25f : 0.25f;
        scrollSlider.value += result;
        Debug.Log(val);
        //scrollSlider.value += c.ReadValue<Vector2>().;
    }
}
