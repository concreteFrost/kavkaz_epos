using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;

    [SerializeField] Button levelControllerBtn;
    [SerializeField] Button gameSettingsBtn;
    [SerializeField] Button quitToMainMenuBtn;

    private Action openLevelController;

    private List<Selectable> allSelectables = new List<Selectable>();

    private void Awake()
    {
        levelControllerBtn.onClick.AddListener(ShowLevelController);
        quitToMainMenuBtn.onClick.AddListener(QuitToMainMenu);

        allSelectables = new List<Selectable>
        {
            levelControllerBtn,
            gameSettingsBtn,
            quitToMainMenuBtn,
        };
    }

    private void OnDestroy()
    {
        levelControllerBtn.onClick.RemoveListener(ShowLevelController);
        quitToMainMenuBtn.onClick.RemoveListener(QuitToMainMenu);
    }

    public void SetLevelControllerAction(Action openLevelController)
    {
        this.openLevelController = openLevelController;
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
        openLevelController?.Invoke();
    }
}
