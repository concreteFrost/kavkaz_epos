using System.Collections;
using System.Collections.Generic;
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
    private CharacterSpellInventory spellInventory;
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

    public InventorySection currentSection { get; private set; }

    public void Init(CharacterSpellInventory spellInventory, PlayerInventoryContextMenuUI contextMenu)
    {
        this.spellInventory = spellInventory;
        this.contextMenu = contextMenu;
       

        InitCells();
    }

    public void ToggleInventory(bool isVisible)
    {
        mainWrapper.SetActive(isVisible);
    }

    private void InitCells()
    {
        for (int i = 0; i < totalCellsToInit; i++)
        {
            GameObject go = Instantiate(itemCellPrefab, cellsContainer);

            QuickSlotItemUI slotItem = go.GetComponent<QuickSlotItemUI>();
            slotItem.ScaleImages(scaleImageSize);

            slotItem.RemoveData();
            slotItems.Add(slotItem);

            slotItem.ItemClicked += ShowItemInfo;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(itemCellPrefab, quickSlotsContainer);
            var data = go.GetComponent<QuickSlotItemUI>();
            quickSlotItems.Add(data);
        }
    }

    private void GetQuickSlotsInfo(InventorySection section)
    {

        List<ItemData> itemsToFill = new List<ItemData>();

        switch (section)
        {
            case InventorySection.Magic:
                itemsToFill.AddRange(spellInventory.GetQuickAccessData());
                break;
        }

        for (int i = 0; i < itemsToFill.Count; i++)
        {
            quickSlotItems[i].UpdateImageDate(itemsToFill[i]);
        }

       
    }

    private void GetSlotsInfo(InventorySection section)
    {
        List<ItemData> itemsToFill = new List<ItemData>();

        switch (section)
        {
            case InventorySection.Magic:
                itemsToFill.AddRange(spellInventory.items);
                break;
        }

        if (itemsToFill.Count == 0) return;

        for (int i = 0; i < itemsToFill.Count; i++)
        {
            slotItems[i].UpdateImageDate(itemsToFill[i]);
        }
    }


    private void ClearCellsData() { 
        slotItems.ForEach((s) => s.RemoveData());
        quickSlotItems.ForEach((s) => s.RemoveData());  
    }

    public void GetSection(InventorySection section)
    {
        ClearCellsData();

        GetSlotsInfo(section);
        GetQuickSlotsInfo(section);

        scrollSlider.value = 1.1f;
        EventSystem.current.SetSelectedGameObject(slotItems[0].gameObject);
    }

    private void ShowItemInfo(ItemData data, Vector2 pos)
    {
        contextMenu.ShowContextMenu(data, pos);
    }

}
