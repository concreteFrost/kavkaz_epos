using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemUI : SlotItemUI, IPointerClickHandler, ISubmitHandler, IDeselectHandler, ISelectHandler 
{
    [SerializeField] protected Image outlineImage;
    
    private Action<ItemData, Vector2> ItemClicked;
    
    public Action<RectTransform> ItemSelected;
    public Action<ItemSO> ItemOutlined;

    private void Awake()
    {
        
        ToggleOutlineImage(false);
    }

    private void ToggleOutlineImage(bool isVisible) => outlineImage.gameObject.SetActive(isVisible);

    public void InitInInventory(Action<ItemData, Vector2> onClick)
    {
        ItemClicked = onClick;
    }

 
    #region Event Handlers
    private void HandleItemEvent()
    {
        
        if (currentItem == null)
        {
            ItemClicked?.Invoke(null,GetAnchoredPosition());   
            return;
        }

        ItemClicked?.Invoke(currentItem, GetAnchoredPosition());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var clickCount = eventData.clickCount;

        if (clickCount < 2) return;

        HandleItemEvent();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        HandleItemEvent();
    }

    public void OnSelect(BaseEventData eventData)
    {

        ToggleOutlineImage(true);
        if(currentItem !=null) ItemOutlined?.Invoke(currentItem.itemSO);
   
    }
    public void OnDeselect(BaseEventData eventData)
    {
        ToggleOutlineImage(false);

    }
    #endregion

}
