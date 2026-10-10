using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [Tooltip("Показывать рамку слота, даже если в нём нет предмета.")]
    [SerializeField] private bool showBackgroundWhenEmpty;
    [SerializeField] protected TextMeshProUGUI quantityText;
    [Tooltip("Рамка количества. Отображается только вместе со счётчиком предметов.")]
    [SerializeField] private GameObject quantityFrame;
    [SerializeField] protected Image cantUseImage;

    protected ItemData currentItem;

    public ItemData GetItem() => currentItem;

    public void UpdateImageDate(ItemData data, CharacterStatsController statsController)
    {
        if(data == null || data.itemSO == null)
        {
            RemoveData();
            return;
        }
        cantUseImage.gameObject.SetActive(false);
        ToggleCantUseImage(false);

        currentItem = data;
        var itemSo = currentItem.itemSO;

        itemImage.enabled = true;
        backgroundImage.enabled = true;

        itemImage.sprite = currentItem.itemSO.itemImage ?? null;

        backgroundImage.enabled = true;

        if (data.itemSO.IsStackable())
        {
            SetQuantityVisible(true);
            quantityText.text = currentItem.quantity.ToString();
        }
        else
        {
            SetQuantityVisible(false);
        }

        if (currentItem.itemSO is SpellProjectileSO spell)
        {
            var requiredModel = statsController.GetCurrentStatLevel(spell.requirements.statType);
            bool canUse = spell.CanEmit(requiredModel);
            ToggleCantUseImage(!canUse);
           
        }
        
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


    protected void SetQuantityVisible(bool visible)
    {
        quantityText.enabled = visible;
        if (quantityFrame != null) quantityFrame.SetActive(visible);
    }

    public virtual void RemoveData()
    {
        currentItem = null;
        itemImage.enabled = false;
        backgroundImage.enabled = showBackgroundWhenEmpty;
        SetQuantityVisible(false);

    }

    public void ToggleCantUseImage(bool value)
    {
        itemImage.color = value ? Color.red : Color.white;
        //cantUseImage.gameObject.SetActive(value);
    }


}
