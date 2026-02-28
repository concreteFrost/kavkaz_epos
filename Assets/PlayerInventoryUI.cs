using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum InventorySection
{
    Magic = 0,
    Resources = 1,
    Consumables = 2,
}

public class PlayerInventoryUI : MonoBehaviour
{
   
    private PlayerInventoryContextMenuUI contextMenu;

    [SerializeField] GameObject mainWrapper;
    [SerializeField] GameObject itemCellPrefab;
    [SerializeField] Transform cellsContainer;
    [SerializeField] Scrollbar scrollSlider;

    private List<QuickSlotItemUI> slotItems = new List<QuickSlotItemUI>();
    private int totalCellsToInit = 100;
    private float scaleImageSize = 1.1f;

    [SerializeField] Transform quickSlotsContainer;
    private List<QuickSlotItemUI> quickSlotItems = new List<QuickSlotItemUI>();

    private QuickAccessInventory currentInventory;
    Dictionary<InventorySection, QuickAccessInventory> inventories;

    Selectable lastSelectedObject;

    public InventorySection currentSection { get; private set; }

    public void Init(QuickAccessInventory spellInventory, PlayerInventoryContextMenuUI contextMenu)
    {
        this.contextMenu = contextMenu;
        contextMenu.OnContextMenuClosed += FocusFirstGridItem;

        //динамическое назначение инвентарей
        inventories = new Dictionary<InventorySection, QuickAccessInventory>
    {
        { InventorySection.Magic,spellInventory },

    };

        //подписка на обновление данных о слотах
        foreach(var inv in inventories.Values)
        {
            inv.OnQuickAccessChanged += GetQuickSlotsInfo;
        }


        InitInventoryCells();
        InitQuickAccessCells();
    }

    private void OnDisable()
    {

        contextMenu.OnContextMenuClosed -= FocusFirstGridItem;  

        foreach (var inv in inventories.Values)
        {
            inv.OnQuickAccessChanged -= GetQuickSlotsInfo;
        }

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
            QuickSlotItemUI slotItem = go.GetComponent<QuickSlotItemUI>();
            slotItem.ScaleImages(scaleImageSize);

            slotItem.SetOnClickAction((item,pos)=>contextMenu.ShowContextMenu(item,pos));
            slotItem.RemoveData();
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
            var data = go.GetComponent<QuickSlotItemUI>();
            data.SetOnClickAction((item, pos) => currentInventory.RemoveFromQuickAccess(item));
            quickSlotItems.Add(data);
        }
    }


    /// <summary>
    /// Показывает актуально информацию о предметах в быстром доступе
    /// </summary>
    private void GetQuickSlotsInfo()
    {
        quickSlotItems.ForEach((s) => s.RemoveData());

        List<ItemData> itemsToFill = new List<ItemData>();

        switch (currentSection)
        {
            case InventorySection.Magic:
                itemsToFill.AddRange(currentInventory.GetQuickAccessData());
                break;
        }

        for (int i = 0; i < itemsToFill.Count; i++)
        {
            quickSlotItems[i].UpdateImageDate(itemsToFill[i]);
        }

    }

    /// <summary>
    /// Показывает актуальную информация о предметах в иневентаре
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
            slotItems[i].UpdateImageDate(itemsToFill[i]);
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

        scrollSlider.value = 1.1f;
      

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

}
