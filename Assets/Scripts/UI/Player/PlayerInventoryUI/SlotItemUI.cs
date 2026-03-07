using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected TextMeshProUGUI quantityText;
    [SerializeField] protected Image cantUseImage;

    protected ItemData currentItem;

    public ItemData GetItem() => currentItem;

    public void UpdateImageDate(ItemData data, CharacterStatsController statsController)
    {

        currentItem = data;
        var itemSo = currentItem.itemSO;

        itemImage.enabled = true;
        backgroundImage.enabled = true;
    
        itemImage.sprite = currentItem.itemSO.itemImage ?? null;
        backgroundImage.enabled = true;

        quantityText.enabled = true;
        quantityText.text = currentItem.quantity.ToString();

        cantUseImage.gameObject.SetActive(false);
       

        if (currentItem.itemSO is SpellProjectileSO spell)
        {
            var requiredModel = statsController.GetCurrentStatLevel(spell.Requirements.statType);
            bool canUse = spell.CanEmit(requiredModel);
            ToggleCantUseImage(!canUse);
           
        }
        
    }


    public virtual void RemoveData()
    {
        currentItem = null;
        itemImage.enabled = false;
        backgroundImage.enabled = false;
        quantityText.enabled = false;

    }

    public void ToggleCantUseImage(bool value)
    {
        itemImage.color = value ? Color.red : Color.white;
        //cantUseImage.gameObject.SetActive(value);
    }


}
