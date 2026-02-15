using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotItemUI : MonoBehaviour
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected TextMeshProUGUI quantityText;
    
    public void UpdateImageDate(ItemData data)
    {

        var itemSo = data.itemSO;

        itemImage.enabled = true;
        backgroundImage.enabled = true;
    
        itemImage.sprite = data.itemSO.itemImage;
        backgroundImage.enabled = true;

        quantityText.enabled = true;
        quantityText.text = data.quantity.ToString();
    }

    public virtual void RemoveData()
    {
        itemImage.enabled = false;
        backgroundImage.enabled = false;
        quantityText.enabled = false;

    }
}
