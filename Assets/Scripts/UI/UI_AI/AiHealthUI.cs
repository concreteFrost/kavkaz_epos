using System;
using UnityEngine;
using UnityEngine.UI;

public class AiHealthUI : MonoBehaviour, IUiProvider
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider balanceSlider;
    Image balanceFill;

    private CharacterStatsController stats;
    private Camera cam;

    #region IUIProvider
    public string HealthProviderId() => null;
    #endregion

    public void Init(CharacterStatsController stats)
    {
        this.stats = stats;
        healthSlider.minValue = 0;
        healthSlider.maxValue = stats.Health.CurrentMax;
        healthSlider.value = stats.Health.Current;

        balanceSlider.minValue = 0;
        balanceSlider.maxValue = stats.Balance.CurrentMax;
        balanceSlider.value = stats.Balance.Current;

        balanceFill = balanceSlider.fillRect.GetComponent<Image>();

        stats.Health.CurrentChanged += UpdateHealth;
        stats.Balance.CurrentChanged += UpdateBalance;
      

        DisableUI();
        cam = Camera.main;
    }



    private void OnDisable()
    {
        stats.Health.CurrentChanged -= UpdateHealth;
        stats.Balance.CurrentChanged -= UpdateBalance;

    }

    public void DisableUI()
    {
        healthSlider.gameObject.SetActive(false);
        balanceSlider.gameObject.SetActive(false);
    }
    public void EnableUI()
    {
        healthSlider.gameObject.SetActive(true);
        balanceSlider.gameObject.SetActive(true);
    }


    private void LateUpdate()
    {
        if (cam == null)
        {
            cam = Camera.main;

            return;
        }


        transform.forward = cam.transform.forward;
    }

    private void UpdateHealth(float health)
    {
        healthSlider.value = health;
    }

    private void UpdateBalance(float balance)
    {
        balanceSlider.value = balance;

        float t = balance / stats.Balance.CurrentMax;

        balanceFill.color = Color.Lerp(
            new Color(1f, 0.5f, 0f), // оранжевый
            Color.white,
            t);
    }
}