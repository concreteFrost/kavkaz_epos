using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;
  
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider staminaSlider;

    RectTransform healthSliderRect;
    RectTransform staminaSliderRect;

    private float sliderScalingFactor = 2f;

    IEnumerator healthCoroutine;
    IEnumerator staminaCoroutine;

    CharacterStatsController stats;

    [SerializeField] private float sliderUpdateSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(CharacterStatsController stats)
    {
        this.stats = stats;

        healthSliderRect = healthSlider.GetComponent<RectTransform>();
        staminaSliderRect = staminaSlider.GetComponent<RectTransform>();
       
        healthSlider.maxValue = this.stats.Health.CurrentMax;
        healthSlider.value = this.stats.Health.Current;
        SetSliderSize(healthSliderRect, stats.Health.CurrentMax);

        staminaSlider.maxValue = this.stats.Stamina.CurrentMax;
        staminaSlider.value = this.stats.Stamina.Current;
        SetSliderSize(staminaSliderRect, stats.Stamina.CurrentMax);

        this.stats.Health.CurrentChanged += UpdateHealthSlider;
        this.stats.Stamina.CurrentChanged += UpdateStaminaSlider;
        this.stats.Health.MaxChanged += UpdateHealthSliderSize;
        this.stats.Stamina.MaxChanged += UpdateStaminaSliderSize;
    }


    private void OnDisable()
    {
        stats.Health.CurrentChanged -= UpdateHealthSlider;
        stats.Stamina.CurrentChanged -= UpdateStaminaSlider;
        stats.Health.MaxChanged -= UpdateHealthSliderSize;
        stats.Stamina.MaxChanged -= UpdateStaminaSliderSize;
    }

    private void SetSliderSize(RectTransform sliderRect, float value)
    {
        sliderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * sliderScalingFactor);   
    }

    public void SetStatsVisible(bool isVisible) => wrapper.SetActive(isVisible);    


    public void UpdateHealthSlider(float value)
    {
        HandleStartCoroutine(ref healthCoroutine, healthSlider, value);
    }

    public void UpdateStaminaSlider(float value)
    {
        HandleStartCoroutine(ref staminaCoroutine,staminaSlider,value);
    }

    private void UpdateHealthSliderSize(int level, float value)
    {
        healthSlider.maxValue = value;  
        SetSliderSize(healthSliderRect, value);
    }

    private void UpdateStaminaSliderSize(int level, float value)
    {
        staminaSlider.maxValue = value; 
        SetSliderSize(staminaSliderRect, value);        
    }

    private void HandleStartCoroutine(ref IEnumerator coroutine, Slider slider, float val)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = UpdateSliderValue(slider,val);
        StartCoroutine(coroutine);  
    }

    IEnumerator UpdateSliderValue(Slider slider, float targetValue)
    {
        float currentValue = slider.value;
        float initialValue = currentValue;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * sliderUpdateSpeed;
            currentValue = Mathf.Lerp(initialValue, targetValue, t);
            slider.value = currentValue;
            yield return null;
        }

        slider.value = targetValue;
    }


}
