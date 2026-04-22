using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPointsControllerUI : MonoBehaviour
{
    CharacterLevelController levelController;

    [SerializeField] Slider pointsSlider;
    [SerializeField] TextMeshProUGUI currentXPText;
    [SerializeField] TextMeshProUGUI xpToNextLevelText;
    
    public void Init(CharacterLevelController levelController)
    {
        this.levelController = levelController;

        SetSliderValues();
        GetCurrentPointsInfo();

        CharacterLevelController.XpGained += OnPointsDropped;
        CharacterLevelController.NewLevelReachedWithMessage += OnNewLevelReachedWithMessage;
        CharacterLevelController.NewLevelReached += OnNewLevelReached;
      
    }

    private void OnDisable()
    {

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
    }

    private void OnNewLevelReachedWithMessage()
    {
        OnNewLevelReached();
      
    }

  
}
