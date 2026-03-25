using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPointsControllerUI : MonoBehaviour
{
    CharacterLevelController levelController;

    [SerializeField] Slider pointsSlider;
    [SerializeField] TextMeshProUGUI currentXPText;
    [SerializeField] TextMeshProUGUI xpToNextLevelText;
    [SerializeField] TextMeshProUGUI newLevelText;

    Coroutine levelUpdatedCoroutine = null;

    public void Init(CharacterLevelController levelController)
    {
        this.levelController = levelController;


        SetSliderValues();
        GetCurrentPointsInfo();

        newLevelText.gameObject.SetActive(false);   

        levelController.XpGained += OnPointsDropped;
        levelController.NewLevelReachedWithMessage += OnNewLevelReachedWithMessage;
        levelController.NewLevelReached += OnNewLevelReached;
      
    }

    private void OnDisable()
    {

        levelController.XpGained -= OnPointsDropped;
        levelController.NewLevelReached -= OnNewLevelReached;   
        levelController.NewLevelReachedWithMessage -= OnNewLevelReachedWithMessage;
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
    }

    private void OnNewLevelReachedWithMessage()
    {
        OnNewLevelReached();
        ShowLevelUpdated();
    }

    private void ShowLevelUpdated()
    {
        if(levelUpdatedCoroutine != null)
        {
            StopCoroutine(levelUpdatedCoroutine);
            levelUpdatedCoroutine = null;
        }

        levelUpdatedCoroutine = StartCoroutine(ShowLevelUpdatedCoroutine());
    }

    IEnumerator ShowLevelUpdatedCoroutine()
    {
        newLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        newLevelText.gameObject.SetActive(false);

        levelUpdatedCoroutine = null;
    }
}
