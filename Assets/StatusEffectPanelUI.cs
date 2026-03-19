using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectPanelUI : MonoBehaviour
{
    [SerializeField] private Image effectImage;
    [SerializeField] private Image affectionImage;

    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI durationText;


    public void SetupEffectInfo(StatusEffectEntry data)
    {
        if (data.effect == null)
        {
            ClearEffectInfo();
            return;
        }

        effectImage.sprite = data.effect.effectImage;
        affectionImage.sprite = data.effect.affectionTypeImage;

        amountText.text = $"{data.amount:F0}";
        durationText.text = $"{data.duration:F1}s";
    }

    public void ClearEffectInfo()
    {
        effectImage.sprite = null;
        affectionImage.sprite = null;
        amountText.text = string.Empty;
        durationText.text = string.Empty;   
    }
}
