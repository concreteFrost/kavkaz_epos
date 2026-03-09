using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSliderUI : MonoBehaviour
{

    public StatusEffectType effectType;
    public Slider slider;
    public Image effectImage;
    public Image fillImage;

    public void SetEffect(StatusEffectType effectType, Sprite img, Color sliderColor)
    {
        this.effectType = effectType;  
        effectImage.sprite = img;
        fillImage.color = sliderColor;
      
    }

    public void Hide()
    {
        slider.gameObject.SetActive(false); 
        effectImage.enabled = false;
    }

    public void Show()
    {
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        slider.maxValue = 1f;
        effectImage.enabled = true;
        
    }

    public void Tick(float amount)
    {
        slider.value = amount;  
    }
}
