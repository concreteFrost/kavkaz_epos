using UnityEngine;
using UnityEngine.UI;

public class QuickSlotBreakableUI : SlotItemUI
{
    [SerializeField] Slider breakdownSlider;
    IBreakable currentWeapon;

    public void UpdateWeaponData(ItemSO data, IBreakable weapon)
    {
        currentWeapon = weapon;

        itemImage.enabled = true;
        backgroundImage.enabled = true;

        itemImage.sprite = data.itemImage;
        
        backgroundImage.enabled = true;

        if (!breakdownSlider.gameObject.activeInHierarchy)
        {
            breakdownSlider.gameObject.SetActive(true);  
        }

        quantityText.enabled = false;


    }

    private void Update()
    {
        if (currentWeapon != null)
        {
            
            breakdownSlider.value = currentWeapon.GetDurability();
        }
    }

    public override void RemoveData()
    {
        base.RemoveData();

        currentWeapon = null;
        breakdownSlider.gameObject.SetActive(false);
    }

    public void UpdateBreakdownSlider(IBreakable breakdown)
    {
        breakdownSlider.value = breakdown.GetDurability();
    }
}
