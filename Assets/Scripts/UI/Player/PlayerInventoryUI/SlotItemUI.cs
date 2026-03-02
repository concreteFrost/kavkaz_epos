using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected TextMeshProUGUI quantityText;

    protected ItemData currentItem;

    public ItemData GetItem() => currentItem;


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


}
