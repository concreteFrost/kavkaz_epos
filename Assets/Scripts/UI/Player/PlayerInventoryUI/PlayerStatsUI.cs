using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider staminaSlider;

    IEnumerator healthCoroutine;
    IEnumerator staminaCoroutine;

    CharacterStatsController stats;

    [SerializeField] private float sliderUpdateSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(CharacterStatsController stats)
    {
        this.stats = stats;

        healthSlider.maxValue = this.stats.Health.CurrentMax;
        healthSlider.value = this.stats.Health.Current;

        staminaSlider.maxValue = this.stats.Stamina.CurrentMax;
        staminaSlider.value = this.stats.Stamina.Current;

        this.stats.Health.Changed += UpdateHealthSlider;
        this.stats.Stamina.Changed += UpdateStaminaSlider;
    }


    private void OnDisable()
    {
        stats.Health.Changed -= UpdateHealthSlider;
        stats.Stamina.Changed -= UpdateStaminaSlider;
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

    IEnumerator UpdateMaxSlider(Slider slider, float newMax)
    {
        float initialMax = slider.maxValue;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * sliderUpdateSpeed;
            slider.maxValue = Mathf.Lerp(initialMax, newMax, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        slider.maxValue = newMax;
    }
}
