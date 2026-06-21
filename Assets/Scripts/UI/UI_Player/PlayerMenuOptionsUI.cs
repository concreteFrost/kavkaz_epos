using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuOptionsUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;

    [SerializeField] Button levelControllerBtn;
    [SerializeField] Button gameSettingsBtn;
    [SerializeField] Button quitToMainMenuBtn;

    PlayerLevelControllerUI levelControllerUI;

    private List<Selectable> allSelectables = new List<Selectable>();

    public void Init(PlayerLevelControllerUI levelControllerUI)
    {
        this.levelControllerUI = levelControllerUI; 


        levelControllerBtn.onClick.AddListener(ShowLevelController);

        quitToMainMenuBtn.onClick.AddListener(QuitToMainMenu);

        allSelectables = new List<Selectable>
        {
            levelControllerBtn,
            gameSettingsBtn,
            quitToMainMenuBtn,
        };


    }


    private void QuitToMainMenu()
    {
        SceneTransitionManager.Instance.LoadMainMenu();    
    }

    public void ToggleMenuOptions(bool isVisible)
    {
        wrapper.SetActive(isVisible);

        if (!isVisible) return;

        UINavigationUtils.ClampHorizontalNavigation(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(allSelectables[0].gameObject));
    }

    public void ShowLevelController()
    {
        levelControllerUI.ToggleLevelControllerPanel(true);
    }
}
