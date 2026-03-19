using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSliderUI : MonoBehaviour
{

    public Slider slider;
    public Image effectImage;
    public Image fillImage;
    public Image affectionImage;
    public string id;

    public void SetEffect(StatusEffectSO data)
    {
        this.id = data.id;

        effectImage.sprite = data.effectImage;
        fillImage.color = data.effectColor;

        effectImage.enabled = (data.effectImage != null); // включаем только если есть спрайт

        if (data.affectionTypeImage != null)
            affectionImage.sprite = data.affectionTypeImage;
        affectionImage.enabled = (data.affectionTypeImage != null);
    }

    public void Show()
    {
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        slider.maxValue = 1f;

        effectImage.enabled = (effectImage.sprite != null); // показываем только если есть спрайт
        affectionImage.enabled = (affectionImage.sprite != null);
    }

    public void Hide()
    {
        slider.gameObject.SetActive(false); 
        effectImage.enabled = false;

        affectionImage.sprite = null;
        affectionImage.enabled = false;
    }



    public void Tick(float amount)
    {
        slider.value = amount;  
    }
}
