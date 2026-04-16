using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossArenaUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Slider slider_BossHealth;
    [SerializeField] TextMeshProUGUI text_bossName;

    private void Awake()
    {
        HidePanel();    
    }

    public void ShowPanel(string bossName, float bossMaxHealth)
    {
        wrapper.SetActive(true);

        slider_BossHealth.maxValue = bossMaxHealth;
        slider_BossHealth.value = bossMaxHealth;

        text_bossName.text = bossName;
    }

    private void HidePanel()
    {
        wrapper?.SetActive(false);  
    }

    public void HidePanelWithDelay()
    {
        if (!wrapper.activeInHierarchy) return;

        StartCoroutine(HidePanelCoroutine());
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        slider_BossHealth.value = currentHealth;    
    }

    IEnumerator HidePanelCoroutine()
    {
        yield return new WaitForSeconds(3f);
        HidePanel();    
    }
}
