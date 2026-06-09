using System;
using UnityEngine;
using UnityEngine.UI;

public class AiHealthUI : MonoBehaviour , IUiProvider
{
    [SerializeField] private Slider healthSlider;

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

        stats.Health.CurrentChanged += UpdateHealth;

        DisableUI();    
        cam = Camera.main;
    }

    private void OnDisable()
    {
        stats.Health.CurrentChanged -= UpdateHealth;

    }

    public void DisableUI()=>healthSlider.gameObject.SetActive(false);
    public void EnableUI()=> healthSlider.gameObject.SetActive(true);

   
    private void LateUpdate()
    {
        if (cam == null)
        {
            cam = Camera.main;  
            Debug.Log("cam is null");
            return;
        }
           

        transform.forward = cam.transform.forward;
    }

    private void UpdateHealth(float health)
    {
        healthSlider.value = health;
    }
}