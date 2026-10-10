using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPointsControllerUI : MonoBehaviour
{
    CharacterLevelController levelController;
    private Image pointsFill;
    private Color pointsFillColor;
    private Coroutine highlightCoroutine;

    [SerializeField] Slider pointsSlider;
    [SerializeField] TextMeshProUGUI currentXPText;
    [SerializeField] TextMeshProUGUI xpToNextLevelText;
    
    public void Init(CharacterLevelController levelController)
    {
        this.levelController = levelController;
        pointsFill = pointsSlider.fillRect.GetComponent<Image>();
        pointsFillColor = pointsFill.color;

        SetSliderValues();
        GetCurrentPointsInfo();

        CharacterLevelController.XpGained += OnPointsDropped;
        CharacterLevelController.NewLevelReachedWithMessage += OnNewLevelReachedWithMessage;
        CharacterLevelController.NewLevelReached += OnNewLevelReached;
      
    }

    private void OnDisable()
    {

        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            highlightCoroutine = null;
            pointsFill.color = pointsFillColor;
        }

        CharacterLevelController.XpGained -= OnPointsDropped;
        CharacterLevelController.NewLevelReached -= OnNewLevelReached;   
        CharacterLevelController.NewLevelReachedWithMessage -= OnNewLevelReachedWithMessage;
    }


    private void SetSliderValues()
    {
        pointsSlider.maxValue = levelController.levelData.xpToNextLevel;
        pointsSlider.value = levelController.levelData.currentXP;
    }

    private void GetCurrentPointsInfo()
    {
        currentXPText.text = levelController.levelData.currentXP.ToString();
        xpToNextLevelText.text = levelController.levelData.xpToNextLevel.ToString();
    }

    private void OnPointsDropped()
    {
        currentXPText.text = levelController.levelData.currentXP.ToString();
        pointsSlider.value = levelController.levelData.currentXP;
    }

    private void OnNewLevelReached()
    {
        SetSliderValues();
        GetCurrentPointsInfo();

        if (!isActiveAndEnabled) return;
        if (highlightCoroutine != null) StopCoroutine(highlightCoroutine);
        highlightCoroutine = StartCoroutine(HighlightPoints());
    }

    private IEnumerator HighlightPoints()
    {
        const float duration = 0.35f;
        float elapsed = 0f;
        Color highlightColor = Color.Lerp(pointsFillColor, Color.white, 0.8f);

        while (elapsed < duration)
        {
            pointsFill.color = Color.Lerp(highlightColor, pointsFillColor,
                Mathf.SmoothStep(0f, 1f, elapsed / duration));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        pointsFill.color = pointsFillColor;
        highlightCoroutine = null;
    }

    private void OnNewLevelReachedWithMessage()
    {
        OnNewLevelReached();
      
    }

  
}
