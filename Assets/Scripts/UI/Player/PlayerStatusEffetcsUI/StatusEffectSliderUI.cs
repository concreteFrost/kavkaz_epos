using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSliderUI : MonoBehaviour
{

    public Slider slider;
    public Image effectImage;
    public Image fillImage;
    public string id;

    public void SetEffect(StatusEffectSO data)
    {
        this.id = data.id;
        effectImage.sprite =data.effectImage;
        fillImage.color =data.effectColor;
      
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
