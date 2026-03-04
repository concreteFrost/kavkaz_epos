using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuOptionsUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;

    [SerializeField] Button levelControllerBtn;
    [SerializeField] Button gameSettingsBtn;

    PlayerLevelControllerUI levelControllerUI;

    private List<Selectable> allSelectables;

    public void Init(PlayerLevelControllerUI levelControllerUI)
    {
        this.levelControllerUI = levelControllerUI; 

        levelControllerBtn.onClick.RemoveAllListeners();
        levelControllerBtn.onClick.AddListener(ShowLevelController);


        allSelectables = new List<Selectable>
        {
            levelControllerBtn,
            gameSettingsBtn
        };

        ToggleMenuOptions(false);

    }


    public void ToggleMenuOptions(bool isVisible)
    {
        wrapper.SetActive(isVisible);

        UINavigationUtils.ClampVerticalNavigation(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(allSelectables[0].gameObject));
    }

    public void ShowLevelController()
    {
        levelControllerUI.ToggleLevelControllerPanel(true);
    }
}
