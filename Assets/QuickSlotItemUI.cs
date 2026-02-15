using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotItemUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI quantityText;
    
    public void UpdateImageDate(ItemData data)
    {
        itemImage.enabled = true;
        backgroundImage.enabled = true;
        quantityText.enabled = true;    

        itemImage.sprite = data.itemSO.itemImage;
        backgroundImage.enabled = true; 
        quantityText.text = data.quantity.ToString();
    }

    public void RemoveData()
    {
        itemImage.enabled = false;
        backgroundImage.enabled = false;
        quantityText.enabled = false;

    }
}
