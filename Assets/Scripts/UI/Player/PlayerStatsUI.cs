using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider staminaSlider;

    IEnumerator healthCoroutine;
    IEnumerator staminaCoroutine;

    HumanoidStats stats;

    [SerializeField] private float sliderUpdateSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(HumanoidStats playerStats)
    {
        stats = playerStats;

        healthSlider.maxValue = stats.maxHealth;
        healthSlider.value = stats.Health.Current;

        staminaSlider.maxValue = stats.maxStamina;  
        staminaSlider.value = stats.Stamina.Current;

        stats.Health.Changed += UpdateHealthSlider;
        stats.Stamina.Changed += UpdateStaminaSlider;
    }

    private void OnDisable()
    {
        stats.Health.Changed -= UpdateHealthSlider;
        stats.Stamina.Changed -= UpdateStaminaSlider;
    }


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
}
