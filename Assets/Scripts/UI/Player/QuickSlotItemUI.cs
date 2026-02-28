using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlotItemUI : MonoBehaviour , IPointerClickHandler , ISubmitHandler ,ISelectHandler, IDeselectHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected TextMeshProUGUI quantityText;
    [SerializeField] protected Image outlineImage;

    ItemData currentItem;

    private Action<ItemData, Vector2> clickHandler;

    public ItemData GetItem() => currentItem;

    private void Awake()
    {
        ToggleOutlineImage(false);
    }

    private void ToggleOutlineImage(bool isVisible)=> outlineImage.gameObject.SetActive(isVisible);  

    public void InitInInventory(Action<ItemData, Vector2> onClick)
    {
        clickHandler = onClick;
    }

    public void UpdateImageDate(ItemData data)
    {

        currentItem = data;
        var itemSo = currentItem.itemSO;

        itemImage.enabled = true;
        backgroundImage.enabled = true;
    
        itemImage.sprite = currentItem.itemSO.itemImage;
        backgroundImage.enabled = true;

        quantityText.enabled = true;
        quantityText.text = currentItem.quantity.ToString();
    }


    public virtual void RemoveData()
    {
        currentItem = null;
        itemImage.enabled = false;
        backgroundImage.enabled = false;
        quantityText.enabled = false;

    }

    public void ScaleImages(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if(currentItem == null) return;

        clickHandler?.Invoke(currentItem, GetAnchoredPosition());
        
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (currentItem == null) return;

        clickHandler?.Invoke(currentItem, GetAnchoredPosition());
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




    public void OnSelect(BaseEventData eventData)
    {
        ToggleOutlineImage(true);

    }
    public void OnDeselect(BaseEventData eventData)
    {
        ToggleOutlineImage(false);

        
    }
}
