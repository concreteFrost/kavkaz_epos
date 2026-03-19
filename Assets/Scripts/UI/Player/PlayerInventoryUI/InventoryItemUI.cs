using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemUI : SlotItemUI, IPointerClickHandler, ISubmitHandler, IDeselectHandler, ISelectHandler
{
    [SerializeField] protected Image outlineImage;
    
    private Action<ItemData, Vector2> clickHandler;
    
    public Action<RectTransform> ItemSelected;
    public Action<ItemSO> ItemOutlined;


    private void Awake()
    {
        
        ToggleOutlineImage(false);
    }

    private void ToggleOutlineImage(bool isVisible) => outlineImage.gameObject.SetActive(isVisible);

    public void InitInInventory(Action<ItemData, Vector2> onClick)
    {
        clickHandler = onClick;
    }

    public void FitToCell(Vector2 cellSize)
    {
        Vector2 baseSize = transform.localScale;
        float scaleX = cellSize.x / baseSize.x;
        float scaleY = cellSize.y / baseSize.y;

        float scale = Mathf.Min(scaleX, scaleY);

        transform.localScale = Vector3.one * (scale * 0.01f);
    }

    public Vector2 GetAnchoredPosition()
    {
        // получаем RectTransform канваса
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // конвертируем позицию слота в локальные координаты канваса
        RectTransform slotRect = GetComponent<RectTransform>();
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            slotRect.position,
            canvasRect.GetComponent<Canvas>().worldCamera,
            out anchoredPos
        );

        return anchoredPos;
    }

    #region Event Handlers
    private void HandleItemEvent()
    {
        if (currentItem == null) return;

        clickHandler?.Invoke(currentItem, GetAnchoredPosition());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
