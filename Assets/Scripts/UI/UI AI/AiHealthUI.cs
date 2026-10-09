using UnityEngine;
using UnityEngine.UI;

public class AiHealthUI : MonoBehaviour, IUiProvider
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider balanceSlider;
    [SerializeField, Min(0.01f), Tooltip("Время плавного изменения полос в секундах.")]
    private float transitionTime = 0.15f;

    private Image balanceFill;
    private CharacterStatsController stats;
    private Camera cam;
    private float targetHealth;
    private float targetBalance;

    public string HealthProviderId() => null;

    public void Init(CharacterStatsController stats)
    {
        Unsubscribe();
        this.stats = stats;
        healthSlider.minValue = 0;
        balanceSlider.minValue = 0;
        balanceFill = balanceSlider.fillRect.GetComponent<Image>();
        SynchronizeValues();
        if (isActiveAndEnabled)
            Subscribe();
        DisableUI();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        if (stats == null) return;
        SynchronizeValues();
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        stats.Health.CurrentChanged += UpdateHealth;
        stats.Balance.CurrentChanged += UpdateBalance;
    }

    private void Unsubscribe()
    {
        if (stats == null) return;
        stats.Health.CurrentChanged -= UpdateHealth;
        stats.Balance.CurrentChanged -= UpdateBalance;
    }

    private void SynchronizeValues()
    {
        healthSlider.maxValue = stats.Health.CurrentMax;
        balanceSlider.maxValue = stats.Balance.CurrentMax;
        targetHealth = stats.Health.Current;
        targetBalance = stats.Balance.Current;
        healthSlider.value = targetHealth;
        balanceSlider.value = targetBalance;
        UpdateBalanceColor();
    }

    public void DisableUI()
    {
        healthSlider.gameObject.SetActive(false);
        balanceSlider.gameObject.SetActive(false);
    }

    public void EnableUI()
    {
        if (stats != null)
            SynchronizeValues();
        healthSlider.gameObject.SetActive(true);
        balanceSlider.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main;
        if (cam != null)
            transform.forward = cam.transform.forward;

        if (stats == null || !healthSlider.gameObject.activeInHierarchy) return;

        healthSlider.maxValue = stats.Health.CurrentMax;
        balanceSlider.maxValue = stats.Balance.CurrentMax;
        float step = Time.deltaTime / Mathf.Max(0.01f, transitionTime);
        healthSlider.value = Mathf.MoveTowards(healthSlider.value, targetHealth, healthSlider.maxValue * step);
        balanceSlider.value = Mathf.MoveTowards(balanceSlider.value, targetBalance, balanceSlider.maxValue * step);
        UpdateBalanceColor();
    }

    private void UpdateHealth(float health)
    {
        targetHealth = health;
    }

    private void UpdateBalance(float balance)
    {
        targetBalance = balance;
    }

    private void UpdateBalanceColor()
    {
        balanceFill.color = Color.Lerp(
            new Color(1f, 0.35f, 0.12f),
            new Color(0.85f, 0.69f, 0.38f),
            balanceSlider.normalizedValue);
    }
}
